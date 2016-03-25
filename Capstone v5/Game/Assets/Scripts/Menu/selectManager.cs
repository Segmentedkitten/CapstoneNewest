using UnityEngine;
using System.Collections;

public class selectManager : MonoBehaviour {


	public bool p1_Selected = false;
	public bool p2_Selected = false;
	public bool p3_Selected = false;
	public bool p4_Selected = false;
	public bool allSelected = false;
    
	private bool runOnce = false;
	public GameObject gemini;
	public GameObject leo;
	public GameObject virgo;
	public GameObject libra;
	public GameObject aquarius;

    public GameObject cg_sprites;
    public GameObject hm_sprites;
    public GameObject sy_Sprites;
    public GameObject bm_sprites;

    public Sprite[] Cg_Features = new Sprite[3];
    public Sprite[] Sy_Features = new Sprite[3];
    public Sprite[] Hm_Features = new Sprite[3];
    public Sprite[] Bm_Features = new Sprite[3];

    public GameObject blipPrefab;




	private static selectManager instance;

	public static selectManager Instance
	{
		get 
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<selectManager>();
			}
			
			return selectManager.instance;
		}
	}

	// Use this for initialization
	void Start () {

        
	}

    void Awake()
    {

        //playerParents = GameObject.Find("playerSelect_Canvas");
    }
	
	// Update is called once per frame
	void Update () {

        if(gameManager.Instance.numOfPlayers == 1)
        {
            p2_Selected = true;
            p3_Selected = true;
            p4_Selected = true;
           
            

        }
        else if(gameManager.Instance.numOfPlayers == 2)
        {
            p3_Selected = true;
            p4_Selected = true;
           

        }
        else if(gameManager.Instance.numOfPlayers == 3)
        {
           
            p4_Selected = true;
     
        }


      
      


		if (p1_Selected && p2_Selected && p3_Selected && p4_Selected) {

			allSelected = true;

		}

		if (allSelected && !runOnce) {
			for(int i = 0; i < gameManager.Instance.numOfPlayers; i++)
			{
				initializePlayer(i, playerInfo.Instance.chosenSprites[i],playerInfo.Instance.chosenFeature[i],playerInfo.Instance.chosenClass[i],playerInfo.Instance.chosenRole[i]);

			}
			runOnce = true;
			gameManager.Instance.changeLevel(2);
		}
	
	}


	public void initializePlayer(int playerID, ENUMS.RACETYPE chosenSprite, ENUMS.FEATURETYPE chosenFeature, ENUMS.CLASSTYPE chosenClass, ENUMS.ROLETYPE chosenRole)
	{
		GameObject playerHolder = GameObject.Find ("Players");
		GameObject prefabTemp = null;
		
		switch(chosenClass)
		{
		case ENUMS.CLASSTYPE.AQUARIUS:
			prefabTemp = Instantiate(aquarius);
			prefabTemp.gameObject.name = "Player" + (playerID + 1);
			prefabTemp.gameObject.GetComponent<Aquarius> ().Player_ID = playerID;
			break;
		case ENUMS.CLASSTYPE.GEMINI:

			prefabTemp = Instantiate(gemini);
			prefabTemp.gameObject.name = "Player" + (playerID + 1);
			prefabTemp.gameObject.GetComponent<Gemini> ().Player_ID = playerID;
			break;
		case ENUMS.CLASSTYPE.LEO:
			prefabTemp = Instantiate(leo);
			prefabTemp.gameObject.name = "Player" + (playerID + 1);
			prefabTemp.gameObject.GetComponent<Leo> ().Player_ID = playerID;
			break;
		case ENUMS.CLASSTYPE.LIBRA:
			prefabTemp = Instantiate(libra);
			prefabTemp.gameObject.name = "Player" + (playerID + 1);
			prefabTemp.gameObject.GetComponent<Libra> ().Player_ID = playerID;
			break;
		case ENUMS.CLASSTYPE.VIRGO:
			prefabTemp = Instantiate(virgo);
			prefabTemp.gameObject.name = "Player" + (playerID + 1);
			prefabTemp.gameObject.GetComponent<Virgo> ().Player_ID = playerID;
			break;

		}

		prefabTemp.transform.parent = playerHolder.transform;
        

        GameObject blip = Instantiate(blipPrefab);
        blip.transform.parent = prefabTemp.transform;
        blip.transform.localScale = new Vector3(blip.transform.localScale.x / 6, blip.transform.localScale.y / 6, blip.transform.localScale.z / 6);
        GameObject spriteTemp = null;
		
        switch(chosenSprite)
        {

            case ENUMS.RACETYPE.CAT_GIRL:
                {
                    spriteTemp = Instantiate(cg_sprites);
                   
                    switch (chosenFeature)
                    {
                        case ENUMS.FEATURETYPE.FEATURE1:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Cg_Features[0];
                                break;
                            }
                        case ENUMS.FEATURETYPE.FEATURE2:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Cg_Features[1];
                                break;
                            }
                        case ENUMS.FEATURETYPE.FEATURE3:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Cg_Features[2];
                                break;
                            }
                    }

                    break;
                }
            case ENUMS.RACETYPE.HUMAN:
                {
                    spriteTemp = Instantiate(hm_sprites);
                   
                    switch (chosenFeature)
                    {
                        case ENUMS.FEATURETYPE.FEATURE1:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Hm_Features[0];
                                break;
                            }
                        case ENUMS.FEATURETYPE.FEATURE2:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Hm_Features[1];
                                break;
                            }
                        case ENUMS.FEATURETYPE.FEATURE3:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Hm_Features[2];
                                break;
                            }
                    }
                    break;
                }
            case ENUMS.RACETYPE.SAYTR:
                {
                    spriteTemp = Instantiate(sy_Sprites);
                  
                    switch (chosenFeature)
                    {
                        case ENUMS.FEATURETYPE.FEATURE1:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Sy_Features[0];
                                break;
                            }
                        case ENUMS.FEATURETYPE.FEATURE2:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Sy_Features[1];
                                break;
                            }
                        case ENUMS.FEATURETYPE.FEATURE3:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Sy_Features[2];
                                break;
                            }
                    }
                    break;
                }
            case ENUMS.RACETYPE.BIRD_MAN:
                {
                    spriteTemp = Instantiate(bm_sprites);
                
                    switch (chosenFeature)
                    {
                        case ENUMS.FEATURETYPE.FEATURE1:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Bm_Features[0];
                                break;
                            }
                        case ENUMS.FEATURETYPE.FEATURE2:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Bm_Features[1];
                                break;
                            }
                        case ENUMS.FEATURETYPE.FEATURE3:
                            {
                                spriteTemp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Bm_Features[2];
                                break;
                            }
                    }
                    break;
                }

        }
        spriteTemp.transform.parent = prefabTemp.transform;
        prefabTemp.GetComponent<PlayerScript>().initializePlayer();                                     

       





	}
}
