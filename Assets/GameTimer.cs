﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
public class GameTimer : NetworkBehaviour
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
		if (isServer && timer > 0)
		{
			timer -= Time.deltaTime;
			int minutes = (int)timer / 60;
			int secound = (int)(timer % 60);
			RpcUpdateTime(minutes, secound);
		}
	}

	[ClientRpc]
	void RpcUpdateTime(int minutes, int secound)
	{
		if (timer <= 0f)
		{
			Finish();
		}
		timeText.text = String.Format("{0:00}:{1:00}", minutes, secound);
	}

	private void Finish()
	{

	}
}
