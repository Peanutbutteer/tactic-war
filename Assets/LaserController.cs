using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LaserController : Skill
{
    public GameObject laserPrefab;
    public GameObject laserChargePrefab;

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        skillLine.SetActive(true);
        skillLine.transform.rotation = Util.TurningFix(horizontal, vertical);
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        skillLine.SetActive(false);
        anim.SetBool("Attack", true);
        player.transform.rotation = Util.Turning(lastHorizontal, lastVertical);
        cooldownSkill.SetActive(true);
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Attack", false);
        CmdSpawnLaserChargeSkill(player);
        yield return new WaitForSeconds(1f);
        CmdSpawnLaserSkill(player);

    }
    [Command]
    void CmdSpawnLaserChargeSkill(GameObject player)
    {
        GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        GameObject laserCharge = (GameObject)Instantiate(laserPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        NetworkServer.Spawn(laserCharge);
    }
    [Command]
    void CmdSpawnLaserSkill(GameObject player)
    {
        GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        GameObject laser = (GameObject)Instantiate(laserPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        NetworkServer.Spawn(laser);
    }
}
