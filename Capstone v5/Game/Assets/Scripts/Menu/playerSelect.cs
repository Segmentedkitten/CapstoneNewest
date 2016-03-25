using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rewired;

public class playerSelect : MonoBehaviour 
{
	private Player player; // The Rewired Player
	public int playerId; // The Rewired player id of this character

	private GameObject spriteParent;
	private GameObject roleParent;
	private GameObject classParent;
    private GameObject featureParent;

	int spriteIndex = 0;
	int classIndex = 0;
	int roleIndex = 0;
    int featureIndex = 0;
	int selectionIndex = 0;
	public bool playerSelected = false;

	void Awake() 
	{
		// Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
		player = ReInput.players.GetPlayer(playerId);
		spriteParent = this.transform.FindChild ("sprite").gameObject;
		classParent = this.transform.FindChild ("ClassBox").gameObject;
		roleParent = this.transform.FindChild("RoleBox").gameObject;
        featureParent = this.transform.FindChild("FeaturesBox").gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{



        if(!playerSelected)
        {
            this.transform.GetChild(4).gameObject.SetActive(false);

        }
        else if (playerSelected)
        {
            
            this.transform.GetChild(4).gameObject.SetActive(true);
        }


		if (player.GetButtonDown ("bottomButton") && !playerSelected) 
		{
			//gets to check mark and overlay box

            playerInfo.Instance.chosenSprites[playerId] = (ENUMS.RACETYPE)spriteIndex;
            playerInfo.Instance.chosenFeature[playerId] = (ENUMS.FEATURETYPE)featureIndex;
			playerInfo.Instance.chosenClass[playerId] = (ENUMS.CLASSTYPE)classIndex;
			playerInfo.Instance.chosenRole[playerId] = (ENUMS.ROLETYPE)roleIndex;
			playerSelected = true;

			if(playerId == 0){
				selectManager.Instance.p1_Selected = true;
			}else if(playerId == 1){
				selectManager.Instance.p2_Selected = true;
			}else if(playerId == 2){
				selectManager.Instance.p3_Selected = true;
			}else if(playerId == 3) {
				selectManager.Instance.p4_Selected = true;
			}



		}

		if (player.GetButtonDown ("rightButton") && playerSelected) 
		{
			//gets to check mark and overlay box
			this.transform.GetChild(4).gameObject.SetActive(false);
			playerSelected = false;

			if(playerId == 0){
				selectManager.Instance.p1_Selected = false;
			}else if(playerId == 1){
				selectManager.Instance.p2_Selected = false;
			}else if(playerId == 2){
				selectManager.Instance.p3_Selected = false;
			}else if(playerId == 3) {
				selectManager.Instance.p4_Selected = false;
			}
		}

		if (!playerSelected) 
		{
			selectPlayer();
		}
	}

	void selectPlayer()
	{



        if (player.GetButtonDown("Dpad_Down")  && selectionIndex < 3)
        {

			selectionIndex ++;



        }
        else if (player.GetButtonDown("Dpad_Up")  && selectionIndex > 0)
        {

			selectionIndex --;


		}
			
	



		if (selectionIndex == 0) {
            
			spriteParent.transform.GetChild (3).GetComponent<Text> ().color = Color.yellow;
			classParent.transform.GetChild (1).GetComponent<Text> ().color = Color.white;
			roleParent.transform.GetChild(2).GetComponent<Text> ().color = Color.white;
            featureParent.transform.GetChild(2).GetComponent<Text>().color = Color.white;


            if (player.GetButtonDown("Dpad_Right") &&  spriteIndex < 3)
            {
                featureIndex = 0;
                featureParent.transform.GetChild(2).GetComponent<Text>().text = spriteArray.Instance.featureTexts[featureIndex];
                featureParent.transform.GetChild(1).gameObject.SetActive(false);
					
				if (spriteIndex == 0) {
					//for left arrow
					spriteParent.transform.GetChild (1).gameObject.SetActive (true);
				}
					
				spriteIndex++;
				spriteParent.GetComponent<Image> ().sprite = spriteArray.Instance.sprites [spriteIndex];
                switch (spriteIndex)
                {
                    case 0:
                        {
                            spriteParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(154, 282);
                            featureParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(154, 282);
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Cg_Features[featureIndex];
                            break;
                        }
                    case 1:
                        {
                            spriteParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(180, 390);
                            featureParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(180, 390);
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Hm_Features[featureIndex];
                            break;

                        }
                    case 2:
                        {
                            spriteParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(170, 370);
                            featureParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(170, 370);
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Sy_Features[featureIndex];
                            break;

                        }
                    case 3:
                        {
                            spriteParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(222, 406);
                            featureParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(222, 406);
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Bm_Features[featureIndex];
                            break;

                        }
                }            
					
					
				//player text
				spriteParent.transform.GetChild (3).GetComponent<Text> ().text = spriteArray.Instance.imageTexts [spriteIndex];
					
				if (spriteIndex == 3) {
					//for right arrow
					spriteParent.transform.GetChild (2).gameObject.SetActive (false);
				}


            }
            else if (player.GetButtonDown("Dpad_Left")  && spriteIndex > 0)
            {
                featureIndex = 0;
                featureParent.transform.GetChild(2).GetComponent<Text>().text = spriteArray.Instance.featureTexts[featureIndex];
                featureParent.transform.GetChild(1).gameObject.SetActive(false);
					
				if (spriteIndex == 3) {
					//for right arrow
					spriteParent.transform.GetChild (2).gameObject.SetActive (true);
				}
						
				spriteIndex--;
				spriteParent.GetComponent<Image> ().sprite = spriteArray.Instance.sprites [spriteIndex];
                switch (spriteIndex)
                {
                    case 0:
                        {
                            spriteParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(154, 282);
                            featureParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(154, 282);
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Cg_Features[featureIndex];
                            break;
                        }
                    case 1:
                        {
                            spriteParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(180, 390);
                            featureParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(180, 390);
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Hm_Features[featureIndex];
                            break;

                        }
                    case 2:
                        {
                            spriteParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(170, 370);
                            featureParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(170, 370);
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Sy_Features[featureIndex];
                            break;

                        }
                    case 3:
                        {
                            spriteParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(222, 406);
                            featureParent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(222, 406);
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Bm_Features[featureIndex];
                            break;

                        }
                }            
					
						
				//player text
				spriteParent.transform.GetChild (3).GetComponent<Text> ().text = spriteArray.Instance.imageTexts [spriteIndex];
						
				if (spriteIndex == 0) {
					//for left arrow
					spriteParent.transform.GetChild (1).gameObject.SetActive (false);
				}	
			}
							
						

		} 
        else if (selectionIndex == 1) 
        {
			spriteParent.transform.GetChild (3).GetComponent<Text> ().color = Color.white;
			classParent.transform.GetChild (1).GetComponent<Text> ().color = Color.white;
            featureParent.transform.GetChild(2).GetComponent<Text>().color = Color.yellow;
			roleParent.transform.GetChild(2).GetComponent<Text> ().color = Color.white;

			if (player.GetButtonDown ("Dpad_Right") && featureIndex < 2) 
            {
				if (featureIndex == 0) 
                {
					//for left arrow
					featureParent.transform.GetChild (1).gameObject.SetActive (true);
				}
						
				featureIndex++;
                switch(spriteIndex)
                {
                    case 0:
                        {
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Cg_Features[featureIndex];
                            break;
                        }
                    case 1:
                        {
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Hm_Features[featureIndex];
                            break;

                        }
                    case 2:
                        {
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Sy_Features[featureIndex];
                            break;

                        }
                    case 3:
                        {
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Bm_Features[featureIndex];
                            break;

                        }
                }            
						
						
				//player text
				featureParent.transform.GetChild (2).GetComponent<Text> ().text = spriteArray.Instance.featureTexts [featureIndex];
			
						
				if (featureIndex == 2) {
					//for right arrow
					featureParent.transform.GetChild (3).gameObject.SetActive (false);
				}
            }
            else if (player.GetButtonDown("Dpad_Left") && featureIndex > 0)
            {

                if (featureIndex == 2)
                {
                    //for right arrow
                    featureParent.transform.GetChild(3).gameObject.SetActive(true);
                }

                featureIndex--;

                switch (spriteIndex)
                {
                    case 0:
                        {
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Cg_Features[featureIndex];
                            break;
                        }
                    case 1:
                        {
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Hm_Features[featureIndex];
                            break;

                        }
                    case 2:
                        {
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Sy_Features[featureIndex];
                            break;

                        }
                    case 3:
                        {
                            featureParent.GetComponent<Image>().sprite = spriteArray.Instance.Bm_Features[featureIndex];
                            break;

                        }
                }            
					


                //player text
                featureParent.transform.GetChild(2).GetComponent<Text>().text = spriteArray.Instance.featureTexts[featureIndex];

                if (featureIndex == 0)
                {
                    //for left arrow
                    featureParent.transform.GetChild(1).gameObject.SetActive(false);
                }

            }
        }
    

            else if (selectionIndex == 2) {

      
			spriteParent.transform.GetChild (3).GetComponent<Text> ().color = Color.white;
			classParent.transform.GetChild (1).GetComponent<Text> ().color = Color.yellow;
			roleParent.transform.GetChild(2).GetComponent<Text> ().color = Color.white;
                featureParent.transform.GetChild(2).GetComponent<Text>().color = Color.white;
			if (player.GetButtonDown ("Dpad_Right") && classIndex < 4) {


				if (classIndex == 0) {
					//for left arrow
					classParent.transform.GetChild (2).gameObject.SetActive (true);
				}
						
				classIndex++;
						
						
				//player text
				classParent.transform.GetChild (1).GetComponent<Text> ().text = spriteArray.Instance.classTexts [classIndex];
			
						
				if (classIndex == 4) {
					//for right arrow
					classParent.transform.GetChild (3).gameObject.SetActive (false);
				}




            }
            else if (player.GetButtonDown("Dpad_Left") && classIndex > 0)
            {

				if (classIndex == 4) {
					//for right arrow
					classParent.transform.GetChild (3).gameObject.SetActive (true);
				}
						
				classIndex--;
						
						
				//player text
				classParent.transform.GetChild (1).GetComponent<Text> ().text = spriteArray.Instance.classTexts [classIndex];
						
				if (classIndex == 0) {
					//for left arrow
					classParent.transform.GetChild (2).gameObject.SetActive (false);
				}

			}
		}

			else if(selectionIndex == 3)
			{
               
                
					spriteParent.transform.GetChild (3).GetComponent<Text> ().color = Color.white;
					classParent.transform.GetChild (1).GetComponent<Text> ().color = Color.white;
					roleParent.transform.GetChild(2).GetComponent<Text> ().color = Color.yellow;
                    featureParent.transform.GetChild(2).GetComponent<Text>().color = Color.white;
                    if (player.GetButtonDown("Dpad_Right")  && roleIndex < 3)
                    {
						
							if (roleIndex == 0) {
								//for left arrow
								roleParent.transform.GetChild(1).gameObject.SetActive (true);
							}
							
						roleIndex++;

							
							//player text
								roleParent.transform.GetChild(2).GetComponent<Text> ().text = spriteArray.Instance.roleTexts[roleIndex];
							
						if (roleIndex == 3) {
								//for right arrow
								roleParent.transform.GetChild(3).gameObject.SetActive (false);
							}


                    }
                    else if (player.GetButtonDown("Dpad_Left")  && roleIndex > 0 )
                    {

                        if (roleIndex == 3)
                        {
                            //for right arrow
                            roleParent.transform.GetChild(3).gameObject.SetActive(true);
                        }

                        roleIndex--;

                        //player text
                        roleParent.transform.GetChild(2).GetComponent<Text>().text = spriteArray.Instance.roleTexts[roleIndex];

                        if (roleIndex == 0)
                        {
                            //for left arrow
                            roleParent.transform.GetChild(1).gameObject.SetActive(false);
                        }
                    }

						
				

			}



		}
	}

