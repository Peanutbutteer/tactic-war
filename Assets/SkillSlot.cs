using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
[Serializable]
public class SkillSlot : MonoBehaviour
{
	public Image image;
	public Button button;
	public Action<int> OnSlotClick;
	public int skillId;
	public bool isSelected = false;
	private Sprite normalState;
	private Sprite selectedState;

	protected void Start()
	{
		normalState = Resources.Load<Sprite>("normal");
		selectedState = Resources.Load<Sprite>("selected");
		button.onClick.AddListener(() =>
		{
			if (OnSlotClick != null)
			{
				OnSlotClick(skillId);
			}
		});
	}

	void Update()
	{
		if (isSelected)
		{
			button.image.sprite = selectedState;
		}
		else
		{
			button.image.sprite = normalState;
		}
	}

	public Sprite skillImage
	{
		set
		{
			image.sprite = value;
		}
	}

}
