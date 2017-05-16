﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SetAuthorityToSkill : NetworkBehaviour
{
	public SkillBehavior[] skill;
	// Use this for initialization
	void Start()
	{
        //if (!isLocalPlayer) return;
        skill = SkillsLibrary.s_Instance.getSkills();
        if (skill != null)
        {
            for (int index = 0; index < skill.Length; index++)
            {
                skill[index].OnStartPlayer();
                CmdSetAuthority(skill[index].GetNetworkIdentity());
            }
        }
        CmdSetAuthority(GameManager.s_Singleton.GetComponent<NetworkIdentity>());
    }

    [Command]
	void CmdSetAuthority(NetworkIdentity grabID)
	{
		grabID.AssignClientAuthority(connectionToClient);
	}

}
