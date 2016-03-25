using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour 
{
	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;

	public int slots;
	public int rows;
	public float slotPaddingLeft;
	public float slotPaddingTop;
	public float slotSize;
	public GameObject slotPrefab;

	public static slot from, to = null;

	private List<GameObject> allSlots;
	private static int emptySlots;

	public GameObject iconPrefab;
	private static GameObject hoverObject;

	//offset for hover to fix placing problem with hover
	private float hoverYOffset;

	//reference to event system for deleting items
	//NOTE: will change how items are removed later
	//public EventSystem eventSystem;

	private static GameObject currentSlot;
	
	public List<playerController> players;

	//reference to canvas
	Canvas canvas;

	public static GameObject CurrentSlot 
	{
		get {return currentSlot;}
		set{currentSlot = value;}
	}

	public static int EmptySlots 
	{
		get {return emptySlots;}
		set{emptySlots = value;}
	}

	/* for splitting stack functionality*/
	public GameObject selectStackSize;
	public Text stackText;

	private int splitAmount;
	private int maxStackCount;

	//for storing items before they are placed
	private static slot movingSlot;

	public static slot MovingSlot 
	{
		get {return movingSlot;}
		set{movingSlot = value;}
	}

	private slot split_from = null;
	
	//singleton design pattern. 
	//Used so all classes have access to the Inventory class without needing an instance of it

	private static Inventory instance;

	public static Inventory Instance
	{
		get 
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<Inventory>();
			}

			return Inventory.instance;
		}
	}

	//Prototype objects for loading
	public GameObject mana;
	public GameObject health;
	public GameObject speed_boost;

	//for tooltip stuff
	public GameObject toolTip;
	public Text sizeText;
	public Text visualText;

	public bool movingSlot_added = false;

	public List<GameObject> playerSlots;

    GameObject pauseGameRef;

    // Use this for initialization
    void Start () 
	{
		movingSlot = GameObject.Find ("movingSlot").GetComponent<slot> ();
		selectStackSize.SetActive (false);
		canvas = GameObject.Find ("inventoryCanvas").GetComponent<Canvas>();
		//currentSlot = new GameObject ();
		createLayout ();
        pauseGameRef = GameObject.Find("pauseControl");
    }
	
	// Update is called once per frame
	void Update () 
	{
		//checks if mouse isnt over game object and left mouse button is released
		// and that object is in being held

		if(gameManager.Instance.Paused)
		{

            if (pauseGameRef.GetComponent<pauseControl>().screenNumber != 0)
            {
                if (!selectStackSize.activeSelf)
                {
                    putItemBack();
                }
            }

            if (currentSlot != null && !currentSlot.GetComponent<slot>().isEmpty && hoverObject == null)
			{
				showToolTip();
			}

			else 
			{
				hideToolTip();
			}
            
			if(hoverObject != null)
			{
                hoverObject.transform.position = new Vector2(pauseGameRef.GetComponent<pauseControl>().cursor.transform.position.x, pauseGameRef.GetComponent<pauseControl>().cursor.transform.position.y);
            }
		}

		else
		{
            if (!selectStackSize.activeSelf)
            {
                putItemBack();
            }
        }
	}

	private void putItemBack()
	{
		if(from != null)
		{
			Destroy(GameObject.Find ("hover"));
			from.GetComponent<Image>().color = Color.white;
			from = null;
		}

		else if (!movingSlot.isEmpty)
		{
			Destroy(GameObject.Find ("hover"));

			foreach(item _item in movingSlot.Items)
			{
				split_from.addItem(_item);
			}

			movingSlot.clearSlot();
		}

		//selectStackSize.SetActive (false);
	}

	public void showToolTip()
	{
		if(!selectStackSize.activeSelf)
		{
			visualText.text = currentSlot.GetComponent<slot> ().currentItem.getToolTip ();
			sizeText.text = visualText.text;
			
			toolTip.SetActive (true);

			float xPos = currentSlot.transform.GetChild(1).position.x;
			float yPos = currentSlot.transform.GetChild(1).position.y - slotPaddingTop; 
			
			toolTip.transform.position = new Vector2(xPos,yPos);
		}
	}

	public void hideToolTip()
	{
		toolTip.SetActive (false);
	}

	public void saveInventory()
	{
		string invContent = string.Empty;

		for(int i = 0; i < allSlots.Count; i++)
		{
			slot temp = allSlots[i].GetComponent<slot>();

			//you only need to save slots that aren't empty...
			//DUH! :p
			if(!temp.isEmpty)
			{
				invContent += i + "-" + temp.currentItem.type.ToString() + "-" + temp.Items.Count.ToString() + "-" + "main" + ";"; 
			}
		}

		for(int i = 0; i < playerSlots.Count; i++)
		{
			slot temp = playerSlots[i].GetComponent<slot>();
			
			//you only need to save slots that aren't empty...
			//DUH! :p
			if(!temp.isEmpty)
			{
				invContent += i + "-" + temp.currentItem.type.ToString() + "-" + temp.Items.Count.ToString() + "-" + "player" + ";"; 
			}
		}

		//uses playerprefs (built into unity) to save data, rather than using an xml
		PlayerPrefs.SetString ("invContent", invContent);
		PlayerPrefs.SetInt ("slots", slots);
		PlayerPrefs.SetInt ("rows", rows);
		PlayerPrefs.SetFloat("slotPaddingLeft", slotPaddingLeft);
		PlayerPrefs.SetFloat("slotPaddingTop", slotPaddingTop);
		PlayerPrefs.SetFloat("slotSize", slotSize);

		PlayerPrefs.Save ();
	}

	public void loadInventory ()
	{
		string invContent;

		if(PlayerPrefs.GetString("invContent") != null)
		{
			invContent = PlayerPrefs.GetString("invContent");
		}

		else
		{
			invContent = string.Empty;
		}

		slots = PlayerPrefs.GetInt ("slots");
		rows = PlayerPrefs.GetInt("rows");
		slotPaddingLeft = PlayerPrefs.GetFloat ("slotPaddingLeft");
		slotPaddingTop = PlayerPrefs.GetFloat ("slotPaddingTop");
		slotSize = PlayerPrefs.GetFloat ("slotSize");

		if(playerSlots != null)
		{
			foreach(GameObject gObject in playerSlots)
			{
				gObject.GetComponent<slot>().Items.Clear();
			}
		}

		createLayout ();

		//splitting content string

		if(invContent != string.Empty)
		{
			string[] splitContent = invContent.Split (';');

			//so it check the character before the semicolon
			//to prevent checking of empty space after semicolon
			for(int i = 0; i < splitContent.Length - 1; i++)
			{
				string[] splitValues = splitContent[i].Split('-');
				int index = Int32.Parse(splitValues[0]);
				ItemType type = (ItemType)Enum.Parse(typeof(ItemType),splitValues[1]);
				int amount = Int32.Parse(splitValues[2]);
				string identifier = splitValues[3];
				print ("blah");
				print ("amount:" + amount);

				List<GameObject> temps = new List<GameObject>();

				if(identifier == "main")
				{
					temps = allSlots;
				}

				else if(identifier == "player")
				{
					temps = playerSlots;
				}

				//places items based on their counts
				for(int j = 0; j < amount; j++)
				{
					print ("blah");
					switch(type)
					{
						case ItemType.MANA:
							temps[index].GetComponent<slot>().addItem(mana.GetComponent<item>());
							break;
						case ItemType.HEALTH:
							temps[index].GetComponent<slot>().addItem(health.GetComponent<item>());
							break;
						case ItemType.SPEED_BOOST:
							temps[index].GetComponent<slot>().addItem(speed_boost.GetComponent<item>());
							break;
					}
				}
			}
		}
	}

	private void createLayout()
	{
		//remove slots before creating if they already exist
		if(allSlots != null)
		{
			foreach(GameObject gObject in allSlots)
			{
				Destroy(gObject);
			}
		}

		allSlots = new List<GameObject>();

		//hover size set to 5 percent of the slot size
		hoverYOffset = slotSize * .05f;

		emptySlots = slots;

		inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

		inventoryRect = GetComponent<RectTransform> ();

		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight);

		int columns = slots / rows;

		//creating main inventory
		for (int y = 0; y < rows; y++)
		{
			for(int x = 0; x < columns; x++)
			{
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();
				BoxCollider2D slotCollision = newSlot.GetComponent<BoxCollider2D>();

				newSlot.name = "Slot";

				//ensures that canvas is parent of instantiated object
				newSlot.transform.SetParent(this.transform.parent);

				slotRect.localPosition = new Vector3(inventoryRect.localPosition.x - inventoryRect.sizeDelta.x/2,inventoryRect.localPosition.y + inventoryRect.sizeDelta.y/2) + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);

				//setting up slot collision box
				slotCollision.size = slotRect.sizeDelta;
				slotCollision.offset =  new Vector3(slotRect.sizeDelta.x / 2, (slotRect.sizeDelta.y / 2) * -1);

				//sets parent to inventory inside of canvas
				//allows for split interface things to appear in front of slots
				newSlot.transform.SetParent(this.transform);

				//move points to corner of slot
				newSlot.transform.GetChild(1).position = new Vector2(newSlot.transform.position.x, newSlot.transform.position.y - slotRect.sizeDelta.y);
				
				//stackbox point
				newSlot.transform.GetChild(2).position = new Vector2(newSlot.transform.position.x + (slotRect.sizeDelta.x / 2), newSlot.transform.position.y - slotRect.sizeDelta.y);

				allSlots.Add (newSlot);
			}
		}
	}

	public bool AddItem(item _item)
	{
		if(_item.maxSize == 1)
		{
			placeOnEmpty(_item);
			return true;
		}

		else
		{
			foreach(GameObject slot in allSlots)
			{
				slot temp = slot.GetComponent<slot>();

				if(!temp.isEmpty)
				{
					if(temp.currentItem.type == _item.type && temp.canStack)
					{
						//adds to different slot when split slot is held and items are being picked up simultaneously
						if(!movingSlot.isEmpty && currentSlot.GetComponent<slot>() == temp.GetComponent<slot>())
						{
							continue;
						}

						else
						{
							temp.addItem(_item);
							return true;
						}
					}
				}
			}

			if(emptySlots > 0)
			{
				placeOnEmpty(_item);
			}
		}

		return false;
	}

	private bool placeOnEmpty(item _item)
	{
		if(emptySlots > 0)
		{
			foreach(GameObject slot in allSlots)
			{
				slot temp = slot.GetComponent<slot>();

				if(temp.isEmpty)
				{
					temp.addItem(_item);
					emptySlots--;

					//no neeed to look for empty slot more than once
					return true;
				}
			}
		}

		return false;
	}

	//public void MoveItem(GameObject clicked)
	//{
	public void MoveItem()
	{
		if(gameManager.Instance.Paused)
		{
			//if we're moving split item(s)
			if(!movingSlot.isEmpty && currentSlot != null)
			{
				slot temp = currentSlot.GetComponent<slot>();

				//if the slot you are placing on is empty,
				//take moving slot items(which is the number of items you split off)
				//and add those to current slot
				if(temp.isEmpty)
				{
					temp.addItems(movingSlot.Items);
					movingSlot.Items.Clear();
					Destroy(GameObject.Find ("hover"));
				}

				//if the slot isnt empty, but items are same and can be merged

				else if(!temp.isEmpty && movingSlot.currentItem.type == temp.currentItem.type && temp.canStack)
				{
					print ("movingSlot: " + movingSlot);
					mergeStacks(movingSlot, temp);
				}
			}

			else if(from == null)
			{
				//if(!clicked.GetComponent<slot>().isEmpty)
				//{
				if(!currentSlot.GetComponent<slot>().isEmpty && !GameObject.Find("hover"))
				{
					//from = clicked.GetComponent<slot>();
					from = currentSlot.GetComponent<slot>();
				

					//gives occupied effect
					from.GetComponent<Image>().color = Color.gray;

					createHoverIcon();
				}
			}

			else if (to == null)
			{
				if(GameObject.Find("hover"))
				{
					to = currentSlot.GetComponent<slot>();

				
					Destroy(GameObject.Find ("hover"));
				}
			}

			if(from != null && to != null)
			{
				if(!to.isEmpty && from.currentItem.type == to.currentItem.type && to.canStack)
				{
					mergeStacks(from, to);
				}

				else
				{
					Stack<item> tempTo = new Stack<item>(to.Items);
					to.addItems(from.Items);

					if(tempTo.Count == 0)
					{
						from.clearSlot();
					}

					else
					{
						from.addItems(tempTo);
					}
				}

				if (from.tag == "invSlot" && to.tag != "invSlot") 
				{
					emptySlots++;
				}

				//setStats();

				//gets rid of occupied effect because it's no longer being occupied
				to.GetComponent<Image>().sprite = to.GetComponent<Button>().spriteState.highlightedSprite;
				from.GetComponent<Image>().color = Color.white;
				//sets to null when object is placed
				to = null;
				from = null;
				Destroy(GameObject.Find("hover"));
			}
		}
	}

	/*private void setStats()
	{
		if(from.tag == "player1Item")
		{
			players[0].de_equipItem();
		}
		
		else if(from.tag == "player2Item")
		{
			players[1].de_equipItem();
		}
		
		else if(from.tag == "player3Item")
		{
			players[2].de_equipItem();
		}
		
		else if(from.tag == "player4Item")
		{
			players[3].de_equipItem();
		}
		
		if(to.tag == "player1Item")
		{
			players[0].equipItem(to.currentItem);
		}
		
		else if(to.tag == "player2Item")
		{
			players[1].equipItem(to.currentItem);
		}
		
		else if(to.tag == "player3Item")
		{
			players[2].equipItem(to.currentItem);
		}
		
		else if(to.tag == "player4Item")
		{
			players[3].equipItem(to.currentItem);
		}
	}*/

	private void createHoverIcon()
	{
		hoverObject = (GameObject)Instantiate(iconPrefab);
		//hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
		hoverObject.GetComponent<Image>().sprite = currentSlot.GetComponent<Image>().sprite;
		hoverObject.name = "hover";
		
		RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
		RectTransform clickedTransform = currentSlot.GetComponent<RectTransform>();
		
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
		
		//sets hover transform to the child of canvas so it shows up on the canvas
		//have to do set parent this way because you are dealing with the inventory prefab
		hoverObject.transform.SetParent(GameObject.Find("inventoryCanvas").transform, true);
		
		//to ensure everything scales correctly
		hoverObject.transform.localScale = currentSlot.gameObject.transform.localScale;

		//used for split object's text
		if(!movingSlot.isEmpty)
		{
			if(movingSlot.Items.Count > 1)
			{
				//gets first child, which is the stack text
				hoverObject.transform.GetChild (0).GetComponent<Text> ().text = movingSlot.Items.Count.ToString();
			}

			else
			{
				hoverObject.transform.GetChild (0).GetComponent<Text> ().text = string.Empty;
			}
		}

		//used for a stack of object's text
		else
		{
			if(currentSlot.GetComponent<slot>().Items.Count > 1)
			{
				//gets first child, which is the stack text
				hoverObject.transform.GetChild (0).GetComponent<Text> ().text = currentSlot.GetComponent<slot>().Items.Count.ToString();
			}
			
			else
			{
				hoverObject.transform.GetChild (0).GetComponent<Text> ().text = string.Empty;
			}
		}
	}

	//function to check item selected and set stack info accordingly
	public void setStackInfo(int _maxStackCount)
	{
		selectStackSize.SetActive (true);
		hideToolTip ();

		float xPos = currentSlot.transform.GetChild(2).position.x;
		float yPos = currentSlot.transform.GetChild(2).position.y + 8;

		selectStackSize.transform.position = new Vector2(xPos,yPos);

		splitAmount = 0;
		maxStackCount = _maxStackCount;
		stackText.text = splitAmount.ToString ();
	}

	//called when okay button is clicked
	public void SplitStack()
	{
		selectStackSize.SetActive (false);

		//if split amount is equal to all of the items in a slot (3 for example)
		//its handled the same way as before by moving the whole stack (3)

		if(currentSlot != null)
		{
			if(splitAmount == maxStackCount)
			{
				//no need to split, just move stack
				MoveItem();
			}

			//if split amount isnt the stackCount and isnt zero, remove items in slot by splitAmount
			else if (splitAmount > 0)
			{
				split_from = currentSlot.GetComponent<slot>();
				movingSlot.Items = currentSlot.GetComponent<slot>().RemoveItems(splitAmount);
				createHoverIcon();
			}
		}
	}

	public void mergeStacks(slot source, slot destination)
	{
		//calculates how many items you can merge with the existing slot
		//for example: if there are 2 objects in the slot, and you can fit 3 health potions together, you can put 1 more
		int max = destination.currentItem.maxSize - destination.Items.Count;
		int count;


		//if you can fit 2 more, and you only have 1 on hand, set count to 1
		if(source.Items.Count < max)
		{
			count = source.Items.Count;
			
		}

		//if you 2 more you can fit, but have 3 on hand, only allow 2 to be transferred
		else
		{
			count = max;
		}

		movingSlot_added = true;
		
		for(int i = 0; i < count; i++)
		{
			destination.addItem(source.RemoveItem());
			hoverObject.transform.GetChild(0).GetComponent<Text>().text = movingSlot.Items.Count.ToString();
		}
		
		movingSlot_added = false;

		if(source.Items.Count == 0)
		{
			source.clearSlot();
			Destroy (GameObject.Find ("hover"));
		}
	}

	public void changeStackText(int i)
	{
		splitAmount += i;

		if(splitAmount < 0)
		{
			splitAmount = 0;
		}

		if(splitAmount > maxStackCount)
		{
			splitAmount = maxStackCount;
		}

		stackText.text = splitAmount.ToString ();
	}
}
