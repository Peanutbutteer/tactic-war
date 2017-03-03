using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.UI;
public class BlockController : MonoBehaviour {

    public GameObject block;
    public GameObject player;
	public GameObject cooldownPrefab;
    private Animator anim;

	private GameObject cooldownSkill;

	// Use this for initialization
	void Start () {
		GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
		cooldownSkill = Instantiate (cooldownPrefab, canvas.transform, false);
        anim = player.GetComponent<Animator>();
	}

	void FixedUpdate() {
	}
	
	// Update is called once per frame
	void Update () {
        if (CnInputManager.GetButtonUp("BlockButton"))
        {
			cooldownSkill.SetActive (true);
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
