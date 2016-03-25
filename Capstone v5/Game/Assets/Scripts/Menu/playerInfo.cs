using UnityEngine;
using System.Collections;

public class playerInfo : MonoBehaviour 
{
	public ENUMS.RACETYPE[] chosenSprites = new ENUMS.RACETYPE[4];
    public ENUMS.FEATURETYPE[] chosenFeature = new ENUMS.FEATURETYPE[4];
	public ENUMS.CLASSTYPE[] chosenClass = new ENUMS.CLASSTYPE[4];
	public ENUMS.ROLETYPE[] chosenRole = new ENUMS.ROLETYPE[4];


	private static playerInfo instance;
	
	public static playerInfo Instance
	{
		get 
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<playerInfo>();
			}
			
			return playerInfo.instance;
		}
	}



}
