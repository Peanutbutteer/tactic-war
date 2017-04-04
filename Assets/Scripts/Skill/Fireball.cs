using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class Fireball : NetworkBehaviour {
    public AudioClip soundFireballHit;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    [Range(0f, 40f)]
    public int damage = 10;

    private AudioSource source;
    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }


    void OnParticleCollision(GameObject other)
    {
        source.PlayOneShot(soundFireballHit, volume);

        if (!isServer)
            return;
        var hit = other.gameObject;
        var playerHealth = hit.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        
    }
}
