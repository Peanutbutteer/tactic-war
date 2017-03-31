using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using UnityEngine.UI;
public class GameManager : NetworkBehaviour
{
	public static GameManager s_Singleton;

	// Use this for initialization
	void Start()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (var item in players)
		{
			if (item.GetComponent<NetworkIdentity>().hasAuthority)
			{
				int id = item.GetComponent<PlayerMageController>().playerId;
				GameObject scoreHud = GameObject.FindGameObjectWithTag("Team");
				Text text = scoreHud.GetComponent<Text>();
				if (text != null)
				{
					if (id != 0)
					{
						text.text = "BLUE";
						text.color = new Color32(108, 166, 200, 255);
					}
				}
			}
		}

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

	public void UpdateHudScore()
	{
		int[] score = new int[LobbyPlayerList._instance._players.Count];
		for (int i = 0; i < score.Length; ++i)
		{
			LobbyPlayer localPlayer = LobbyPlayerList._instance._players[i] as LobbyPlayer;
			score[i] = localPlayer.score;
			if (score[i] == 3)
			{
				StartCoroutine(EndGame());
				RpcShowHudWinner(i);
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
		RpcShowHudWinner(0);
	}

	[ClientRpc]
	void RpcShowHudWinner(int winnerNo)
	{
		EndGamePanel[] endGamePanel = Resources.FindObjectsOfTypeAll<EndGamePanel>();
		if (endGamePanel[0] != null)
		{
			endGamePanel[0].UpdateWinnerText(winnerNo);
			endGamePanel[0].Show();
		}
	}

	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(10f);
		LobbyManager.s_Singleton.ServerChangeScene(LobbyManager.s_Singleton.offlineScene);
	}

}
