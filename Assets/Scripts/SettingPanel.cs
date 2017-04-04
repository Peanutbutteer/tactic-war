using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingPanel : MonoBehaviour
{
	public Button singleButton;

	public void Display()
	{
		singleButton.onClick.RemoveAllListeners();

		singleButton.onClick.AddListener(() => { gameObject.SetActive(false); });

		gameObject.SetActive(true);
	}

	public void close()
	{
		gameObject.SetActive(false);
	}
}