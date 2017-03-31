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
			if (player.score < 3) player.IncrementScore();
			RpcRespawn();
			GameManager.s_Singleton.UpdateHudScore();
		}
	}

	IEnumerator PlayerDeath()
	{
		anim.SetBool("Death", true);
		yield return new WaitForSeconds(4f);
		anim.SetBool("Death", false);
		transform.position = new Vector3(70, 0, 60);
	}

	void OnChangeHealth(int health)
	{
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			StartCoroutine(PlayerDeath());
		}
	}
}
