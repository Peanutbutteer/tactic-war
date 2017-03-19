using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class EditNamePanel : LobbyInfoPanel
{
	public Button saveButton;
	public InputField inputName;
	public ProfilePanel profilePanel;
	public void Display()
	{
		Display("Edit Name", "Cancel", null);

		if (DataManager.s_Singleton != null)
		{
			inputName.text = DataManager.s_Singleton.playerName;
		}

		saveButton.onClick.AddListener(() =>
		{
			if (DataManager.s_Singleton != null)
			{
				DataManager.s_Singleton.playerName = inputName.text;
				DataManager.s_Singleton.Save();
				profilePanel.Load();
			}

			gameObject.SetActive(false);
		});

	}
}
