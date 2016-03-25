using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Rewired;

public class moveCursor : MonoBehaviour
{
    private Player player; // The Rewired Player
    public float speed = 400;
    float moveVelocityX;
    float moveVelocityY;

    GameObject myTarget;
    float moveHorizontal = 0;
    float moveVertical = 0;

    bool triggerleftPressed = false;
    bool triggerrightPressed = false;
    bool idSet = true;
    public bool slotChanged = false;

    GameObject pauseGameRef;

    void Start()
    {
        pauseGameRef = GameObject.Find("pauseControl");
    }

    private static moveCursor instance;

    public static moveCursor Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<moveCursor>();
            }

            return moveCursor.instance;
        }
    }

    public void setID(Player _player)
    {
        player = _player;
        idSet = true;
        print("hello?");
        print(idSet);
    }

    // Update is called once per frame
    void Update()
    {
        if (idSet)
        {
            print(idSet);
            if (gameManager.Instance.Paused)
            {
                if (pauseGameRef.GetComponent<pauseControl>().screenNumber == 0)
                {
                    handleMovement();
                    handleButtons();
                }

                else if (pauseGameRef.GetComponent<pauseControl>().screenNumber != 0)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                    if (Inventory.Instance.selectStackSize.activeSelf)
                    {
                        if (slotChanged)
                        {
                            resetSlot();
                        }

                        Inventory.Instance.selectStackSize.SetActive(false);
                    }
                }
            }

            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                idSet = false;

                if (Inventory.Instance.selectStackSize.activeSelf)
                {
                    if (slotChanged)
                    {
                        resetSlot();
                    }

                    Inventory.Instance.selectStackSize.SetActive(false);
                }
            }
        }

    }

    void handleMovement()
    {
        float moveHorizontal = player.GetAxis("moveHorizontal") * speed;
        float moveVertical = player.GetAxis("moveVertical") * speed;

        GetComponent<Rigidbody2D>().velocity = new Vector2(moveHorizontal, moveVertical);
    }

    //flip all necessary bools back after stack menu is closed
    public void resetSlot()
    {
        Inventory.CurrentSlot.GetComponent<Image>().sprite = myTarget.gameObject.GetComponent<Button>().spriteState.pressedSprite;
        Inventory.CurrentSlot = null;
        myTarget = null;
    }

    void handleButtons()
    {
        if (player.GetButtonDown("leftButton"))
        {
            if (Inventory.CurrentSlot != null)
            {
                if (!GameObject.Find("hover") && !Inventory.CurrentSlot.GetComponent<slot>().isEmpty && !Inventory.Instance.selectStackSize.activeSelf)
                {
                    //Inventory.Instance.selectStackSize.transform.position = this.transform.position;
                    Inventory.Instance.setStackInfo(Inventory.CurrentSlot.GetComponent<slot>().Items.Count);
                }

                else if (Inventory.Instance.selectStackSize.activeSelf)
                {
                    if (slotChanged)
                    {
                        resetSlot();
                    }

                    Inventory.Instance.selectStackSize.SetActive(false);
                }
            }
        }

        if (player.GetButtonDown("bottomButton"))
        {
            if (Inventory.CurrentSlot != null && !Inventory.Instance.selectStackSize.activeSelf)
            {
                Inventory.Instance.MoveItem();
            }
        }

        float l_triggerValue = player.GetAxis("leftTrigger");
        float r_triggerValue = player.GetAxis("rightTrigger");

        if (Inventory.Instance.selectStackSize.activeSelf)
        {
            if (l_triggerValue == 1)
            {
                if (!triggerleftPressed)
                {
                    Inventory.Instance.changeStackText(-1);
                    triggerleftPressed = true;
                }
            }

            else
            {
                triggerleftPressed = false;
            }

            if (r_triggerValue == 1)
            {
                if (!triggerrightPressed)
                {
                    Inventory.Instance.changeStackText(1);
                    triggerrightPressed = true;
                }
            }

            else
            {
                triggerrightPressed = false;
            }

            if (player.GetButtonDown("bottomButton"))
            {
                Inventory.Instance.SplitStack();

                if (slotChanged)
                {
                    resetSlot();
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (gameManager.Instance.Paused)
        {
            if (!Inventory.Instance.selectStackSize.activeSelf)
            {
                if (other.gameObject.tag == "invSlot" || other.gameObject.tag == "player1Item" || other.gameObject.tag == "player2Item"
                   || other.gameObject.tag == "player3Item" || other.gameObject.tag == "player4Item")
                {
                    other.gameObject.GetComponent<Image>().sprite = other.gameObject.GetComponent<Button>().spriteState.highlightedSprite;

                    Inventory.CurrentSlot = other.gameObject;
                    myTarget = other.gameObject;

                    print(Inventory.CurrentSlot);
                }
            }

            else
            {
                if (other.gameObject.tag == "invSlot")
                {
                    if (other.gameObject != Inventory.CurrentSlot)
                    {
                        slotChanged = true;
                    }

                    else
                    {
                        slotChanged = false;
                    }

                    print("slot changed:" + slotChanged);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (gameManager.Instance.Paused)
        {
            if (!Inventory.Instance.selectStackSize.activeSelf)
            {
                if (other.gameObject == myTarget)
                {
                    other.gameObject.GetComponent<Image>().sprite = myTarget.gameObject.GetComponent<Button>().spriteState.pressedSprite;

                    Inventory.CurrentSlot = null;
                    myTarget = null;

                    print(Inventory.CurrentSlot);
                }
            }

            else
            {
                if (other.gameObject.tag == "invSlot")
                {
                    slotChanged = true;
                    print("slot changed:" + slotChanged);
                }
            }
        }
    }
}