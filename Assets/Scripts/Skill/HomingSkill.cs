using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class HomingSkill : MonoBehaviour
{
    public float missileVelocity = 0.1f;
    public GameObject target;
    private Rigidbody homingMissile;
    // Use this for initialization
    void Start()
    {
        homingMissile = GetComponent<Rigidbody>();
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
            if (!item.GetComponent<NetworkIdentity>().hasAuthority)
            {
                if (Vector3.Distance(transform.position, item.transform.position) < 20)
                {
                    target = item;
                    var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

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
