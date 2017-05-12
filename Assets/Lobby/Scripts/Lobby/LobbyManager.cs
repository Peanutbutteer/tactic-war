using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;
using System;


namespace Prototype.NetworkLobby
{
	public enum MenuPage
	{
		Home,
		Lobby
	}


	public enum SceneChangeMode
	{
		None,
		Game,
		Menu
	}

	public enum GameState
	{
		Inactive,
		Pregame,
		Connecting,
		InLobby,
		InGame
	}


	public class LobbyManager : NetworkLobbyManager
	{
		static short MsgKicked = MsgType.Highest + 1;

		static public LobbyManager s_Singleton;

		public GameState state
		{
			get;
			set;
		}

		public static MenuPage s_ReturnPage;

		public event Action<NetworkConnection> clientConnected;

		public event Action<NetworkConnection, int> serverError;

		public event Action<NetworkConnection, int> clientError;

        public event Action everyOneLeftGame;

        public event Action matchDropped;

		public static bool s_IsServer
		{
			get
			{
				return NetworkServer.active;
			}
		}

		private SceneChangeMode m_SceneChangeMode = SceneChangeMode.None;

		public static bool s_InstanceExists
		{
			get { return s_Singleton != null; }
		}


		[Header("Unity UI Lobby")]
		[Tooltip("Time in second between all players ready & match start")]
		public float prematchCountdown = 5.0f;

		[Space]
		[Header("UI Reference")]

		public RectTransform mainMenuPanel;
		public RectTransform lobbyPanel;

		public RectTransform createRoomPanel;

		public LobbyInfoPanel infoPanel;

		protected RectTransform currentPanel;


		//Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
		//of players, so that even client know how many player there is.
		[HideInInspector]
		public int _playerNumber = 0;

		protected ulong _currentMatchID;

		protected LobbyHook _lobbyHooks;

		public event Action<NetworkConnection> clientDisconnected;

		void Start()
		{
			LoadingModal modal = LoadingModal.s_Instance;
			if (modal != null)
			{
				modal.FadeOut();
			}
			_lobbyHooks = GetComponent<Prototype.NetworkLobby.LobbyHook>();
			currentPanel = mainMenuPanel;

			GetComponent<Canvas>().enabled = true;

			DontDestroyOnLoad(gameObject);

			switch (s_ReturnPage)
			{
				case MenuPage.Home:
				default:
					ShowDefaultPanel();
					break;
				case MenuPage.Lobby:
					ShowLobbyPanel();
					break;
			}
		}

		public override void OnLobbyClientSceneChanged(NetworkConnection conn)
		{
			if (SceneManager.GetSceneAt(0).name == lobbyScene)
			{
				ShowDefaultPanel();
				Disconnect();
			}
			else
			{
				HideAllPanel();
			}
		}

		protected virtual void Awake()
		{
			if (s_Singleton != null)
			{
				Destroy(gameObject);
			}
			else
			{
				s_Singleton = this;
			}
		}

		protected virtual void OnDestroy()
		{
			if (s_Singleton == this)
			{
				s_Singleton = null;
			}
		}

		void Update()
		{
			if (m_SceneChangeMode != SceneChangeMode.None)
			{
				if (m_SceneChangeMode == SceneChangeMode.Menu)
				{
					if (state != GameState.Inactive)
					{
						ServerChangeScene(lobbyScene);
					}
					else
					{
						ShowDefaultPanel();
					}
				}
				else
				{
					//LoadingModal modal = LoadingModal.s_Instance;
					//if (modal != null)
					//{
					//	modal.FadeOut();
					//}
				}
				m_SceneChangeMode = SceneChangeMode.None;
			}
		}


		public void ShowDefaultPanel()
		{
			ChangeTo(mainMenuPanel);
		}

		public void ShowLobbyPanel()
		{
			ChangeTo(lobbyPanel);
		}

		public void HideAllPanel()
		{
			ChangeTo(null);
		}

		public void ShowInfoPopup(string info)
		{
			if (infoPanel != null)
			{
				infoPanel.Display(info, "Cancel", null);
			}
		}

		public void ShowInfoPopup(string info, string des)
		{
			if (infoPanel != null)
			{
				infoPanel.Display(info, des, null);
			}
		}

		public void ShowInfoPopup(string info, string des, UnityEngine.Events.UnityAction callback)
		{
			if (infoPanel != null)
			{
				infoPanel.Display(info, des, callback);
			}
		}

		public void HideInfoPopup()
		{
			if (infoPanel != null)
			{
				infoPanel.gameObject.SetActive(false);
			}
		}

		public void ChangeTo(RectTransform newPanel)
		{
			if (currentPanel != null)
			{
				currentPanel.gameObject.SetActive(false);
			}
			currentPanel = newPanel;
			if (newPanel != null)
			{
				newPanel.gameObject.SetActive(true);
			}
		}

		class KickMsg : MessageBase { }
		public void KickPlayer(NetworkConnection conn)
		{
			conn.Send(MsgKicked, new KickMsg());
		}

		public void ShowConnectingModal(bool reconnectMatchmakingClient)
		{
			infoPanel.Display("Connecting...", "Cancel", () =>
			{
				if (reconnectMatchmakingClient)
				{
					Disconnect();
					StartMatchingmakingClient();
				}
				else
				{
					Disconnect();
				}
			});
		}

		public void KickedMessageHandler(NetworkMessage netMsg)
		{
			ShowInfoPopup("Kicked by Server", "Close");
			netMsg.conn.Disconnect();
		}

		public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
		{
			base.OnMatchCreate(success, extendedInfo, matchInfo);
			_currentMatchID = (System.UInt64)matchInfo.networkId;

			if (success)
			{
				state = GameState.InLobby;
			}
			else
			{
				state = GameState.Inactive;
				ShowInfoPopup("Failed to create game.");
			}
		}

		public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
		{
			base.OnMatchJoined(success, extendedInfo, matchInfo);
			Debug.Log("OnMatchJoined");

			if (success)
			{
				HideInfoPopup();
				ShowInfoPopup("Entering lobby...", "Cancel", () =>
				{
					Disconnect();
					ShowDefaultPanel();
				});
				state = GameState.InLobby;
			}
			else
			{
				ShowInfoPopup("Failed to join game.");
				state = GameState.Pregame;
			}
		}

		public override void OnDestroyMatch(bool success, string extendedInfo)
		{
			base.OnDestroyMatch(success, extendedInfo);
			StopMatchMaker();
			StopHost();
		}

		//allow to handle the (+) button to add/remove player
		public void OnPlayersNumberModified(int count)
		{
			_playerNumber += count;

		}

		// ----------------- Server callbacks ------------------

		//we want to disable the button JOIN if we don't have enough player
		//But OnLobbyClientConnect isn't called on hosting player. So we override the lobbyPlayer creation
		public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
		{
			GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

			LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();
			newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);

			for (int i = 0; i < lobbySlots.Length; ++i)
			{
				LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

				if (p != null)
				{
					p.RpcUpdateRemoveButton();
					p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
				}
			}

			return obj;
		}

		public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
		{
			for (int i = 0; i < lobbySlots.Length; ++i)
			{
				LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

				if (p != null)
				{
					p.RpcUpdateRemoveButton();
					p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
				}
			}
        }


        public override void OnLobbyServerDisconnect(NetworkConnection conn)
		{
			for (int i = 0; i < lobbySlots.Length; ++i)
			{
				LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

				if (p != null)
				{
					p.RpcUpdateRemoveButton();
					p.ToggleJoinButton(numPlayers >= minPlayers);
				}
			}

		}

		public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
		{
			//This hook allows you to apply state data from the lobby-player to the game-player
			//just subclass "LobbyHook" and add it to the lobby object.

			if (_lobbyHooks)
				_lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

			return true;
		}



		// --- Countdown management

		public override void OnLobbyServerPlayersReady()
		{
			bool allready = true;
			for (int i = 0; i < lobbySlots.Length; ++i)
			{
				if (lobbySlots[i] != null)
					allready &= lobbySlots[i].readyToBegin;
			}

			if (allready)
			{
				for (int i = 0; i < lobbySlots.Length; ++i)
				{
					if (lobbySlots[i] != null)
					{
						(lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown();
					}
				}
				ServerChangeScene(playScene);
				m_SceneChangeMode = SceneChangeMode.Game;
				state = GameState.InGame;
			}
		}

		public override void OnClientConnect(NetworkConnection conn)
		{
			base.OnClientConnect(conn);

			state = GameState.InLobby;
			HideInfoPopup();
			conn.RegisterHandler(MsgKicked, KickedMessageHandler);

		}


		public override void OnClientDisconnect(NetworkConnection conn)
		{
			// Call on client when client is disconnect
			base.OnClientDisconnect(conn);

			if (clientDisconnected != null)
			{
				clientDisconnected(conn);
			}
		}

		public override void OnLobbyClientDisconnect(NetworkConnection conn)
		{
			base.OnLobbyClientDisconnect(conn);
		}

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);

            if (state == GameState.InGame && LobbyPlayerList._instance._players.Count <= minPlayers)
            {
                everyOneLeftGame();
            }
        }


        public override void OnClientError(NetworkConnection conn, int errorCode)
		{
			base.OnClientError(conn, errorCode);

			if (clientError != null)
			{
				clientError(conn, errorCode);
			}
		}

		public override void OnDropConnection(bool success, string extendedInfo)
		{
			base.OnDropConnection(success, extendedInfo);

			if (matchDropped != null)
			{
				matchDropped();
			}
		}

		public override void OnServerError(NetworkConnection conn, int errorCode)
		{
			base.OnClientDisconnect(conn);

			if (serverError != null)
			{
				serverError(conn, errorCode);
			}
		}

		public void DisconnectAndReturnToMenu()
		{
			Disconnect();
			ReturnToMenu(MenuPage.Home);
		}

		public void ReturnToMenu(MenuPage returnPage)
		{
			s_ReturnPage = returnPage;

			m_SceneChangeMode = SceneChangeMode.Menu;

			if (s_IsServer && state == GameState.InGame)
			{
				for (int i = 0; i < lobbySlots.Length; ++i)
				{
					if (lobbySlots[i] != null)
					{
						(lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown();
					}
				}
			}

			else
			{
				LoadingModal loading = LoadingModal.s_Instance;

				if (loading != null)
				{
					loading.FadeOut();
				}
			}
		}

		public override void OnStartClient(NetworkClient lobbyClient)
		{
			base.OnStartClient(lobbyClient);
			// Change to lobby on Create Game
			ChangeTo(lobbyPanel);

		}

		public IEnumerator ReturnToLoby()
		{
			yield return new WaitForSeconds(3.0f);
			ServerReturnToLobby();
		}

		public void StartMatchingmakingClient()
		{
			state = GameState.Pregame;
			StartMatchMaker();
		}

		public void Disconnect()
		{
			switch (state)
			{
				case GameState.Pregame:
					if (s_IsServer)
					{
						Debug.LogError("Server should never be in this state.");
					}
					else
					{
						StopMatchMaker();
					}
					break;

				case GameState.Connecting:
					if (s_IsServer)
					{
						StopMatchMaker();
						StopHost();
						matchInfo = null;
					}
					else
					{
						StopMatchMaker();
						StopClient();
						matchInfo = null;
					}
					break;

				case GameState.InLobby:
				case GameState.InGame:
					if (s_IsServer)
					{
						if (matchMaker != null && matchInfo != null)
						{
							matchMaker.DestroyMatch(matchInfo.networkId, 0, (success, info) =>
								{
									if (!success)
									{
										Debug.LogErrorFormat("Failed to terminate matchmaking game. {0}", info);
									}
									StopMatchMaker();
									StopHost();

									matchInfo = null;
								});
						}
						else
						{
							Debug.LogWarning("No matchmaker or matchInfo despite being a server in matchmaking state.");

							StopMatchMaker();
							StopHost();
							matchInfo = null;
						}
					}
					else
					{
						if (matchMaker != null && matchInfo != null)
						{
							matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, (success, info) =>
								{
									if (!success)
									{
										Debug.LogErrorFormat("Failed to disconnect from matchmaking game. {0}", info);
									}
									StopMatchMaker();
									StopClient();
									matchInfo = null;
								});
						}
						else
						{
							Debug.LogWarning("No matchmaker or matchInfo despite being a client in matchmaking state.");

							StopMatchMaker();
							StopClient();
							matchInfo = null;
						}
					}
					break;
			}

			state = GameState.Inactive;
		}

	}
}
