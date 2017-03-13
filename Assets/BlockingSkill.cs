using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingSkill : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.gameObject.tag);    
        if (other.gameObject.tag == "Attack")
        {
            Destroy(other);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag+ "1+");
    }

}
