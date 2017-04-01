using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ThreeFireballController : Skill
{
    public GameObject fireballPrefab;

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
        StartCoroutine(spawnFireball(player));
        //GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        //GameObject fireball = (GameObject)Instantiate(fireballPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        //NetworkServer.Spawn(fireball);
    }
    IEnumerator spawnFireball(GameObject player)
    {
        GameObject skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        Vector3 fireballSpawnPosition = new Vector3(skillSpwanPosition.transform.position.x, skillSpwanPosition.transform.position.y, skillSpwanPosition.transform.position.z);
        Quaternion fireballSpawnRotation = new Quaternion(skillSpwanPosition.transform.rotation.x, skillSpwanPosition.transform.rotation.y, skillSpwanPosition.transform.rotation.z, skillSpwanPosition.transform.rotation.w);
        GameObject fireball = (GameObject)Instantiate(fireballPrefab, fireballSpawnPosition, skillSpwanPosition.transform.rotation);
        float offset;
        NetworkServer.Spawn(fireball);
        yield return new WaitForSeconds(0.4f);
        offset = Random.Range(-10f, -5f);
        fireballSpawnRotation *= Quaternion.Euler(0, offset, 0);
        fireball = (GameObject)Instantiate(fireballPrefab, fireballSpawnPosition, fireballSpawnRotation);
        NetworkServer.Spawn(fireball);
        fireballSpawnRotation = new Quaternion(skillSpwanPosition.transform.rotation.x, skillSpwanPosition.transform.rotation.y, skillSpwanPosition.transform.rotation.z, skillSpwanPosition.transform.rotation.w);
        yield return new WaitForSeconds(0.2f);
        offset = Random.Range(5f , 10f);
        fireballSpawnRotation *= Quaternion.Euler(0, offset, 0);
        fireball = (GameObject)Instantiate(fireballPrefab, fireballSpawnPosition, fireballSpawnRotation);
        NetworkServer.Spawn(fireball);
    }
}
