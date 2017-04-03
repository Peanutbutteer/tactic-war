using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class BlinkControllerT : Skill
{
    
    public GameObject effectBlink;
    public AudioClip audioBlink;
    [Range(10f, 50f)]
    public int skillRadius = 20;
    [Range(10f, 50f)]
    public int skillArea = 20;
    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource audioSource;
    private GameObject blinkPoint;
    private GameObject blinkArea;
    private Projector rendBlinkPoint;
    private Projector rendBlinkArea;
    private Vector3 positionSkill;

	public override void OnStartPlayer()
    {
		base.OnStartPlayer();
        blinkPoint = FindObjectInPlayer("SkillPoint");
        blinkArea = FindObjectInPlayer("SkillArea");

        audioSource = player.GetComponent<AudioSource>();

        rendBlinkPoint = blinkPoint.GetComponent<Projector>();
        rendBlinkArea = blinkArea.GetComponent<Projector>();
        
    }

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        rendBlinkPoint.orthographicSize = 2;
        rendBlinkArea.orthographicSize = skillArea;
        blinkArea.SetActive(true);
        blinkPoint.SetActive(true);
        positionSkill = new Vector3(horizontal * skillRadius, 1000f * 0.03f, vertical * skillRadius);
        blinkPoint.transform.position = player.transform.position + positionSkill;
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        cooldownSkill.SetActive(true);
        StartCoroutine(CastBlink());
    }

    IEnumerator CastBlink()
    {
        audioSource.PlayOneShot(audioBlink, volume);
        
        blinkArea.SetActive(false);
        blinkPoint.SetActive(false);
        anim.SetBool("Blink", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Blink", false);

        CmdSpawnEffectBlink(player);
        
        Vector3 position = blinkPoint.transform.position;
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
