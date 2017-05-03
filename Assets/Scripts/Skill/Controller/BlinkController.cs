using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class BlinkController : Skill
{
    
    public GameObject effectBlink;
    public AudioClip audioBlink;
    [Range(10f, 50f)]
    public int skillRadius = 20;
    [Range(10f, 50f)]
    public int skillAreaSize = 20;
    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource audioSource;
    private Vector3 positionSkill;

	public override void OnStartPlayer()
    {
		base.OnStartPlayer();
        audioSource = player.GetComponent<AudioSource>();
    }

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        skillPoint.GetComponent<Projector>().orthographicSize = 2;
        skillArea.GetComponent<Projector>().orthographicSize = skillAreaSize;
        skillArea.SetActive(true);
        skillPoint.SetActive(true);
        positionSkill = new Vector3(horizontal * skillRadius, 1000f * 0.03f, vertical * skillRadius);
        skillPoint.transform.position = player.transform.position + positionSkill;
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        StartCoroutine(CastBlink());
    }

    IEnumerator CastBlink()
    {
        audioSource.PlayOneShot(audioBlink, volume);
        
        anim.SetBool("Blink", true);
        if (skillPoint.transform.position.x >= 238)
        {
            Debug.Log("1");
            skillPoint.transform.position = new Vector3(235, skillPoint.transform.position.y, skillPoint.transform.position.z);
        }
        if (skillPoint.transform.position.x <= 50)
        {
            Debug.Log("2");
            skillPoint.transform.position = new Vector3(48, skillPoint.transform.position.y, skillPoint.transform.position.z);
        }
        if (skillPoint.transform.position.z >= 236)
        {
            Debug.Log("3");
            skillPoint.transform.position = new Vector3(skillPoint.transform.position.x, skillPoint.transform.position.y, 234);
        }
        if (skillPoint.transform.position.z <= 43)
        {
            Debug.Log("4");
            skillPoint.transform.position = new Vector3(skillPoint.transform.position.x, skillPoint.transform.position.y, 41);
        }
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Blink", false);



        CmdSpawnEffectBlink(player);
        
        Vector3 position = skillPoint.transform.position;
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
