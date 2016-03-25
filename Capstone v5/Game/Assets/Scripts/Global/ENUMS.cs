using UnityEngine;
using System.Collections;

public class ENUMS : MonoBehaviour {
	public enum CLASSTYPE
	{
		LEO,
		VIRGO,
		GEMINI,
		LIBRA,
		AQUARIUS
	}
    public enum RACETYPE
    {
        CAT_GIRL,
        HUMAN,
        SAYTR,
        BIRD_MAN
    }
    public enum FEATURETYPE
    {
        FEATURE1,
        FEATURE2,
        FEATURE3

    }

	public enum ROLETYPE
	{
		RANGED_DPS,
		MELEE_DPS,
		SUPPORT,
		TANK
	}

	public enum LEVELS
	{
		CHARACTERSELECT,
		EXAMPLELEVEL1,
		EXAMPLELEVEL2
	}





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
