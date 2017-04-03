using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FreezeController : Skill
{
    public GameObject freezePrefab;

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
        CmdSpawnFreezeSkill(player);
    }
    [Command]
    void CmdSpawnFreezeSkill(GameObject player)
    {
        GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        GameObject freeze = (GameObject)Instantiate(freezePrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        NetworkServer.Spawn(freeze);
        Destroy(freeze, 10f);
    }
}
