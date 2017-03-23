using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BlinkControllerT : Skill
{
    
    public GameObject effectBlink;
    public GameObject cooldownPrefab;
    public int skillRadius = 15;
    
    private GameObject BlinkPoint;
    private new Projector renderer;
    private GameObject blinkArea;
    private Vector3 positionBlink;

    public override void Start()
    {
        base.Start();
        InstantiateCooldownSkill(cooldownPrefab);
        BlinkPoint = FindObjectInPlayer("BlinkPoint");
        blinkArea = FindObjectInPlayer("BlinkArea");
        renderer = BlinkPoint.GetComponent<Projector>();
        renderer.enabled = false;
    }

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        blinkArea.SetActive(true);
        renderer.enabled = true;
        positionBlink = new Vector3(horizontal * skillRadius, 1000f * 0.03f, vertical * skillRadius);
        BlinkPoint.transform.position = player.transform.position + positionBlink;
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        cooldownSkill.SetActive(true);
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        renderer.enabled = false;
        anim.SetBool("Blink", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Blink", false);
        CmdSpawnEffectBlink(player);
        blinkArea.SetActive(false);
        Vector3 position = BlinkPoint.transform.position;
        position.y = 0;
        player.transform.position = position;
        positionBlink = new Vector3(0, 0, 0);

    }
    [Command]
    void CmdSpawnEffectBlink(GameObject player)
    {
        Vector3 positionEffectBlink = new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z);
        var Blink = (GameObject)Instantiate(effectBlink, positionEffectBlink, player.transform.rotation);
        NetworkServer.Spawn(Blink);

    }
}
