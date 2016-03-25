using UnityEngine;
using System.Collections;

public class LevelManager_1 : MonoBehaviour {

	GameObject[] players;

	// Use this for initialization
	void Start () {
	
		players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < gameManager.Instance.numOfPlayers; i++) {

            //players[i].GetComponent<PlayerScript>().initializePlayer();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
