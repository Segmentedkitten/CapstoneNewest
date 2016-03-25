using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


    GameObject player;
	// Use this for initialization
	void Start () {

        player = GameObject.Find("Player1");

	}
	
	// Update is called once per frame
	void Update () {


        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -6);
	
	}
}
