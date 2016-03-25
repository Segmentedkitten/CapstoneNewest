using UnityEngine;
using System.Collections;

public class initCombat : MonoBehaviour {
    GameObject[] players;

	// Use this for initialization
	void Awake () {
        players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < gameManager.Instance.numOfPlayers; i++) {
            print(players[i]);
            players[i].GetComponent<PlayerScript>().initializePlayerCombat();
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
