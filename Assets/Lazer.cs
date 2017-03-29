using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Lazer : NetworkBehaviour
{
    public float lazerVelocity = 0.1f;
    private Rigidbody lazer;
    // Use this for initialization
    void Start()
    {
        lazer = GetComponent<Rigidbody>();
    }

    void Update()
    {
        lazer.velocity = transform.forward * lazerVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
        if (!isServer)
            return;
        var hit = collision.gameObject;
        var playerHealth = hit.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }
}
