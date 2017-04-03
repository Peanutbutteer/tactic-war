using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Firefog : NetworkBehaviour
{
    [Range(0f, 10f)]
    public int damage = 5;

    private bool burned = false;

    void Start()
    {
        
    }

    void Update()
    {
        StartCoroutine(destroyCollider());
    }

    IEnumerator destroyCollider()
    {
        yield return new WaitForSeconds(5.0f);
        GetComponent<Collider>().enabled = false;
    }


    void OnTriggerStay(Collider other)
    {
        if (!isServer)
            return;
        if (other.gameObject.tag == ("Player") && !burned)
        {
            burned = true;
            StartCoroutine(Attack());
            var hit = other.gameObject;
            var playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        burned = false;
    }
}
