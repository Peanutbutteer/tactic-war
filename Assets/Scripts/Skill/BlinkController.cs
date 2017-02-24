using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;

public class BlinkController : MonoBehaviour
{
    public GameObject player;
    public GameObject blinkArea;
    public GameObject effectBlink;
	public int skillRadius = 15;
	public GameObject cooldownSkill;
	public GameObject cooldownSkillText;
	public int cooldown = 4;
	private float cooldownTimer = 0;
    new Projector renderer;
    Vector3 positionBlink = new Vector3(0, 0, 0);
    Animator anim;
	Text txtCooldown;

    void Start()
    {
        renderer = GetComponent<Projector>();
        renderer.enabled = false;
        anim = player.GetComponent<Animator>();
		txtCooldown = cooldownSkillText.GetComponent<Text> ();
    }

	void FixedUpdate() {
		if (cooldownTimer > 0) {
			txtCooldown.text = "" + (int)cooldownTimer;
			cooldownTimer -= Time.deltaTime;
		}
	}

    void Update()
    {
		float horizontal = CnInputManager.GetAxis("BlinkHorizontal");
        float vertical = CnInputManager.GetAxis("BlinkVertical");


		if (cooldownTimer < 1)
		{
			cooldownSkill.SetActive(false);
			cooldownTimer = 1;
		}

        Move(horizontal, vertical);

    }



    void Move(float x, float y)
    {
		if(CnInputManager.GetButton("BlinkButton"))
        {
            blinkArea.SetActive(true);
            renderer.enabled = true;
			positionBlink = new Vector3(x * skillRadius, 1000f*0.03f, y * skillRadius);
            transform.position = player.transform.position + positionBlink;
        }
		if (CnInputManager.GetButtonUp("BlinkButton"))
		{
			cooldownSkill.SetActive(true);
			cooldownTimer = cooldown;
            StartCoroutine(Blink());
		}
    }

    IEnumerator Blink()
    {
        renderer.enabled = false;
        anim.SetBool("Blink",true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Blink", false);
        Vector3 positionEffectBlink = new Vector3(player.transform.position.x, player.transform.position.y+3, player.transform.position.z);
        Instantiate(effectBlink, positionEffectBlink, player.transform.rotation);
        blinkArea.SetActive(false);
        Vector3 position = transform.position;
        position.y = 0;
        player.transform.position = position;
        positionBlink = new Vector3(0, 0, 0);
    }
}
