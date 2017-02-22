using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class BlockController : MonoBehaviour {

    public GameObject block;
    public GameObject player;

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = player.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (CnInputManager.GetButtonUp("BlockButton"))
        {
            block.SetActive(true);
            anim.SetBool("Block", true);
            StartCoroutine(Block());
        }
    }

    IEnumerator Block()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Block", false);
        yield return new WaitForSeconds(0.5f);
        block.SetActive(false);
    }
}
