using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class DataManager : MonoBehaviour
{
	public static DataManager s_Singleton;

	PlayerData playerData = new PlayerData();

	// Use this for initialization
	void Start()
	{

	}

	void Awake()
	{
		if (s_Singleton == null)
		{
			DontDestroyOnLoad(gameObject);
			s_Singleton = this;
			Load();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public string playerName {
		get { return playerData.playerName; }
		set { playerData.playerName = value; }
	}

	public string GetFilePath()
	{
		return Application.persistentDataPath + "/playerInfo.sav";
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(GetFilePath());
		bf.Serialize(file,playerData);
		file.Close();
	}

	public void Load()
	{
		if (File.Exists(GetFilePath()))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(GetFilePath(), FileMode.Open);
			playerData = (PlayerData)bf.Deserialize(file);
			file.Close();
		}
	}

}

[Serializable]
public class PlayerData
{
	public string playerName = "player";
}