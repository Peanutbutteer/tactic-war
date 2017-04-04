using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreHUD : MonoBehaviour {

    public Text blueScore;
    public Text redScore;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateScore(int[] score)
    {
		if (score.Length == 2)
		{
			redScore.text = score[1].ToString();
			blueScore.text = score[0].ToString();
		}
		else
		{
			redScore.text = score[0].ToString();
		}
    }
}
