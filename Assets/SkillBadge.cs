using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillBadge : SkillSlot
{
	public Text skillName;
	public void Start()
	{
		base.Start();
	}

	public string SkillName
	{
		set { skillName.text = value; }
	}
}
