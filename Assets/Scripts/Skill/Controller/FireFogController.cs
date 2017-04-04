using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireFogController : Skill {

    public GameObject firefogPrefab;
    [Range(10f, 50f)]
    public int skillRadius = 20;
    [Range(10f, 50f)]
    public int skillAreaSize = 20;
    [Range(2f, 20f)]
    public int skillSize = 12;
    
    private Vector3 positionSkill;

	public override void OnStartPlayer()
    {
		base.OnStartPlayer();
    }

    public override void ButtonDirection(float vertical, float horizontal)
    {
        base.ButtonDirection(vertical, horizontal);
        skillPoint.GetComponent<Projector>().orthographicSize = skillSize;
        skillArea.GetComponent<Projector>().orthographicSize = skillAreaSize;

        skillPoint.SetActive(true);
        skillArea.SetActive(true);
        positionSkill = new Vector3(horizontal * skillRadius, 1000f * 0.03f, vertical * skillRadius);
        skillPoint.transform.position = player.transform.position + positionSkill;
    }

    public override void ButtonUp()
    {
        base.ButtonUp();
        StartCoroutine(CastFirefog());
    }

    IEnumerator CastFirefog()
    {
        anim.SetBool("Blink", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Blink", false);

        Vector3 positionFirefog = new Vector3(
            skillPoint.transform.position.x, 
            player.transform.position.y, 
            skillPoint.transform.position.z);

        CmdSpawnFirefog(player , positionFirefog);
        
        Vector3 position = skillPoint.transform.position;
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
