using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class PlayerHealth : NetworkBehaviour
{
    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    ScoreHUD scoreHud;
    

    [Server]
    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;
        
        currentHealth -= 100;
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;

            // called on the Server, but invoked on the Clients
            //
            int id = gameObject.GetComponent<PlayerMageController>().playerId;
            LobbyPlayer player = LobbyManager.s_Singleton.lobbySlots[id] as LobbyPlayer;
            player.IncrementScore();
            Debug.Log("ID: "+id);
            Debug.Log("Score: " + player);
            int[] score = new int[LobbyManager.s_Singleton.lobbySlots.Length];
            for (int i = 0; i < score.Length; ++i)
            {
                LobbyPlayer localPlayer = LobbyManager.s_Singleton.lobbySlots[id] as LobbyPlayer;
                Debug.Log(localPlayer.playerName + " " + localPlayer.score);
                score[i] = localPlayer.score;
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
