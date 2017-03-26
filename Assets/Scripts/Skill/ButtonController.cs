using System.Collections;
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
	void Start()
	{
		skill = SkillsLibrary.s_Instance.getSkill(indexSelectedSkill);
	}

	// Update is called once per frame
	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		if (CnInputManager.GetButtonDown(buttonName + "Button"))
		{
			skill.ButtonDown();
		}
		if (CnInputManager.GetButtonUp(buttonName + "Button"))
		{
			skill.ButtonUp();
		}
		if (!isPressButton && CnInputManager.GetButton(buttonName + "Button"))
		{
			skill.ButtonDirection(CnInputManager.GetAxis(buttonName + "Vertical"), CnInputManager.GetAxis(buttonName + "Horizontal"));
		}
	}
}
