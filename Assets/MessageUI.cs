﻿using System.Collections;
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
				lobbyManager.DisconnectAndReturnToMenu();
			}
		}

		private void ShowErrorPanel()
		{
			if (lobbyManager != null)
			{
				errorPanel.SetupTimer(10f, lobbyManager.DisconnectAndReturnToMenu);
				errorPanel.Show();
			}
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
