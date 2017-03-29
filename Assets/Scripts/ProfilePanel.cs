using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour
{
	const int SKILL_CATAGORY_ONE = 0;
	const int SKILL_CATAGORY_TWO = 1;
	const int SKILL_CATAGORY_THREE = 2;
	const int SKILL_CATAGORY_FOUR = 3;
	public Text playerName;
	public RectTransform rowOne;
	public RectTransform rowTwo;
	public Text skillName;
	public Text skillDescription;
	public GameObject skillSlotPrefab;
	public int catSelectedIndex = SKILL_CATAGORY_ONE;
	public SkillSlot[] slots;
	public int idSelected = 0;
	void Start()
	{
		Load();
		SlotClick(0);
		idSelected = DataManager.s_Singleton.slots[0];
	}

	public void Load()
	{
		if (DataManager.s_Singleton != null)
		{
			playerName.text = DataManager.s_Singleton.playerName;
			int index = 0;
			foreach (SkillSlot slot in slots)
			{
				slot.skillImage = SkillsInfoLibrary.s_Instance.getSkill(DataManager.s_Singleton.slots[index]).buttonSourceImage;
				slot.skillId = index;
				slot.OnSlotClick = SlotClick;
				index++;
			}
			refreshCatagorySelect();
		}
	}

	private void refreshCatagorySelect()
	{
		//Catagory
		int index = 0;
		foreach (SkillSlot slot in slots)
		{
			slot.isSelected = false;
			if (index == catSelectedIndex)
			{
				slot.isSelected = true;
				idSelected = DataManager.s_Singleton.slots[index];
				UpdateDescription(idSelected);
				RefreshSkillPanel(SkillsInfoLibrary.s_Instance.getSkillsByCatagory(index));
			}
			index++;
		}
	}

	public void SlotClick(int id)
	{
		int catagoryId = SkillsInfoLibrary.s_Instance.getSkill(id).catagory;
		catSelectedIndex = catagoryId;
		refreshCatagorySelect();
	}

	public void SkillClick(int id)
	{
		SkillsInfoLibrary.SkillInfo skill = SkillsInfoLibrary.s_Instance.getSkill(id);
		catSelectedIndex = skill.catagory;
		if (catSelectedIndex < 4)
		{
			DataManager.s_Singleton.slots[skill.catagory] = id;
			DataManager.s_Singleton.Save();
		}
		Load();
	}

	private void UpdateDescription(int id)
	{
		SkillsInfoLibrary.SkillInfo skill = SkillsInfoLibrary.s_Instance.getSkill(id);
		skillName.text = skill.skillName;
		skillDescription.text = skill.description;
	}

	public void RefreshSkillPanel(List<SkillsInfoLibrary.SkillInfo> skills)
	{
		foreach (Transform child in rowOne.transform)
		{
			Destroy(child.gameObject);
		}
		foreach (Transform child in rowTwo.transform)
		{
			Destroy(child.gameObject);
		}
		int index = 0;
		foreach (SkillsInfoLibrary.SkillInfo skill in skills)
		{
			Transform rowTransform;
			if (index < 4)
			{
				rowTransform = rowOne.transform;
			}
			else
			{
				rowTransform = rowTwo.transform;
			}
			GameObject skillSlot = Instantiate(skillSlotPrefab, rowTransform, false);
			SkillBadge slot = skillSlot.GetComponent<SkillBadge>();
			if (slot != null)
			{
				slot.skillId = skill.id;
				slot.OnSlotClick = SkillClick;
				slot.SkillName = skill.skillName;
				slot.skillImage = skill.buttonSourceImage;
				slot.isSelected = idSelected == skill.id;
				
			}
			index++;
		}
	}

}
