using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chargeAttack : MonoBehaviour
{
    bulkyBuddy buddyRef;
    List<GameObject> lastHit = new List<GameObject>();
    List<GameObject> lastHitToRemove = new List<GameObject>();
    bool canHitPlayer = false;

    void Awake()
    {
        buddyRef = this.GetComponentInParent<bulkyBuddy>();
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "playerTrigger")
        {
            if (buddyRef.charging)
            {
                PlayerScript player = other.GetComponentInParent<PlayerScript>();

                canHitPlayer = player.checkObject(this.gameObject);

                if (canHitPlayer)
                {
                    player.takeDamage(20);
                    //lastHit.Add(other.gameObject);
                    print("hitYou");
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "playerTrigger")
        {
            removeTrigger_fromList();
        }
    }

    public void removeTrigger_fromList()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("playerTrigger");

        foreach (GameObject obj in go)
        {
            if (obj.GetComponentInParent<PlayerScript>().objects.Count > 0)
            {
                obj.GetComponentInParent<PlayerScript>().removeObject(this.gameObject);
                print("asfdafas");
            }
        }


        // GameObject[] go = GameObject.FindGameObjectsWithTag("playerTrigger");


        /*For letting player be hit again after off collision (say if player teleports he can be ramed again. okay not like that sicko)*/
        //foreach (GameObject obj in lastHit)
        //{
        //    if (obj.GetComponentInParent<PlayerScript>().objects.Count > 0)
        //    {
        //        obj.GetComponentInParent<PlayerScript>().removeObject(this.gameObject);
        //        lastHitToRemove.Add(obj);
        //        print("asfdafas");
        //    }
        //}

        //foreach (GameObject toRemove in lastHitToRemove)
        //{
        //    lastHit.Remove(toRemove);
        //}

        //lastHitToRemove.Clear();

        //print("list count is: " + lastHit.Count);
    }
}
