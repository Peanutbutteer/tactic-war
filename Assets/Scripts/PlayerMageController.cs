using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.Networking;

public class PlayerMageController : NetworkBehaviour
{

	public float speed = 15f;
    
	new Rigidbody rigidbody;
	Vector3 movement;
    Animator anim;

    void Start () {
		rigidbody = GetComponent<Rigidbody> ();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (!isLocalPlayer)
        {
            return;
        }

        float horizontal = CnInputManager.GetAxis ("Horizontal");
		float vertical = CnInputManager.GetAxis ("Vertical");

        if (anim.GetBool("Attack") == false && anim.GetBool("Blink") == false)
        {
            if (CnInputManager.GetButton("WalkButton"))
            {
                rigidbody.rotation = Util.Turning(horizontal, vertical);
            }
            Move(horizontal, vertical);
            Animating(horizontal, vertical);
        }


    }

    void Move(float x,float y) {
		movement.Set (x, 0, y);

		movement = movement.normalized * speed * Time.deltaTime;
		rigidbody.MovePosition (transform.position + movement);

	}


    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f && anim.GetBool("Attack") == false;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("Walk Forward", walking);
    }
   


}
