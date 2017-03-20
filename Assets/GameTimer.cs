using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class GameTimer : MonoBehaviour
{
	public int time;
	[SerializeField]
	private float timer;
	public Text timeText;

	// Use this for initialization
	void Start()
	{
		timer = time * 60f;
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if (timer <= 0f)
		{
			Finish();
		}
		timer -= Time.unscaledDeltaTime;
		int minutes = (int)timer / 60;
		int secound = (int)(timer % 60);
		timeText.text = String.Format("{0:00}:{1:00}", minutes, secound);
	}

	private void Finish()
	{

	}
}
