using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class combatTrigger : MonoBehaviour {

    [HeaderAttribute("Encounter Setup")]
    /// <summary>
    /// How many enemies you want in the encounter
    /// Add to the size then drag the enemy prefabs in
    /// </summary>
    public GameObject[] enemiesInEncounter;
    public Vector2[] enemyLocations;
    public GameObject combatMan;


   [HideInInspector]    
   

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {		
		GameObject CombatManager;
        if(other.tag == "Player" && !gameManager.Instance.inCombat)
        {

            //This sets up the spawing of the enemies an starting combat
            gameManager.Instance.inCombat = true;
            CombatManager = (GameObject)Instantiate(combatMan, this.transform.position, Quaternion.identity);
            for(int i = 0; i <enemiesInEncounter.Length;i++)
            {
                enemiesInEncounter[i] = (GameObject)Instantiate(enemiesInEncounter[i].gameObject, new Vector2(transform.position.x + enemyLocations[i].x,transform.position.y + enemyLocations[i].y) , Quaternion.identity);
                enemiesInEncounter[i].transform.parent = CombatManager.transform; 
            }
            CombatManager.GetComponent<CombatManager>().initializeEnemyList();
			other.GetComponent<PlayerScript>().startCombat();
            Destroy(this.gameObject);

        }
        

    }
}
