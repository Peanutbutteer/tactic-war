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
        if (CnInputManager.GetButtonUp("BlockButton"))
        {
            if(skill!=null) {
            	skill[0].ButtonUp();
            }
        }

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
    }
}
