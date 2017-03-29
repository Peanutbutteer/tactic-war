using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollider : MonoBehaviour {


    public float FireballVelocity = 0.1f;
    Rigidbody FireballRigid;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        FireballRigid.velocity = transform.forward * FireballVelocity;
    }
}
