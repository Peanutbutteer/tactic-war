using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Staff : MonoBehaviour
{
    public GameObject player;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        var playerHealth = other.GetComponent<PlayerHealth>();
        Debug.Log("playerHealth");
        if (playerHealth != null)
        {
            if (player != null)
            {
                var attack = player.GetComponent<Attack>();
                attack.AttackPlayer(other.gameObject);
                Debug.Log("attack");
            }
        }
    }

}
