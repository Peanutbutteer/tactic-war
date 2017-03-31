using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class PlayerHealth : NetworkBehaviour
{
	public const int maxHealth = 10;

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;

	public RectTransform healthBar;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
	{
		
		if (!isServer)
			return;

		currentHealth -= amount;
		if (currentHealth <= 0)
		{
            currentHealth = maxHealth;
			int id = gameObject.GetComponent<PlayerMageController>().playerId;
			LobbyPlayer player = LobbyPlayerList._instance._players[id];
			player.IncrementScore();
            RpcRespawn();
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
	}

    IEnumerator PlayerDeath()
    {
        anim.SetBool("Death", true);
        yield return new WaitForSeconds(4f);
        anim.SetBool("Death", false);
        transform.position = new Vector3(70, 0, 60);
    }

    IEnumerator EndGame()
	{
		yield return new WaitForSeconds(5f);
		LobbyManager.s_Singleton.ServerChangeScene(LobbyManager.s_Singleton.offlineScene);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.E)) {
			CmdEndGame();
		}
	}

	[Command]
	void CmdEndGame() {
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

	void OnChangeHealth(int health)
	{
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

	//void OnParticleCollision(GameObject other)
	//{
	//    if (!isServer)
	//        return;

	//    if (other.gameObject.tag == "Attack")
	//    {
	//        RpcRespawn();
	//    }
	//}

	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
            StartCoroutine(PlayerDeath());
		}
	}
}
