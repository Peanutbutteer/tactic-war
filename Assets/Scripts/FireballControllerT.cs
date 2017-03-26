using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FireballControllerT : Skill
{
    public GameObject fireballPrefab;

    public override void Start()
    {
        base.Start();
    }

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        skillLine.SetActive(true);
        skillLine.transform.rotation = Util.TurningFix(horizontal , vertical);
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
        //CmdSpawnSkill("SkillSpawn", fireballPrefab, player);
        CmdSpawnFireballSkill(player);
    }
    [Command]
    void CmdSpawnFireballSkill(GameObject player)
    {
        GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        GameObject fireball = (GameObject)Instantiate(fireballPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        NetworkServer.Spawn(fireball);
    }
    //[Command]
    //protected override void SpawnSkill(string childName, GameObject skillPrefab, GameObject player)
    //{
    //    base.CmdSpawnSkill(childName, skillPrefab , player);
    //}
}
