using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SkillsInfoLibrary : MonoBehaviour
{
	[SerializeField]
	SkillInfo[] skillsInfo;
	// Use this for initialization
	void Start()
	{

	}

	public static SkillsInfoLibrary s_Instance
	{
		get;
		private set;
	}

	protected virtual void Awake()
	{
		if (s_Instance == null)
		{
			s_Instance = this;
			DontDestroyOnLoad(this);
			for (int index = 0; index < skillsInfo.Length; index++)
			{
				skillsInfo[index].id = index;
			}
		}
	}

	public Sprite GetImageSkill(int id)
	{
		return skillsInfo[id].buttonSourceImage;
	}

	public int GetCooldownSkill(int id)
	{
		return skillsInfo[id].cooldown;
	}

	public SkillInfo getSkill(int index)
	{
		return skillsInfo[index];
	}

	public List<SkillInfo> getSkillsByCatagory(int cat)
	{
		List<SkillInfo> skillTemp = new List<SkillInfo>();
		foreach (SkillInfo skill in skillsInfo)
		{
			if (skill.catagory == cat)
			{
				skillTemp.Add(skill);
			}
		}
		return skillTemp;
	}

	[Serializable]
	public class SkillInfo
	{
		public int catagory;
		public int cooldown = 3;
		public string skillName;
		public Sprite buttonSourceImage;
		public string description;
		public int id
		{
			set { mId = value; }
			get { return mId; }
		}
		protected int mId;
	}
}
