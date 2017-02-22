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
    public GameObject cooldownSkill;
    public GameObject cooldownSkillText;
    public float cooldownTimer = 2;

    private float nextFire;
    new Rigidbody rigidbody;
    Animator anim;
    Vector3 movement;
    Text txtCooldown;

    void Start()
    {
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

        if (cooldownTimer > 1)
        {
            txtCooldown = cooldownSkillText.GetComponent<Text>();
            txtCooldown.text = "" + (int)cooldownTimer;
            cooldownTimer -= Time.deltaTime;
        }
        if (cooldownTimer < 1)
        {
            cooldownSkill.SetActive(false);
            cooldownTimer = 1;
        }

        if (CnInputManager.GetButton(skillName + "Button"))
		{
            skillLine.SetActive(true);
            skillLine.transform.rotation = Util.TurningFix(CnInputManager.GetAxis(skillName + "Horizontal"), CnInputManager.GetAxis(skillName + "Vertical"));

        }

		if (CnInputManager.GetButtonUp(skillName + "Button") && cooldownTimer == 1)
		{
            cooldownSkill.SetActive(true);
            cooldownTimer = 4;
            skillLine.SetActive(false);
            anim.SetBool(skillName, true);
			player.transform.rotation = rigidbody.transform.rotation;
			StartCoroutine(Attack());
		}
	}

	IEnumerator Attack()
	{
		yield return new WaitForSeconds(0.5f);
        anim.SetBool(skillName, false);
		Instantiate(skill, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
	}
}
