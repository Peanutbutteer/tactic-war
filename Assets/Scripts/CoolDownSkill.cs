using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoolDownSkill : MonoBehaviour {
	public int coolDownTime = 3;
	private float cooldownTimer = 1;
	Text txtCooldown;
	// Use this for initialization
	void Start () {
		txtCooldown = this.GetComponentInChildren<Text> ();
	}

	
	// Update is called once per frame
	void Update () {
		if (cooldownTimer == 1 && gameObject.activeSelf) {
			cooldownTimer = coolDownTime+1;
		}

		if (cooldownTimer > 0) {
			txtCooldown.text = "" + (int)cooldownTimer;
			cooldownTimer -= Time.deltaTime;
		}

		if (cooldownTimer < 1)
		{
			gameObject.SetActive(false);
			cooldownTimer = 1;
		}
	}




}
