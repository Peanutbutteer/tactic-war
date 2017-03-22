using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FireballController : NetworkBehaviour
{
    public GameObject skill;
	public GameObject skillSpwanPosition;
    public GameObject skillLine;
	public GameObject coolDownPrefab;
    public GameObject controller;
    private GameObject cooldownSkill;

    private float nextFire;
    private float playerHorizontal;
    private float playerVertical;
    Animator anim;
    Vector3 movement;
    Text txtCooldown;

    void Start()
    {
		GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
		cooldownSkill = Instantiate (coolDownPrefab, canvas.transform, false);
        anim = GetComponent<Animator>();
    }

	void FixedUpdate () {
        float horizontal = CnInputManager.GetAxis("FireballHorizontal");
        float vertical = CnInputManager.GetAxis("FireballVertical");
        //horizontal = CnInputManager.GetAxis("FireballHorizontal");
        //vertical = CnInputManager.GetAxis("FireballVertical");
        //fireball direction selector
        if (horizontal != 0 || vertical != 0) {
            playerHorizontal = horizontal;
            playerVertical = vertical;
            //rigidbody.transform.rotation = Util.Turning(horizontal, vertical);
        }
			
	}

	void Update() {

        if (!isLocalPlayer)
        {
            return;
        }

        if (CnInputManager.GetButton("FireballButton"))
		{
            skillLine.SetActive(true);
            skillLine.transform.rotation = Util.TurningFix(CnInputManager.GetAxis("FireballHorizontal"), CnInputManager.GetAxis("FireballVertical"));

        }

		if (CnInputManager.GetButtonUp("FireballButton") && !cooldownSkill.activeSelf)
		{
            skillLine.SetActive(false);
            anim.SetBool("Attack", true);
            //transform.rotation = rigidbody.transform.rotation;
            transform.rotation = Util.Turning(playerHorizontal, playerVertical);
            cooldownSkill.SetActive (true);
            StartCoroutine(Attack());
		}
	}

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Attack", false);
        CmdSpawnFireballSkill();
    }
    [Command]
    void CmdSpawnFireballSkill()
    {
        var attackSkill = (GameObject)Instantiate(skill, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        NetworkServer.Spawn(attackSkill);
    }


    //void OnParticleCollision(GameObject other)
    //{
    //    var playerHealth = GetComponent<PlayerHealth>();
    //    if (playerHealth != null)
    //    {
    //        playerHealth.TakeDamage(10);
    //    }

    //    Destroy(other);
    //}
}
