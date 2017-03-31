using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoseBoom : NetworkBehaviour
{

	// Use this for initialization
	void Start () {
        StartCoroutine(DisableCollider());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<Collider>().enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
       
        if (!isServer)
            return;
        var hit = collision.gameObject;
        var playerHealth = hit.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(20);
        }

    }

}
