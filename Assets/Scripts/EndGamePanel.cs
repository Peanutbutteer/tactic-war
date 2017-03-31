using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndGamePanel : MonoBehaviour
{
	public Text winner;

	public void UpdateWinnerText(int winnerNo)
	{
		string winnerTxt = "Blue Win";
		if (winnerNo != 0)
		{
			winner.color = new Color32(201, 41, 45, 255);
			winnerTxt = "Red Win";
		}
		winner.text = winnerTxt;
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}
}
