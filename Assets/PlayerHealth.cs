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

    ScoreHUD scoreHud;
    
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
            int[] score = new int[LobbyPlayerList._instance._players.Count];
            for (int i = 0; i < score.Length; ++i)
            {
                LobbyPlayer localPlayer = LobbyPlayerList._instance._players[i] as LobbyPlayer;
                score[i] = localPlayer.score;
                if(score[i] == 3)
                {
                    StartCoroutine(LobbyManager.s_Singleton.ReturnToLoby());
                }
            }
            RpcUpdateHudScore(score);

        }
    }

    [ClientRpc]
    void RpcUpdateHudScore(int[] score)
    {
        scoreHud = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreHUD>();
        scoreHud.UpdateScore(score);
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
            transform.position = new Vector3(70, 0, 60);
        }
    }
}
