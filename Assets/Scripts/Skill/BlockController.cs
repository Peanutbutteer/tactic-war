using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
public class BlockController : MonoBehaviour {

    public GameObject block;
    public GameObject player;
	public GameObject cooldownSkill;
	public GameObject cooldownSkillText;
	public int cooldown = 4;
	private float cooldownTimer = 0;
	private Text txtCooldown;


    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = player.GetComponent<Animator>();
		txtCooldown = cooldownSkillText.GetComponent<Text> ();
	}

	void FixedUpdate() {
		if (cooldownTimer > 1) {
			txtCooldown.text = "" + (int) cooldownTimer;
			cooldownTimer -= Time.deltaTime;
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (CnInputManager.GetButtonUp("BlockButton"))
        {
            block.SetActive(true);
            anim.SetBool("Block", true);
			cooldownTimer = cooldown;
			cooldownSkill.SetActive (true);
            StartCoroutine(Block());
        }

		if (cooldownTimer < 1) {
			cooldownTimer = 1;
			cooldownSkill.SetActive (false);
		}
    }

    IEnumerator Block()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Block", false);
        yield return new WaitForSeconds(0.5f);
        block.SetActive(false);
    }
}
