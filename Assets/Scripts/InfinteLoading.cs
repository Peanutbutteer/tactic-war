using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfinteLoading : MonoBehaviour {

	public Transform LoadingBar;
	[SerializeField] private float current;
	[SerializeField]
	private float speed;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (current < 100)
		{
			current += speed * Time.deltaTime;
		}
		else
		{
			current = 0;
		}

		LoadingBar.GetComponent<Image>().fillAmount = current / 100;
	}
}
