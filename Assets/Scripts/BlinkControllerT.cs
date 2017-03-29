using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BlinkControllerT : Skill
{
    
    public GameObject effectBlink;
    public int skillRadius = 15;
    
    private GameObject BlinkPoint;
    private Projector rendBlinkPoint;
    private GameObject blinkArea;
    private Vector3 positionSkill;

	public override void OnStartPlayer()
    {
		base.OnStartPlayer();
        BlinkPoint = FindObjectInPlayer("SkillPoint");
        blinkArea = FindObjectInPlayer("SkillArea");

        rendBlinkPoint = BlinkPoint.GetComponent<Projector>();
        rendBlinkPoint.orthographicSize = 2;
        rendBlinkPoint.enabled = false;
    }

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        blinkArea.SetActive(true);
        rendBlinkPoint.enabled = true;
        positionSkill = new Vector3(horizontal * skillRadius, 1000f * 0.03f, vertical * skillRadius);
        BlinkPoint.transform.position = player.transform.position + positionSkill;
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        cooldownSkill.SetActive(true);
        StartCoroutine(CastBlink());
    }

    IEnumerator CastBlink()
    {
        rendBlinkPoint.enabled = false;
        blinkArea.SetActive(false);
        anim.SetBool("Blink", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Blink", false);

        CmdSpawnEffectBlink(player);
        
        Vector3 position = BlinkPoint.transform.position;
        position.y = 0;
        player.transform.position = position;
        positionSkill = new Vector3(0, 0, 0);

    }
    [Command]
    void CmdSpawnEffectBlink(GameObject player)
    {
        Vector3 positionEffectBlink = new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z);
        var Blink = (GameObject)Instantiate(effectBlink, positionEffectBlink, player.transform.rotation);
        NetworkServer.Spawn(Blink);

    }
}
