using UnityEngine;
using System.Collections;

public class spriteArray : MonoBehaviour 
{
	public Sprite[] sprites = new Sprite[4];
    public Sprite[] Cg_Features = new Sprite[3];
    public Sprite[] Sy_Features = new Sprite[3];
    public Sprite[] Hm_Features = new Sprite[3];
    public Sprite[] Bm_Features = new Sprite[3];
	public string[] imageTexts = new string[]{"Cat Girl","Human","Saytr","Bird Man"};
	public string[] roleTexts = new string[]{"Ranged", "Melee", "Tank" , "Support"};
	public string[] classTexts = new string[]{"Leo" , "Virgo", "Gemini" , "Libra", "Aquarius"};
    public string[] featureTexts = new string[]{"Feature 1", "Feature 2", "Feature 3"};
   


	private static spriteArray instance;
	
	public static spriteArray Instance
	{
		get 
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<spriteArray>();
			}

			return spriteArray.instance;
		}
	}
}
