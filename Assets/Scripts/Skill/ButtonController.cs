﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;
public class ButtonController : NetworkBehaviour
{
	public SkillBehavior skill;
	public string buttonName;
	public int indexSelectedSkill;
	public bool isPressButton = false;
	public int xPosition, yPosition;
	public float scale = 1;
	public GameObject virtualJoyStickPrefab;

	private string simpleButtonName;
	private string verticalButtonName;
	private string horizontalButtonName;

	private GameObject cooldownPrefab;
	private GameObject canvas;
	private Sprite buttonImage;
	void Start()
	{
		canvas = GameObject.FindGameObjectWithTag("Canvas");
		skill = SkillsLibrary.s_Instance.getSkill(indexSelectedSkill);
		cooldownPrefab = (GameObject)Resources.Load("coolDown", typeof(GameObject));
		virtualJoyStickPrefab = (GameObject)Resources.Load("Joystick", typeof(GameObject));
		simpleButtonName = buttonName + "Button";
		verticalButtonName = buttonName + "Vertical";
		horizontalButtonName = buttonName + "Horizontal";
		buttonImage = skill.GetButtonImage();
		InstantiateButton();
	}

	// Update is called once per frame
	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		if (CnInputManager.GetButtonDown(simpleButtonName))
		{
			skill.ButtonDown();
		}
		if (CnInputManager.GetButtonUp(simpleButtonName))
		{
			skill.ButtonUp();
		}
		if (!isPressButton && CnInputManager.GetButton(simpleButtonName))
		{
			skill.ButtonDirection(CnInputManager.GetAxis(verticalButtonName), CnInputManager.GetAxis(horizontalButtonName));
		}
	}

	protected void InstantiateButton()
	{
		cooldownPrefab.transform.localPosition = new Vector3(xPosition, yPosition);
		cooldownPrefab.transform.localScale = new Vector3(scale, scale);
		cooldownPrefab.GetComponent<CoolDownSkill>().coolDownTime = skill.GetCoolDownTime();
		skill.SetCooldown(Instantiate(cooldownPrefab, canvas.transform, false));

		virtualJoyStickPrefab.transform.localPosition = new Vector3(xPosition, yPosition);
		virtualJoyStickPrefab.transform.localScale = new Vector3(scale, scale);
		SimpleJoystick joyStick = virtualJoyStickPrefab.GetComponent<SimpleJoystick>();
		joyStick.HorizontalAxisName = horizontalButtonName;
		joyStick.VerticalAxisName = verticalButtonName;
		SimpleButton button = virtualJoyStickPrefab.GetComponent<SimpleButton>();
		button.ButtonName = simpleButtonName;
		ButtonImage buttonImg = virtualJoyStickPrefab.GetComponentInChildren<ButtonImage>();
		buttonImg.changeImage(buttonImage);
		Instantiate(virtualJoyStickPrefab, canvas.transform, false);
	}
}
