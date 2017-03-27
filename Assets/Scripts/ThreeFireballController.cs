using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ThreeFireballController : Skill
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
        yield return new WaitForSeconds(0.3f);
        CmdSpawnFireballSkill(player);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Attack", false);
        //CmdSpawnSkill("SkillSpawn", fireballPrefab, player);
    }
    [Command]
    void CmdSpawnFireballSkill(GameObject player)
    {
        StartCoroutine(spawnFireball());
        //GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        //GameObject fireball = (GameObject)Instantiate(fireballPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        //NetworkServer.Spawn(fireball);
    }
    IEnumerator spawnFireball()
    {
        GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        GameObject fireball = (GameObject)Instantiate(fireballPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        float offset;
        //for (float round = -1f; round < 0; round++)
        //{
            NetworkServer.Spawn(fireball);
            yield return new WaitForSeconds(0.4f);
            offset = Random.Range(-10f, -5f);
            skillSpwanPosition.transform.Rotate(0, offset, 0);
            fireball = (GameObject)Instantiate(fireballPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        NetworkServer.Spawn(fireball);
        skillSpwanPosition.transform.rotation = Quaternion.Euler(0, 30, 0);
        yield return new WaitForSeconds(0.2f);
        //}
        offset = Random.Range(5f , 10f);
        skillSpwanPosition.transform.Rotate(0, offset, 0);
        fireball = (GameObject)Instantiate(fireballPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        NetworkServer.Spawn(fireball);
        skillSpwanPosition.transform.rotation = Quaternion.Euler(0, 30, 0);
    }
}
