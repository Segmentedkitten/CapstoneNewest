using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Rewired;

public class interactText : MonoBehaviour {

    [HeaderAttribute("Dialog")]
    public string[] dialogStrings;
    
    [HideInInspector]
    public bool typingText = false;
    public int dialogCount = 0;

    bool f_pressed = false;
    string currentText;

    CanvasGroup cGroup;
    GameObject canvas;
    GameObject playerColliding;
    bool playerNear = false;


    void Start()
    {
        
        cGroup = GameObject.Find("TextCanvas").GetComponent<CanvasGroup>();
        canvas = GameObject.Find("TextCanvas").gameObject;
        cGroup.alpha = 0;
       

    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear)
        {
            if (playerColliding.GetComponent<PlayerScript>().Player.GetButtonDown("bottomButton"))
            {
                playerColliding.GetComponent<PlayerScript>().topAnimator.SetBool("interact", true);
                if (typingText && f_pressed)
                {
                    canvas.transform.GetChild(1).GetComponent<Text>().text = currentText;
                    StopAllCoroutines();
                    typingText = false;
                    f_pressed = false;
                }
                else
                {
                    if (dialogCount <= dialogStrings.Length - 1)
                    {
                        StartCoroutine(typeText(dialogStrings[dialogCount]));
                        dialogCount++;
                    }
                    else
                    {

                        StartCoroutine(typeText(dialogStrings[dialogStrings.Length - 1]));
                    }



                    if (cGroup.alpha == 0)
                    {
                        cGroup.alpha = 1;
                    }

                    else if (cGroup.alpha == 1 && !typingText)
                    {
                        cGroup.alpha = 0;
                    }
                    f_pressed = true;
                }
            }
            
        }
        else
        {
            
        }
    }


    IEnumerator typeText(string text)
    {
        typingText = true;
        currentText = text;
        canvas.transform.GetChild(1).GetComponent<Text>().text = "";


        foreach (char letter in text.ToCharArray())
        {
            canvas.transform.GetChild(1).GetComponent<Text>().text += letter;
            yield return new WaitForSeconds(0.04f);
        }

        typingText = false;
    }


    public void startTyping()
    {


    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Player")
        {
           if(!playerNear)
           {
             playerColliding = col.gameObject;
           }
          
            transform.GetChild(1).gameObject.SetActive(true);
            playerNear = true;
            
            col.GetComponent<PlayerScript>().collidingWithItem = true;

        }


    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            playerColliding.GetComponent<PlayerScript>().topAnimator.SetBool("interact", false);
            playerColliding = null;
            transform.GetChild(1).gameObject.SetActive(false);
            playerNear = false;
            
            col.GetComponent<PlayerScript>().collidingWithItem = false;
        }

        cGroup.alpha = 0;

    }
}
