using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.NetworkLobby
{

	public class MainMenu : MonoBehaviour {

    	public LobbyManager lobbyManager;
		public RectTransform lobbyServerList;
		public RectTransform createRoomPanel;
		public RectTransform aboutPanel;

		public void OnClickOpenServerList()
   		{
			lobbyManager.StartMatchingmakingClient();
      	  	lobbyManager.ChangeTo(lobbyServerList);
		}

		public void OnClickCreateRoom()
		{
			lobbyManager.ChangeTo(createRoomPanel);
		}

		public void OnClickAboutUs()
		{
			lobbyManager.ChangeTo(aboutPanel);
		}

		public void OnClickQuitGame()
		{
			Application.Quit();
		}
	}

}
