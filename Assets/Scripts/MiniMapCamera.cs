using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class MiniMapCamera : MonoBehaviour
{
	private GameObject player;
	private Vector3 offset = new Vector3(0f, 100f, 0f);
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (player != null)
		{
			return;
		}
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (var item in players)
		{
			if (item.GetComponent<NetworkIdentity>().hasAuthority)
			{
				player = item;
			}
		}
	}

	void LateUpdate()
	{
		if (player != null)
		{
			transform.position = player.transform.position + offset;
		}
	}
}
