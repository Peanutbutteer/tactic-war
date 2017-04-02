﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashScreen : MonoBehaviour {
	public string sceneName = "LobbyScene";
    public AudioClip sound;
    public AudioSource source;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		if (Input.anyKey) {
            source.clip = sound;
            source.Play();
            GoToNextScene ();
		}
	}

	void GoToNextScene() {
		SceneManager.LoadScene (sceneName);
	}
}
