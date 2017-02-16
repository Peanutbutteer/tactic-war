using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject startGameScene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void OnStart() {
		startGameScene.SetActive (false);
	}

	public void OnExit() {
		Application.Quit ();
	}
}
