using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    static public List<PlayerMageController> s_Player = new List<PlayerMageController>();

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void OnStart() {

	}

	public void OnExit() {
		Application.Quit ();
	}

}
