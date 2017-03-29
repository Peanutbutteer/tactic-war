using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Freeze : NetworkBehaviour
{
    public float freezeVelocity = 0.1f;
    private Rigidbody freeze;
    // Use this for initialization
    void Start()
    {
        freeze = GetComponent<Rigidbody>();
    }

    void Update()
    {
        freeze.velocity = transform.forward * freezeVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
        if (!isServer)
            return;
        var hit = collision.gameObject;
        var playerFreeze = hit.GetComponent<PlayerMageController>();
        if(playerFreeze != null) {
            playerFreeze.RpcFreezing(playerFreeze.playerId);
        }
        var playerHealth = hit.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }


}
