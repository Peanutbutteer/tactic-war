using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace Prototype.NetworkLobby
{
	public class MessageUI : MonoBehaviour
	{
		private LobbyManager lobbyManager;
		public SettingPanel settingsPanel;
		public ErrorModal errorPanel;
        public RectTransform blackPanel;

		// Use this for initialization
		void Start()
		{
			lobbyManager = LobbyManager.s_Singleton;
			if (lobbyManager != null)
			{
				lobbyManager.clientDisconnected += OnDisconnect;
				lobbyManager.matchDropped += OnDrop;
				lobbyManager.clientError += OnError;
				lobbyManager.serverError += OnError;
			}
		}

		void OnDestroy()
		{
			if (lobbyManager != null)
			{
				lobbyManager.clientDisconnected -= OnDisconnect;
				lobbyManager.matchDropped -= OnDrop;
				lobbyManager.clientError -= OnError;
				lobbyManager.serverError -= OnError;

			}
		}

		public void OnClickOpenSettings()
		{
			settingsPanel.Display();
		}

		public void OnClickQuitGame()
		{
			if (lobbyManager != null)
			{
				settingsPanel.close();
                blackPanel.gameObject.SetActive(true);
                lobbyManager.DisconnectAndReturnToMenu();
            }
		}

		private void ShowErrorPanel()
		{
			if (lobbyManager != null)
			{
				ErrorModal.s_Instance.SetupTimer(5f, null);
				ErrorModal.s_Instance.Show();
                //StartCoroutine(WaitforEndGame());
                //LobbyManager.s_Singleton.ServerChangeScene(LobbyManager.s_Singleton.offlineScene);
            }
		}

        IEnumerator WaitforEndGame()
        {
            yield return new WaitForSeconds(5f);
            LobbyManager.s_Singleton.DisconnectAndReturnToMenu();
            blackPanel.gameObject.SetActive(true);
        }

        private void OnDrop()
		{
			ShowErrorPanel();
		}

		private void OnError(NetworkConnection connection, int errorCode)
		{
			ShowErrorPanel();
		}

		protected virtual void OnDisconnect(UnityEngine.Networking.NetworkConnection conn)
		{
			ShowErrorPanel();
		}

	}

}
