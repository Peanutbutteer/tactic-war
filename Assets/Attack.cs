using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Attack : NetworkBehaviour
{

    public GameObject Staff;
    Animator anim;
    
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	void Update () {

        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown("space"))
        {
            Staff.GetComponent<BoxCollider>().enabled = true;
            anim.SetBool("AttackStaff",true);
            StartCoroutine(AttackStaff());
        }
    }

    IEnumerator AttackStaff()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("AttackStaff", false);
        Staff.GetComponent<BoxCollider>().enabled = false;
    }

    public void AttackPlayer(GameObject player)
    {
        CmdServerTakeDamage(player,10);
    }

    [Command]
    public void CmdServerTakeDamage(GameObject player, int amount)
    {
        var health = player.GetComponent<PlayerHealth>();
        health.TakeDamage(amount);
        Debug.Log("CmdServerTakeDamage");
    }
}
