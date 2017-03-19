using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour {
	public Text playerName;

	// Use this for initialization
	void Start () {
		Load();
	}

	public void Load()
	{
		if (DataManager.s_Singleton != null)
		{
			playerName.text = DataManager.s_Singleton.playerName;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
