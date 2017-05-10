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
    [Range(10f, 50f)]
    public int skillRadius = 20;
    [Range(10f, 50f)]
    public int skillAreaSize = 20;
    [Range(2f, 10f)]
    public int skillPointSize = 2;

    private Vector3 positionSkill;
    private Vector3 positionSpawn;

	public override void OnStartPlayer()
    {
		base.OnStartPlayer();
    }

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        skillPoint.GetComponent<Projector>().orthographicSize = skillPointSize;
        skillArea.GetComponent<Projector>().orthographicSize = skillAreaSize;
        skillArea.SetActive(true);
        skillPoint.SetActive(true);
        positionSkill = new Vector3(horizontal * skillRadius, 1000f * 0.03f, vertical * skillRadius);
        skillPoint.transform.position = player.transform.position + positionSkill;
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        StartCoroutine(CastRoseFire());
    }

    IEnumerator CastRoseFire()
    {
        anim.SetBool("Blink", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Blink", false);

        positionSpawn = new Vector3(
        skillPoint.transform.position.x,
        player.transform.position.y + 3,
        skillPoint.transform.position.z);

        CmdSpawnRoseFire(player , positionSpawn);
        yield return new WaitForSeconds(1.5f);
        CmdSpawnRoseBoom(player , positionSpawn);
        Vector3 position = skillPoint.transform.position;
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
