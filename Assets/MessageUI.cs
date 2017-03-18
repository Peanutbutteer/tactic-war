using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prototype.NetworkLobby
{
	public class MessageUI : MonoBehaviour
	{
		private LobbyManager lobbyManager;
		public SettingPanel settingsPanel;
		public ErrorModal errorPanel;

		// Use this for initialization
		void Start()
		{
			lobbyManager = LobbyManager.s_Singleton;
			if (lobbyManager != null)
			{
				lobbyManager.clientDisconnected += OnDisconnect;
			}
		}

		void OnDestroy()
		{
			if (lobbyManager != null)
			{
				lobbyManager.clientDisconnected -= OnDisconnect;
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
				lobbyManager.DisconnectAndReturnToMenu();
			}
		}

		protected virtual void OnDisconnect(UnityEngine.Networking.NetworkConnection conn)
		{
			if (lobbyManager != null)
			{
				errorPanel.SetupTimer(2f, lobbyManager.DisconnectAndReturnToMenu);
				errorPanel.Show();
				Debug.Log("errorPanel");
			}
		}
	}

}
