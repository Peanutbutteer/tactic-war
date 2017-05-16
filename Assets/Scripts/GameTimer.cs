using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
public class GameTimer : NetworkBehaviour
{
	public int time;
	[SerializeField]
	private float timer;
	public Text timeText;
    public GameObject loadingPanel;
	private bool isFinishing = false;

	// Use this for initialization
	void Start()
	{
        ErrorModal.s_Instance.SetupTimer(3f, ()=>
        {
            timer = time * 60f;
            ErrorModal.s_Instance.CloseModal();
            loadingPanel.gameObject.SetActive(false);
        });
        ErrorModal.s_Instance.Show("Waiting");
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
		else if (timer < 0 && !isFinishing)
		{
			isFinishing = true;
			Finish();
		}
	}

	[ClientRpc]
	void RpcUpdateTime(int minutes, int secound)
	{
		timeText.text = String.Format("{0:00}:{1:00}", minutes, secound);
	}

	private void Finish()
	{
		GameManager.s_Singleton.CmdEndGame();
	}
}
