using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using UnityEngine.UI;
public class GameManager : NetworkBehaviour
{
	public static GameManager s_Singleton;
    public RectTransform blackPanel;

	// Use this for initialization
	void Start()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (var item in players)
		{
			if (item.GetComponent<NetworkIdentity>().hasAuthority)
			{
				int id = item.GetComponent<PlayerMageController>().playerId;
				string name = item.GetComponent<PlayerMageController> ().playerName;
				GameObject scoreHud = GameObject.FindGameObjectWithTag("Team");
				Text text = scoreHud.GetComponent<Text>();
				if (text != null)
				{
					text.text = name;
                    if (id == 0)
                    {
                        text.color = new Color32(108, 166, 200, 255);
                    }
				}
			}
		}
        LobbyManager.s_Singleton.everyOneLeftGame += () => {
            ErrorModal.s_Instance.SetupTimer(5f, null);
            ErrorModal.s_Instance.Show();
            StartCoroutine(WaitforEndGame());
        };


    }
    IEnumerator WaitforEndGame()
    {
        yield return new WaitForSeconds(5f);
        LobbyManager.s_Singleton.DisconnectAndReturnToMenu();
        blackPanel.gameObject.SetActive(true);
    }

    void Awake()
	{
		if (s_Singleton == null)
		{
			s_Singleton = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			CmdEndGame();
		}
	}

	public void UpdateHudScore(bool isEndGame)
	{
		int max = 0;
		int maxId = -1;
		string playerName = "";
		int[] score = new int[LobbyPlayerList._instance._players.Count];
		for (int i = 0; i < score.Length; ++i)
		{
			LobbyPlayer localPlayer = LobbyPlayerList._instance._players[i] as LobbyPlayer;
			score[i] = localPlayer.score;
			if (score [i] > max) {
				maxId = i;
				max = score [i];
				playerName = localPlayer.playerName;
			}
			if (score[i] >= 3)
			{
				StartCoroutine(EndGame());
				RpcShowHudWinner(true,i,localPlayer.playerName);
			}
		}
		if (isEndGame) {
			if (maxId != -1) {
				RpcShowHudWinner (true, maxId, playerName);
			} else {
				RpcShowHudWinner (false, 0, "Draw");
			}
		}
		RpcUpdateHudScore(score);
	}

	[ClientRpc]
	void RpcUpdateHudScore(int[] score)
	{
		GameObject[] scoreHud = GameObject.FindGameObjectsWithTag("Score");
		foreach (GameObject hud in scoreHud)
		{
			ScoreHUD scoreHUD = hud.GetComponent<ScoreHUD>();
			if (scoreHUD != null)
			{
				scoreHUD.UpdateScore(score);
			}
		}
	}

	[Command]
	public void CmdEndGame()
	{
		StartCoroutine(EndGame());
		UpdateHudScore (true);
	}

	[ClientRpc]
	void RpcShowHudWinner(bool hasWinner,int winnerNo,string winnerName)
	{
		EndGamePanel[] endGamePanel = Resources.FindObjectsOfTypeAll<EndGamePanel>();
		if (endGamePanel[0] != null)
		{
			endGamePanel[0].UpdateWinnerText(hasWinner,winnerNo,winnerName);
			endGamePanel[0].Show();
		}
	}

	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(10f);
        blackPanel.gameObject.SetActive(true);
        LobbyManager.s_Singleton.ServerChangeScene(LobbyManager.s_Singleton.offlineScene);
    }

}
