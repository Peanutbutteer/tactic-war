using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;
public class ButtonOneController : NetworkBehaviour
{
	public SkillBehavior[] skill;
    void Start () {
        skill = SkillsLibrary.s_Instance.getSkill();
        if(skill != null) {
            for (int index = 0; index < skill.Length; index++)
            {
                skill[index].Start();
                CmdSetAuthority(skill[index].GetNetworkIdentity());
            }
        }
	}

	[Command]
	void CmdSetAuthority(NetworkIdentity grabID)
	{
		grabID.AssignClientAuthority(connectionToClient);
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        // Skill slot 0
        if (skill[0] != null)
        {
            if (CnInputManager.GetButtonUp("BlockButton"))
            {
                skill[0].ButtonUp();
            }
        }
        // Skill slot 1
        if (skill[1] != null)
        {
            if (CnInputManager.GetButton("FireballButton"))
            {
                skill[1].ButtonDirection(CnInputManager.GetAxis("FireballVertical"), CnInputManager.GetAxis("FireballHorizontal"));
            }
            if (CnInputManager.GetButtonUp("FireballButton"))
            {
                skill[1].ButtonUp();
            }
        }
        // Skill slot 2
        if (skill[2] != null)
        {
            if (CnInputManager.GetButton("BlinkButton"))
            {
                skill[2].ButtonDirection(CnInputManager.GetAxis("BlinkVertical"), CnInputManager.GetAxis("BlinkHorizontal"));
            }
            if (CnInputManager.GetButtonUp("BlinkButton"))
            {
                skill[2].ButtonUp();

            }
        }
        // Skill slot 3
        if (skill[3] != null)
        {
            if (CnInputManager.GetButton("HomingButton"))
            {
                skill[3].ButtonDirection(CnInputManager.GetAxis("HomingVertical"), CnInputManager.GetAxis("HomingHorizontal"));
            }
            if (CnInputManager.GetButtonUp("HomingButton"))
            {
                skill[3].ButtonUp();

            }
        }
    }
}
