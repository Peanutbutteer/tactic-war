using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.NetworkLobby
{

	public class MainMenu : MonoBehaviour {

    	public LobbyManager lobbyManager;
		public RectTransform lobbyServerList;
		public RectTransform createRoomPanel;

		public void OnClickOpenServerList()
   		{
			lobbyManager.StartMatchMaker();
			lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
      	  	lobbyManager.ChangeTo(lobbyServerList);
		}

		public void OnClickCreateRoom()
		{
			lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
			lobbyManager.ChangeTo(createRoomPanel);
		}
	}

}
