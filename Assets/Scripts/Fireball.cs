using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class Fireball : NetworkBehaviour {
    public AudioClip audioFireballHit;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }


    void OnParticleCollision(GameObject other)
    {
        AudioSource.PlayClipAtPoint(audioFireballHit, other.transform.position , 1000f);

        if (!isServer)
            return;
        var hit = other.gameObject;
        var playerHealth = hit.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
        
    }
}
