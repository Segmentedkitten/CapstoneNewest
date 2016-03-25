using UnityEngine;
using System.Collections;

public class LevelManager_2 : MonoBehaviour {

    GameObject[] players;
	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < gameManager.Instance.numOfPlayers; i++)
        {

            players[i].transform.position = new Vector3(0,0,0);
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
