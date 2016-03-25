using UnityEngine;
using System.Collections.Generic;

public class spinScript : MonoBehaviour {
    List<GameObject> enemiesHit = new List<GameObject>();
     float spinTime = 3;
     float damage = 0;
     bool ifCheck = true;
	void Start () {
        damage = transform.parent.GetComponent<PlayerScript>().power * .02f;
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.GetChild(0).eulerAngles += new Vector3(0, 0, 20);
        print(enemiesHit.Count);
        if (spinTime > 0)
        {
            spinTime -= Time.deltaTime;
        }

        else
        {
            transform.parent.GetComponent<Leo>().spinning = false;
            ifCheck = false;

            foreach (GameObject e in enemiesHit)
            {
                
                
                e.GetComponent<enemyScript>().endContHit();
               
            }

            Destroy(this.gameObject);
        }
	
	}


     void OnTriggerStay2D(Collider2D other)
    {
        if (ifCheck)
        {
            if (other.gameObject.tag == "enemyCollider" )
            {
                if (!enemiesHit.Contains(other.transform.parent.gameObject))
                {
                    
                    GameObject es = other.transform.parent.gameObject;
                    es.GetComponent<enemyScript>().makeNewContText();
                    enemiesHit.Add(es);
                    
                  
                }
               
                other.transform.parent.GetComponent<enemyScript>().takeDamageCont(damage);

              
               
               
                
            }
        }
        
    }
}
