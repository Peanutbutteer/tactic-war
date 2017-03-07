using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class CameraController : MonoBehaviour {

	private Vector3 offset;
	private GameObject player;
	// Use this for initialization
	void Start () {
		

	}

	void Update() {

		if(player!=null) {
			return;
		}
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (var item in players)
		{
			if(item.GetComponent<NetworkIdentity> ().hasAuthority) {
				player = item;
				offset = transform.position - player.transform.position;
			}
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (player!=null) {
			transform.position = player.transform.position + offset;
		}
	}
}
