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
    [SerializeField]
    public int spawnIndex = 1;

    public RectTransform healthBar;

    private NetworkStartPosition[] spawnPoints;
    
    private Animator anim;

	private Vector3 spawnPoint = Vector3.zero;

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
            foreach(LobbyPlayer play in LobbyPlayerList._instance._players)
            {
                if(play.playerId != id)
                {
                    id = play.playerId;
                    break;
                }
            }
            LobbyPlayer player = LobbyPlayerList._instance._players[id];
			if (player.score < 3) player.IncrementScore();
			RpcRespawn();
            currentHealth = maxHealth;
            GameManager.s_Singleton.UpdateHudScore(false);
		}
	}

	IEnumerator PlayerDeath()
	{
        // Client side
        anim.SetBool("Death", true);
		yield return new WaitForSeconds(3f);
		anim.SetBool("Death", false);
        spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        GetComponent<AudioSource>().PlayOneShot(spawnSound);
		transform.position = spawnPoint;
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
