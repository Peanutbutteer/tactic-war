﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class HomingSkill : NetworkBehaviour
{
    public float missileVelocity = 0.1f;
    private Rigidbody homingMissile;
	[SyncVar]
	public int idOwner;
    // Use this for initialization
    void Start()
    {
		homingMissile = GetComponent<Rigidbody>();
		Debug.Log("ID Owner:" + idOwner);
    }

    // Update is called once per frame
    void FixedUpdate()
    {


    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in players)
        {
			if (!(item.GetComponent<PlayerMageController>().playerId == idOwner) && Vector3.Distance(transform.position, item.transform.position) < 20)
            {
                {
                    var targetRotation = Quaternion.LookRotation(item.transform.position - transform.position);

                    homingMissile.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 2.5f));
                }
            }
        }

        homingMissile.velocity = transform.forward * missileVelocity;

    }
    void OnParticleCollision(GameObject other)
    {
        Destroy(this.gameObject);
    }
    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
