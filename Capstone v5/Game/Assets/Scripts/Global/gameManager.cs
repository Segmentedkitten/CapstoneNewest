using UnityEngine;
using System.Collections;
using Rewired;

public class gameManager : MonoBehaviour 
{
	private bool paused = false;
    private bool _combat = false;
	
	private int levelOn;
    [HeaderAttribute("Debug/Testing")]
	public bool inCombat = false;
    public int numOfPlayers = 4;
    public bool usingKeyboard = false;
    [HideInInspector] 
    public static string pauseTag;
    public Player player;

    public int LevelOn
	{
		get{return levelOn;}
		set{ levelOn = value;}
	}

	public bool Paused
	{
		get {return paused;}
	}

	

	private static gameManager instance;
	public static gameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<gameManager>();
			}

			return gameManager.instance;
		}
	}

	// Update is called once per frame
	void Update () 
	{
        if (GameObject.Find("combatCanvas"))
        {
            if (inCombat)
            {
                //remove other canvases
                if (paused)
                {
                    GameObject.Find("combatCanvas").GetComponent<CanvasGroup>().alpha = 0;
                }

                else
                {
                    GameObject.Find("combatCanvas").GetComponent<CanvasGroup>().alpha = 1;
                }
            }
            else
            {
                //re-add other canvases
                GameObject.Find("combatCanvas").GetComponent<CanvasGroup>().alpha = 0;
            }
        }
	}

	public void pauseGame()
	{
		paused = !paused;
	}

	public void changeLevel(int levelTo)
	{
	
		levelOn = levelTo;
		Application.LoadLevel (levelTo);
	}
}
