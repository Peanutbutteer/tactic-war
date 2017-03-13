using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
using UnityEngine.Networking;
public class BlockController : NetworkBehaviour
{


    public GameObject block;
	public GameObject cooldownPrefab;
    private Animator anim;
	private GameObject cooldownSkill;

    [SyncVar(hook = "OnShieldLevelChanged")]
    //The current shield level of the tank.
    public bool m_ShieldLevel;

    // Use this for initialization
    void Start () {
		GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
		cooldownSkill = Instantiate (cooldownPrefab, canvas.transform, false);
        anim = GetComponent<Animator>();
        block.SetActive(false);
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
            StartCoroutine(DisableBlock());
        }

    }
   
    void OnShieldLevelChanged(bool active)
    {
        block.SetActive(active);
    }

    IEnumerator DisableBlock()
    {
        yield return new WaitForSeconds(1f);
        CmdDisableSpawnBlockSkill();
    }

    [Command]
    void CmdSpawnBlockSkill()
    {
        m_ShieldLevel = true;
    }

    [Command]
    void CmdDisableSpawnBlockSkill()
    {
        m_ShieldLevel = false;
    }



    //var myNewSmoke = Instantiate(poisonSmoke, Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    //myNewSmoke.transform.parent = gameObject.transform;
}
