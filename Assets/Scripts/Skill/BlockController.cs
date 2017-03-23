using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;
public class BlockController : Skill
{
    public GameObject blockPrefab;
	public GameObject cooldownPrefab;
    // Use this for initialization
    public override void Start () {
        base.Start();
        InstantiateCooldownSkill(cooldownPrefab);
    }

	[Command]
	public void CmdSpawnBlock(GameObject player)
	{
		GameObject block = Instantiate(blockPrefab, blockPrefab.transform.position, blockPrefab.transform.rotation);
		block.transform.parent = player.transform;
		NetworkServer.Spawn(block);
        Destroy(block, 1.0f);
        RpcSyncBlock(block.transform.position,block.transform.rotation,block,player);
	}
    [ClientRpc]
     public void RpcSyncBlock(Vector3 localPos, Quaternion localRot,GameObject block, GameObject parent)
     {
         block.transform.parent = parent.transform;
         block.transform.localPosition = localPos;
         block.transform.localRotation = localRot;
     }

    public override void ButtonHold()
    {
        base.ButtonHold();
    }
    public override void ButtonDown() {
    }

    public override void ButtonUp() {
        cooldownSkill.SetActive(true);
        CmdSpawnBlock(player);
    }

    public override void ButtonDirection(float vertical, float horizontal) {
    }
}
