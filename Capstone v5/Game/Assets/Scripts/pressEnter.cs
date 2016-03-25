using UnityEngine;
using System.Collections;


public class pressEnter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	
	}
}
