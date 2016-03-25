using UnityEngine;
using System;
using System.Collections;
using Rewired;

public class save_load : MonoBehaviour 
{
	private static save_load instance;
	
	public static save_load Instance
	{
		get 
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<save_load>();
			}
			
			return save_load.instance;
		}
	}

	public void load()
	{
		string playerContent;
		
		if(PlayerPrefs.GetString("playerContent") != null)
		{
			playerContent = PlayerPrefs.GetString("playerContent");
		}
		
		else
		{
			playerContent = string.Empty;
		}

		if(playerContent != string.Empty)
		{
			string[] splitContent = playerContent.Split (';');
			
			//so it check the character before the semicolon
			//to prevent checking of empty space after semicolon
			for(int i = 0; i < splitContent.Length - 1; i++)
			{
				string[] splitValues = splitContent[i].Split('-');
				int index = Int32.Parse(splitValues[0]);
				float speed = float.Parse(splitValues[1]);

				Inventory.Instance.players[index].speed = speed;

				Debug.Log ("new speed is:" + speed);
			}
		}

		Inventory.Instance.loadInventory();
	}

	public void save()
	{
		string playerContent = string.Empty;

		Debug.Log (Inventory.Instance.players.Count);

		for(int i = 0; i < Inventory.Instance.players.Count; i++)
		{
			playerContent += i + "-" + Inventory.Instance.players[i].speed + ";"; 
			Debug.Log ("speed is:" + Inventory.Instance.players[i].speed);
		}

		//uses playerprefs (built into unity) to save data, rather than using an xml
		PlayerPrefs.SetString ("playerContent", playerContent);

		Inventory.Instance.saveInventory();
	}
}
