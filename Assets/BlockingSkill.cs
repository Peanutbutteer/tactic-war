using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingSkill : MonoBehaviour
{
    public GameObject Environment;

    // Use this for initialization
    void Start()
    {
        //Physics.IgnoreCollision(transform.parent.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Attack")
        {
            Destroy(other);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("1");
        if(collision.gameObject.tag == "Attack")
        {
            Debug.Log("2");
        }
    }
}
