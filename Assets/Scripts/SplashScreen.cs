using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashScreen : MonoBehaviour {
	public string sceneName = "LobbyScene";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			GoToNextScene ();
		}
	}

	void GoToNextScene() {
		SceneManager.LoadScene (sceneName);
	}
}
