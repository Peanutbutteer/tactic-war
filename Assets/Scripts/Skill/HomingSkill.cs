using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class HomingSkill : NetworkBehaviour
{
    public float missileVelocity = 0.1f;
    private Rigidbody homingMissile;
	[SyncVar]
	public int idOwner;
    public AudioClip soundHoming;
    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource source;
    // Use this for initialization
    void Start()
    {
        homingMissile = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        source.loop = true;
        source.volume = volume;
        source.clip = soundHoming;
        source.Play();
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

    void OnCollisionEnter(Collision collision)
    {

        Destroy(this.gameObject);
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
