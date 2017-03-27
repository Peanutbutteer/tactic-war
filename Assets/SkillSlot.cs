using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SkillSlot : MonoBehaviour
{
	public Image image;
	public Button button;
	public Action<int> OnSlotClick;
	public int skillId;
	protected void Start()
	{
		button.onClick.AddListener(() =>
		{
			if (OnSlotClick != null)
			{
				OnSlotClick(skillId);
			}
		});
	}

	public Sprite skillImage
	{
		set
		{
			image.sprite = value;
		}
	}

}
