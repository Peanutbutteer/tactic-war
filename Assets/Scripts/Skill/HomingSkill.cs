using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        homingMissile.velocity = transform.forward * missileVelocity;

        if (Vector3.Distance(transform.position, target.transform.position) < 20)
        {
            var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

            homingMissile.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 2.5f));
        }
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
