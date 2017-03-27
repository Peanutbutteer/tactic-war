using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillsLibrary : MonoBehaviour
{
	[SerializeField]
	private Skill[] skills;
	// Use this for initialization
	public static SkillsLibrary s_Instance
	{
		get;
		private set;
	}

	public SkillBehavior[] getSkills()
	{
		return skills;
	}


	public SkillBehavior getSkill(int index)
	{
		return skills[index];
	}

	public List<Skill> getSkillsByCatagory(int cat)
	{
		List<Skill> skillTemp = new List<Skill>();
		foreach (Skill skill in skills)
		{
			if (skill.catagory == cat)
			{
				skillTemp.Add(skill);
			}
		}
		return skillTemp;
	}

	protected virtual void Awake()
	{
		if (s_Instance == null)
		{
			s_Instance = this;
			DontDestroyOnLoad(this);
			for (int index = 0; index < skills.Length; index++)
			{
				skills[index].id = index;
			}
		}
	}

}
