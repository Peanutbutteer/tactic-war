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

		public event Action matchDropped;

		public static bool s_IsServer
		{
			get
			{
				return NetworkServer.active;
			}
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
		public LobbyCountdownPanel countdownPanel;

		protected RectTransform currentPanel;


		//Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
		//of players, so that even client know how many player there is.
		[HideInInspector]
		public int _playerNumber = 0;

		protected bool _disconnectServer = false;

		public bool isInGame = false;

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
			s_Singleton = this;
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

		protected virtual void OnDestroy()
		{
			if (s_Singleton == this)
			{
				s_Singleton = null;
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

		public void ShowInfoPopup(string info)
		{
			if (infoPanel != null)
			{
				infoPanel.Display(info, "Cancel", null);
			}
		}

		public void HideInfoPopup()
		{
			if (infoPanel != null)
			{
				infoPanel.gameObject.SetActive(false);
			}
		}


		public override void OnLobbyClientSceneChanged(NetworkConnection conn)
		{
			if (SceneManager.GetSceneAt(0).name == lobbyScene)
			{
				ChangeTo(mainMenuPanel);
			}
			else
			{
				ChangeTo(null);
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
			infoPanel.Display("Kicked by Server", "Close", null);
			netMsg.conn.Disconnect();
		}

		//===================

		public override void OnStartHost()
		{
			base.OnStartHost();
		}

		public override void OnStopHost()
		{
			base.OnStopHost();
		}

		public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
		{
			base.OnMatchCreate(success, extendedInfo, matchInfo);
			_currentMatchID = (System.UInt64)matchInfo.networkId;
		}

		public override void OnDestroyMatch(bool success, string extendedInfo)
		{
			base.OnDestroyMatch(success, extendedInfo);
			Debug.Log("DestroyMatch OK T^T");
			if (_disconnectServer)
			{
				StopMatchMaker();
				StopHost();
			}
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
			}
		}

		public override void OnClientConnect(NetworkConnection conn)
		{
			base.OnClientConnect(conn);

			state = GameState.InLobby;
			infoPanel.gameObject.SetActive(false);

			Debug.Log("OnClientConnect");

			conn.RegisterHandler(MsgKicked, KickedMessageHandler);

			if (!NetworkServer.active)
			{//only to do on pure client (not self hosting client)
				ChangeTo(lobbyPanel);
			}
		}


		public override void OnClientDisconnect(NetworkConnection conn)
		{
			// Call on client when client is disconnect
			base.OnClientDisconnect(conn);

			if (clientDisconnected != null)
			{
				clientDisconnected(conn);
			}

			//infoPanel.Display("Disconnect from Server", "Cancel", null);
			//ChangeTo(mainMenuPanel);
		}

		public override void OnLobbyClientDisconnect(NetworkConnection conn)
		{
			base.OnLobbyClientDisconnect(conn);
		}

		public override void OnClientError(NetworkConnection conn, int errorCode)
		{
			ChangeTo(mainMenuPanel);
			infoPanel.Display("Cient error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()), "Close", null);
		}

		public void DisconnectAndReturnToMenu()
		{
			Disconnect();
			ChangeTo(mainMenuPanel);
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
