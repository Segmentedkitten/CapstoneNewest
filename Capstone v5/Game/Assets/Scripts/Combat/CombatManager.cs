using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {


	GameObject [] enemyList;
	GameObject [] playerList;
	public List<GameObject> Enemies = new List<GameObject>();
    bool enemyListInit = false;

    

	// Use this for initialization
	void Start () 
    {
	

	}

	public void initializeEnemyList() // Adds enemies spawned by trigger into the list
	{
        enemyList = GameObject.FindGameObjectsWithTag("enemyObj");
        

		foreach (GameObject enemy in enemyList) 
        {
			Enemies.Add(enemy);
		}
        enemyListInit = true;

        playerList = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<enemyScript>().setTarget(playerList[0].transform);
        }

	}



	void Awake()
	{		
		
	}

	
	// Update is called once per frame
	void Update ()
    {
        //Checks if all enemies are dead, then ends combat
        if (enemyListInit)
        {
            if (Enemies.Count <= 0)
            {
                foreach (GameObject player in playerList)
                {
                    player.GetComponent<PlayerScript>().endCombat();
                }

                gameManager.Instance.inCombat = false;
                Destroy(this.gameObject);
            }
            else
            {
                foreach(GameObject enemy in Enemies)
                {
                    enemy.GetComponent<enemyScript>().Update();
                }
            }
        }
	
	}
}
