using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prototype.NetworkLobby
{
public class MessageUI : MonoBehaviour {

	public RectTransform settingsPanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickOpenSettings() {
		settingsPanel.gameObject.SetActive(true);
	}

	public void OnClickQuitGame() {
		if (LobbyManager.s_Singleton != null) {
			LobbyManager.s_Singleton.DisconnectAndReturnToMenu();
		}
	}
}

}
