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

    private Vector3 chargeSpawnPosition;
    private Quaternion chargeSpawnRotation;

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
        CmdSpawnLaserChargeSkill(player);
        yield return new WaitForSeconds(1f);
        CmdSpawnLaserSkill(player);

    }
    [Command]
    void CmdSpawnLaserChargeSkill(GameObject player)
    {
        GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        GameObject laserCharge = (GameObject)Instantiate(laserChargePrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        chargeSpawnPosition = new Vector3(skillSpwanPosition.transform.position.x, skillSpwanPosition.transform.position.y, skillSpwanPosition.transform.position.z);
        chargeSpawnRotation = new Quaternion(skillSpwanPosition.transform.rotation.x, skillSpwanPosition.transform.rotation.y, skillSpwanPosition.transform.rotation.z, skillSpwanPosition.transform.rotation.w);
        NetworkServer.Spawn(laserCharge);
        Destroy(laserCharge, 1f);
    }
    [Command]
    void CmdSpawnLaserSkill(GameObject player)
    {
        GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        chargeSpawnRotation *= Quaternion.Euler(90, 0, 0);
        GameObject laser = (GameObject)Instantiate(laserPrefab, chargeSpawnPosition, chargeSpawnRotation);
        NetworkServer.Spawn(laser);
    }
}
