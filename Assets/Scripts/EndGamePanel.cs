using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndGamePanel : MonoBehaviour
{
	public Text winner;

	public void UpdateWinnerText(bool hasWinner,int winnerNo,string winnerName)
	{
		string winnerTxt = "Draw";
		if (hasWinner)
		{
			winnerTxt = winnerName + " Win";
			if (winnerNo == 0) {
				winner.color = new Color32 (108,166,200,255);
			} else {
				winner.color = new Color32 (201, 41, 45, 255);
			}
		}
		winner.text = winnerTxt;
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}
}
