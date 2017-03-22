using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FireballControllerT : Skill
{
    public GameObject fireballPrefab;
    public GameObject cooldownPrefab;
    private GameObject cooldownSkill;
    private GameObject skillSpwanPosition;
    private GameObject skillLine;

    private float playerHorizontal;
    private float playerVertical;
    Animator anim;

    public override void Start()
    {
        base.Start();
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        skillLine = player.transform.FindChild("SkillLine").gameObject;
        cooldownSkill = Instantiate(cooldownPrefab, canvas.transform, false);
        anim = player.GetComponent<Animator>();
        
    }

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        if (horizontal != 0 || vertical != 0)
        {
            playerHorizontal = horizontal;
            playerVertical = vertical;
        }
        skillLine.SetActive(true);
        skillLine.transform.rotation = Util.TurningFix(horizontal , vertical);
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        skillLine.SetActive(false);
        anim.SetBool("Attack", true);
        player.transform.rotation = Util.Turning(playerHorizontal, playerVertical);
        cooldownSkill.SetActive(true);
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Attack", false);
        CmdSpawnFireballSkill(player);
    }
    [Command]
    void CmdSpawnFireballSkill(GameObject player)
    {
        skillSpwanPosition = player.transform.FindChild("SkillSpawn").gameObject;
        var fireball = (GameObject)Instantiate(fireballPrefab, skillSpwanPosition.transform.position, skillSpwanPosition.transform.rotation);
        NetworkServer.Spawn(fireball);
    }
}
