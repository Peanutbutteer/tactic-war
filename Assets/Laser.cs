using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Laser : NetworkBehaviour
{
    public float laserVelocity = 0.1f;
    private Rigidbody laser;
    // Use this for initialization
    void Start()
    {
        laser = GetComponent<Rigidbody>();
    }

    void Update()
    {
        laser.velocity = transform.forward * laserVelocity;
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
