using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MoveSpeedController : Skill
{
    public GameObject speedUpPrefab;
    
    private PlayerMageController playerMage;
    // Use this for initialization
    public override void OnStartPlayer()
    {
		base.OnStartPlayer();
        playerMage = player.GetComponent<PlayerMageController>();
    }

    public override void ButtonDown()
    {
        base.ButtonDown();
        StartCoroutine(MoveSpeed());        
    }
    IEnumerator MoveSpeed()
    {
        CmdSpawnSpeedUp(player);
        playerMage.speed = 25;
        yield return new WaitForSeconds(1.5f);
        playerMage.speed = 15;
    }

    [Command]
    public void CmdSpawnSpeedUp(GameObject player)
    {
        GameObject speedUpSkill = Instantiate(speedUpPrefab, speedUpPrefab.transform.position, speedUpPrefab.transform.rotation);
        speedUpSkill.transform.parent = player.transform;
        NetworkServer.Spawn(speedUpSkill);
        Destroy(speedUpSkill, 1.5f);
        RpcSyncSpeed(speedUpSkill.transform.position, speedUpSkill.transform.rotation, speedUpSkill, player);
    }
    [ClientRpc]
    public void RpcSyncSpeed(Vector3 localPos, Quaternion localRot, GameObject speedUpSkill, GameObject parent)
    {
        speedUpSkill.transform.parent = parent.transform;
        speedUpSkill.transform.localPosition = localPos;
        speedUpSkill.transform.localRotation = localRot;
    }
}
