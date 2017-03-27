using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour
{
	public Text playerName;
	public SkillSlot slotOne;
	public SkillSlot slotTwo;
	public SkillSlot slotThree;
	public SkillSlot slotFour;
	public RectTransform rowOne;
	public RectTransform rowTwo;
	public Text skillName;
	public Text skillDescription;
	public GameObject skillSlotPrefab;
	void Start()
	{
		Load();
	}

	public void Load()
	{
		if (DataManager.s_Singleton != null)
		{
			playerName.text = DataManager.s_Singleton.playerName;
			slotOne.skillImage = SkillsLibrary.s_Instance.getSkill(DataManager.s_Singleton.slotOne).GetButtonImage();
			slotTwo.skillImage = SkillsLibrary.s_Instance.getSkill(DataManager.s_Singleton.slotTwo).GetButtonImage();
			slotThree.skillImage = SkillsLibrary.s_Instance.getSkill(DataManager.s_Singleton.slotThree).GetButtonImage();
			slotFour.skillImage = SkillsLibrary.s_Instance.getSkill(DataManager.s_Singleton.slotFour).GetButtonImage();
			slotOne.OnSlotClick = SlotClick;
			slotTwo.OnSlotClick = SlotClick;
			slotThree.OnSlotClick = SlotClick;
			slotFour.OnSlotClick = SlotClick;
			slotFour.skillId = 0;
			slotTwo.skillId = 1;
			slotThree.skillId = 2;
			slotFour.skillId = 3;
		}
	}

	public void SlotClick(int id)
	{
		int catagoryId = SkillsLibrary.s_Instance.getSkill(id).GetCatagory();
		RefreshSkillPanel(SkillsLibrary.s_Instance.getSkillsByCatagory(catagoryId));
	}

	public void SkillClick(int id)
	{
		Debug.Log(id);
		SkillBehavior skill = SkillsLibrary.s_Instance.getSkill(id);
		skillName.text = skill.GetName();
		skillDescription.text = skill.GetDescription();
	}

	public void RefreshSkillPanel(List<Skill> skills)
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
		foreach (Skill skill in skills)
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
				slot.skillImage = skill.GetButtonImage();
			}
			index++;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

}
