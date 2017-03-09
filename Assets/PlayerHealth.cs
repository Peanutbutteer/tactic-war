using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        Debug.Log("1");
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;

            // called on the Server, but invoked on the Clients
            RpcRespawn();
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
            transform.position = new Vector3(70, 0, 60);
        }
    }
}
