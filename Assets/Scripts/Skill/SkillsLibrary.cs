using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillsLibrary : MonoBehaviour {
	[SerializeField]
	private Skill[] skills;
	// Use this for initialization
	public static SkillsLibrary s_Instance
	{
		get;
		private set;
	}

	public SkillBehavior[] getSkill(){
		return skills;
	}

	protected virtual void Awake()
	{
		if (s_Instance == null)
		{
			s_Instance = this;
			DontDestroyOnLoad(this);
		}
	}

}
