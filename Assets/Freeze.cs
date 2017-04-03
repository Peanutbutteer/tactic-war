using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Freeze : NetworkBehaviour
{
    public AudioClip soundFreezeHit;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    [Range(0f, 40f)]
    public float freezeVelocity = 0.1f;
    [Range(0f, 40f)]
    public int damage = 10;

    private AudioSource source;
    private Rigidbody freeze;
    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        freeze = GetComponent<Rigidbody>();
    }

    void Update()
    {
        freeze.velocity = transform.forward * freezeVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        transform.GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = false;
        source.PlayOneShot(soundFreezeHit, volume);
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
            playerHealth.TakeDamage(damage);
        }
    }


}
