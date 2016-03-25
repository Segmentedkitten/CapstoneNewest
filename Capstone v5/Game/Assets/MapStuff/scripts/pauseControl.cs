using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine.UI;

public class pauseControl : MonoBehaviour 
{
    public GameObject pauseMenu;
	public List<Canvas> pauseScreens;
	List<Text> pauseTexts = new List<Text>();
	public int screenNumber = 0;
	public GameObject cursorPrefab;
	public GameObject cursor;
	private Player player; // The Rewired Player
	bool onPause = false;
	public Camera mapCamera;
	public GameObject waypointPrefab;
	GameObject waypoint;
    public GameObject centerPoint;

	// Use this for initialization
	void Start () 
	{
		createMouseIcon ();
		
        foreach (Transform child in pauseMenu.transform.FindChild("texts"))
        {
            pauseTexts.Add(child.GetComponent<Text>());
        }

        pauseTexts[screenNumber].color = Color.yellow;
    }
	
	// Update is called once per frame
	void Update () 
	{
		if (gameManager.Instance.Paused) 
		{
			if(!onPause)
			{
			    moveCursor.Instance.setID(gameManager.Instance.player);
				player = gameManager.Instance.player;
				pauseScreens[screenNumber].GetComponent<CanvasGroup>().alpha = 1;
                pauseMenu.GetComponent<CanvasGroup>().alpha = 1;
                mapCamera.gameObject.SetActive(true);
				onPause = true;
			}

			if(screenNumber == 1)
			{
                float moveHorizontal = player.GetAxis("moveHorizontal") * 1.1f;
                float moveVertical = player.GetAxis("moveVertical") * 1.1f;
                float l_triggerValue = player.GetAxis("leftTrigger");
                float r_triggerValue = player.GetAxis("rightTrigger");

                mapCamera.transform.Translate(new Vector2(moveHorizontal, moveVertical));
                limitCamera();

                if (r_triggerValue > 0)
                {
                    //ZoomOrthoCamera(mapCamera.ScreenToWorldPoint(this.transform.GetChild(0).transform.position), 1.2f);
                    ZoomOrthoCamera(.4f);
                    //mapCamera.orthographicSize -= 1;
                }

                if (l_triggerValue > 0)
                {
                    //ZoomOrthoCamera(mapCamera.ScreenToWorldPoint(this.transform.GetChild(0).transform.position), -1.2f);
                    ZoomOrthoCamera(-.4f);
                    //mapCamera.orthographicSize += 1;
                }

                //this.GetComponent<Image>().acti = false;
            }

			if(player.GetButtonDown("rightBumper"))
			{
				if(screenNumber < 1)
				{
					pauseTexts[screenNumber].color = Color.black;
					pauseScreens[screenNumber].GetComponent<CanvasGroup>().alpha = 0;

					screenNumber++;

					pauseTexts[screenNumber].color = Color.yellow;
					pauseScreens[screenNumber].GetComponent<CanvasGroup>().alpha = 1;

					//cursor.transform.SetParent(pauseScreens[screenNumber].transform);
				}
			}

			if(player.GetButtonDown("leftBumper"))
			{
				if(screenNumber > 0)
				{
					pauseTexts[screenNumber].color = Color.black;
					pauseScreens[screenNumber].GetComponent<CanvasGroup>().alpha = 0;

					screenNumber--;

					pauseTexts[screenNumber].color = Color.yellow;
					pauseScreens[screenNumber].GetComponent<CanvasGroup>().alpha = 1;

					//cursor.transform.SetParent(pauseScreens[screenNumber].transform);
				}
			}

			if(player.GetButtonDown("bottomButton"))
			{
				if(screenNumber == 1)
				{
					if(waypoint == null)
					{
						createWaypoint();
					}

					else
					{
						moveWaypoint();
					}
				}
			}

			if(player.GetButtonDown("rightButton"))
			{
				if(screenNumber == 1)
				{
					if(waypoint != null)
					{
						Destroy(waypoint);
					}
				}
			}
		}

		else
		{
			pauseScreens[screenNumber].GetComponent<CanvasGroup>().alpha = 0;
            pauseMenu.GetComponent<CanvasGroup>().alpha = 0;
            mapCamera.gameObject.SetActive(false);
			onPause = false;
		}
	}

	void limitCamera()
	{
		Vector3 pos = mapCamera.transform.position;

		pos.x = Mathf.Clamp (mapCamera.transform.position.x, -42, 77.6f);
		pos.y = Mathf.Clamp (mapCamera.transform.position.y, -40, 37);

		mapCamera.transform.position = pos;
	}

	//void ZoomOrthoCamera(Vector3 zoomTowards, float amount)
	void ZoomOrthoCamera(float amount)
	{
		//Calculate how much we will have to move towards the zoomTowards position
		//		float multiplier = (1 / mapCamera.orthographicSize * amount);
		//
		//		// Move camera
		//		if(mapCamera.orthographicSize < 94.7f && mapCamera.orthographicSize > 14.3f)
		//		{
		//			mapCamera.transform.position += (zoomTowards - mapCamera.transform.position) * multiplier; 
		//		}
		
		// Zoom camera
		mapCamera.orthographicSize -= amount;
		
		// Limit zoom
		mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize, 3, 39.5f);
	}

	public void createMouseIcon()
	{
		//create cursor so it appears above slots
		cursor = (GameObject)Instantiate(cursorPrefab);
		RectTransform cursorRect = cursor.GetComponent<RectTransform>();
		//RectTransform inventoryRect;
		
		cursor.transform.SetParent(pauseScreens[screenNumber].transform);
		
		cursorRect.localPosition = new Vector3 (50, 100);
		//cursorRect.localPosition = new Vector3 (inventoryRect.localPosition.x, inventoryRect.localPosition.y + 125);

		//Inventory.Instance.playerCursor = cursor;
	}

	void createWaypoint()
	{
		waypoint = (GameObject)Instantiate(waypointPrefab);
		moveWaypoint ();
	}

	void moveWaypoint()
	{
        Vector2 centerPointWorld = mapCamera.ScreenToWorldPoint(centerPoint.transform.position);
		waypoint.transform.position = new Vector3(centerPointWorld.x, centerPointWorld.y, 0);
	}
}
