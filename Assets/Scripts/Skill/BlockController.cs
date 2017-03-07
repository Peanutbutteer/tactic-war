using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;
public class BlockController : NetworkBehaviour
{

    public GameObject block;
    public GameObject controller;
	public GameObject cooldownPrefab;
    private Animator anim;
	private GameObject cooldownSkill;

	// Use this for initialization
	void Start () {
		GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
		cooldownSkill = Instantiate (cooldownPrefab, canvas.transform, false);
        anim = GetComponent<Animator>();
	}

	void FixedUpdate() {
	}
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
        {
            return;
        }
        if (CnInputManager.GetButtonUp("BlockButton"))
        {
			cooldownSkill.SetActive (true);
            CmdSpawnBlockSkill();
            anim.SetBool("Block", true);
            StartCoroutine(Block());
        }

    }

    IEnumerator Block()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Block", false);
        yield return new WaitForSeconds(0.5f);
    }

    [Command]
    void CmdSpawnBlockSkill()
    {
        var blockSkill = (GameObject)Instantiate(
            block
            , new Vector3(transform.position.x,transform.position.y+(70*0.03f),transform.position.z+(25*0.03f))
            , transform.rotation);
        blockSkill.transform.parent = transform;
        NetworkServer.Spawn(blockSkill);
        Destroy(blockSkill, 1f);
    }


    //var myNewSmoke = Instantiate(poisonSmoke, Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    //myNewSmoke.transform.parent = gameObject.transform;
}
