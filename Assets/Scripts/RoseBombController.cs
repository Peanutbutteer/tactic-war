using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RoseBombController : Skill
{

    public GameObject rosefirePrefab;
    public GameObject roseboomPrefab;
    public int skillRadius = 20;

    private GameObject rosebombPoint;
    private Projector rendRoseBombPoint;
    private GameObject rosebombArea;
    private Vector3 positionSkill;
    private Vector3 positionSpawn;

    public override void Start()
    {
        base.Start();
        rosebombPoint = FindObjectInPlayer("SkillPoint");
        rosebombArea = FindObjectInPlayer("SkillArea");

        rendRoseBombPoint = rosebombPoint.GetComponent<Projector>();
        rendRoseBombPoint.orthographicSize = 2;
        rendRoseBombPoint.enabled = false;
    }

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        rosebombArea.SetActive(true);
        rendRoseBombPoint.enabled = true;
        positionSkill = new Vector3(horizontal * skillRadius, 1000f * 0.03f, vertical * skillRadius);
        rosebombPoint.transform.position = player.transform.position + positionSkill;
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        cooldownSkill.SetActive(true);
        StartCoroutine(CastRoseFire());
    }

    IEnumerator CastRoseFire()
    {
        rendRoseBombPoint.enabled = false;
        rosebombArea.SetActive(false);
        anim.SetBool("Blink", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Blink", false);

        positionSpawn = new Vector3(
            rosebombPoint.transform.position.x,
            player.transform.position.y + 3, 
            rosebombPoint.transform.position.z);

        CmdSpawnRoseFire(player , positionSpawn);
        yield return new WaitForSeconds(3f);
        CmdSpawnRoseBoom(player , positionSpawn);
        Vector3 position = rosebombPoint.transform.position;
        position.y = 0;
        positionSkill = new Vector3(0, 0, 0);

    }
    [Command]
    void CmdSpawnRoseFire(GameObject player , Vector3 positionSpawn)
    {
        
        var RoseFire = (GameObject)Instantiate(rosefirePrefab, positionSpawn, player.transform.rotation);
        NetworkServer.Spawn(RoseFire);
        Destroy(RoseFire, 5f);
    }

    [Command]
    void CmdSpawnRoseBoom(GameObject player, Vector3 positionSpawn)
    {
        var RoseBoom = (GameObject)Instantiate(roseboomPrefab, positionSpawn, player.transform.rotation);
        NetworkServer.Spawn(RoseBoom);
        Destroy(RoseBoom, 4f);
    }
}
