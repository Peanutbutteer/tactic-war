using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;

public class DirectionSkillController : MonoBehaviour
{
    
    public string skillName;
	public GameObject player;
    public GameObject skill;
	public GameObject skillSpwanPosition;
    public GameObject skillLine;
	public GameObject coolDownPrefab;

    private GameObject cooldownSkill;

    private float nextFire;
    new Rigidbody rigidbody;
    Animator anim;
    Vector3 movement;
    Text txtCooldown;

    void Start()
    {
		GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
		cooldownSkill = Instantiate (coolDownPrefab, canvas.transform, false);
        rigidbody = GetComponent<Rigidbody>();
        anim = player.GetComponent<Animator>();
    }

	void FixedUpdate () {
		float horizontal = CnInputManager.GetAxis(skillName + "Horizontal");
		float vertical = CnInputManager.GetAxis(skillName + "Vertical");
        //fireball direction selector
		if (horizontal != 0 || vertical != 0) {
			rigidbody.transform.rotation = Util.Turning (horizontal, vertical);
		}
			
	}

	void Update() {
		
        if (CnInputManager.GetButton(skillName + "Button"))
		{
            skillLine.SetActive(true);
            skillLine.transform.rotation = Util.TurningFix(CnInputManager.GetAxis(skillName + "Horizontal"), CnInputManager.GetAxis(skillName + "Vertical"));

        }

		if (CnInputManager.GetButtonUp(skillName + "Button") && !cooldownSkill.activeSelf)
		{
            skillLine.SetActive(false);
            anim.SetBool("Attack", true);
			player.transform.rotation = rigidbody.transform.rotation;
			cooldownSkill.SetActive (true);
			StartCoroutine(Attack());
		}
	}

	IEnumerator Attack()
	{
		yield return new WaitForSeconds(0.5f);
        anim.SetBool("Attack", false);
		Instantiate(skill, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
	}
}
