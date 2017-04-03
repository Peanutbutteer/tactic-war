using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class PlayerHealth : NetworkBehaviour
{
	public const int maxHealth = 100;
    public AudioClip spawnSound;

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;
    [SyncVar(hook = "OnChangeSpawnIndex")]
    private int spawnIndex = 2;

    public RectTransform healthBar;

    private NetworkStartPosition[] spawnPoints;
    
    private Animator anim;

	void Start()
	{
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }

        anim = GetComponent<Animator>();
	}

	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;

        if(anim.GetBool("Death") != true)
		    currentHealth -= amount;

		if (currentHealth <= 0)
		{
			int id = gameObject.GetComponent<PlayerMageController>().playerId;
			LobbyPlayer player = LobbyPlayerList._instance._players[id];
			if (player.score < 3) player.IncrementScore();
			RpcRespawn();
            currentHealth = maxHealth;
            GameManager.s_Singleton.UpdateHudScore();
		}
	}

	IEnumerator PlayerDeath()
	{
		anim.SetBool("Death", true);
		yield return new WaitForSeconds(4f);
		anim.SetBool("Death", false);

        Vector3 spawnPoint = Vector3.zero;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            spawnPoint = spawnPoints[spawnIndex].transform.position;
            spawnIndex++;
            if(spawnIndex >= spawnPoints.Length)
            {
                spawnIndex = 0;
            }
        }
        GetComponent<AudioSource>().PlayOneShot(spawnSound);
        transform.position = spawnPoint;
    }

	void OnChangeHealth(int health)
	{
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

    void OnChangeSpawnIndex(int index)
	{
        spawnIndex += index;

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
