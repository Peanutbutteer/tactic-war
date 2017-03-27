using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireFogController : Skill {

    public GameObject firefogPrefab;
    public int skillRadius = 15;

    private GameObject firefogPoint;
    private GameObject firefogArea;
    private Projector rendFirefogPoint;
    private Projector rendFirefogArea;
    private Vector3 positionSkill;

    public override void Start()
    {
        base.Start();
        firefogPoint = FindObjectInPlayer("SkillPoint");
        firefogArea = FindObjectInPlayer("SkillArea");

        rendFirefogPoint = firefogPoint.GetComponent<Projector>();
        rendFirefogPoint.orthographicSize = 12;
        rendFirefogPoint.enabled = false;

        rendFirefogArea = firefogArea.GetComponent<Projector>();
        rendFirefogArea.orthographicSize = 20;

    }

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        firefogArea.SetActive(true);
        rendFirefogPoint.enabled = true;
        positionSkill = new Vector3(horizontal * skillRadius, 1000f * 0.03f, vertical * skillRadius);
        firefogPoint.transform.position = player.transform.position + positionSkill;
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        cooldownSkill.SetActive(true);
        StartCoroutine(CastFirefog());
    }

    IEnumerator CastFirefog()
    {
        rendFirefogPoint.enabled = false;
        firefogArea.SetActive(false);
        anim.SetBool("Blink", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Blink", false);

        Vector3 positionFirefog = new Vector3(
            firefogPoint.transform.position.x, 
            player.transform.position.y, 
            firefogPoint.transform.position.z);

        CmdSpawnFirefog(player , positionFirefog);
        
        Vector3 position = firefogPoint.transform.position;
        position.y = 0;
        positionSkill = new Vector3(0, 0, 0);

    }
    [Command]
    void CmdSpawnFirefog(GameObject player , Vector3 positionFirefog)
    {
        var Firefog = (GameObject)Instantiate(firefogPrefab, positionFirefog, player.transform.rotation);
        NetworkServer.Spawn(Firefog);
        Destroy(Firefog, 7.0f);
    }
}
