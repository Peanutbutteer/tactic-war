﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HomingController : Skill
{
    public GameObject homingPrefab;

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        skillLine.SetActive(true);
        skillLine.transform.rotation = Util.TurningFix(horizontal, vertical);
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        anim.SetBool("Attack", true);
        player.transform.rotation = Util.Turning(lastHorizontal, lastVertical);
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Attack", false);
        CmdSpawnSkill(player);
    }

    [Command]
    void CmdSpawnSkill(GameObject player)
    {
        GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        GameObject Homing = (GameObject)Instantiate(homingPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        Homing.GetComponent<HomingSkill>().idOwner = player.GetComponent<PlayerMageController>().playerId;
        NetworkServer.SpawnWithClientAuthority(Homing, connectionToClient);
        Destroy(Homing, 10f);
    }
}
