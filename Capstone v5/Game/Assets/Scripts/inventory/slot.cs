using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class slot : MonoBehaviour, IPointerClickHandler 
{
	private Stack<item> items;
	public Text stackText;
	public Sprite slotEmpty;
	public Sprite slotHighlight;

	public Stack<item> Items
	{
		get {return items;}
		set{items = value;}
	}

	public bool isEmpty
	{
		get {return items.Count == 0;}
	}

	public bool canStack
	{
		//as long the current item's max size is greater than its current size,
		//can stack is true
		get {return currentItem.maxSize > items.Count;}
	}

	public item currentItem
	{
		get {return items.Peek();}
	}

	//a stack of how many items need to be removed
	public Stack<item> RemoveItems(int amount)
	{
		Stack<item> temp = new Stack<item> ();

		for(int i = 0; i < amount; i++)
		{
			//wtf is this witchcraft harry?

			//items we are adding to the temp are the ones being removed
			//from the item stack
			temp.Push(items.Pop());
		}

		if (items.Count > 1)
		{
			stackText.text = items.Count.ToString();
		}

		else
		{
			stackText.text = string.Empty;
		}

		return temp;
	}

	//removing one item from stack in hand(to see if it can be merged with an inventory item)
	public item RemoveItem()
	{
		item temp;

		temp = items.Pop ();

		if (items.Count > 1)
		{
			stackText.text = items.Count.ToString();
		}
		
		else
		{
			stackText.text = string.Empty;
		}

		return temp;
	}

	void Awake()
	{
		items = new Stack<item> ();
	}

	// Use this for initialization
	void Start () 
	{
		RectTransform slotRect = GetComponent<RectTransform> ();
		RectTransform textRect = stackText.GetComponent<RectTransform> ();

		int textScaleFactor = (int)(slotRect.sizeDelta.x * 0.60f);

		stackText.resizeTextMaxSize = textScaleFactor;
		stackText.resizeTextMinSize = textScaleFactor;

		textRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
		textRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void addItem(item _item)
	{
		items.Push (_item);

		if(items.Count >= 2)
		{
			stackText.text = items.Count.ToString();
		}

		changeSprite (_item.spriteNeutral, _item.spriteHighlighted);
	}

	public void addItems(Stack<item> items)
	{
		this.items = new Stack<item> (items);

		if(items.Count > 1)
		{
			stackText.text = items.Count.ToString();
		}
		
		else
		{
			stackText.text = string.Empty;
		}

		changeSprite (currentItem.spriteNeutral, currentItem.spriteHighlighted);
	}

	private void changeSprite(Sprite neautral, Sprite highlight)
	{
		if(!Inventory.Instance.movingSlot_added)
		{
			GetComponent<Image> ().sprite = neautral;
		}
		
		else
		{
			
		}

		SpriteState st = new SpriteState ();
		st.highlightedSprite = highlight;
		st.pressedSprite = neautral;

		GetComponent<Button> ().spriteState = st;
	}

	private void useItem()
	{
		if(!isEmpty)
		{
			items.Pop ().use();

			if(items.Count > 1)
			{
				stackText.text = items.Count.ToString();
			}

			else
			{
				stackText.text = string.Empty;
			}

			if(isEmpty)
			{
				changeSprite(slotEmpty, slotHighlight);
				Inventory.EmptySlots++;
			}
		}
	}

	public void clearSlot()
	{
		items.Clear ();
		changeSprite (slotEmpty, slotHighlight);
		stackText.text = string.Empty;
	}

	//for using item on second mouse button press (right clicking)
	public void OnPointerClick(PointerEventData eventData)
	{
		if(gameManager.Instance.Paused)
		{														         //Inventory.from == null also works and may be better
			if(eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("hover"))
			{
				useItem ();
			}
		}
	}
}
