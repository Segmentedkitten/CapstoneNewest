using UnityEngine;
using System.Collections;

public class enemyMelee : MonoBehaviour
{
    bool canHitPlayer = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void removeFromLists()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in go)
        {
            if (obj.GetComponent<PlayerScript>().objects.Count > 0)
            {   
                obj.GetComponent<PlayerScript>().removeObject(this.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerScript player = other.GetComponentInParent<PlayerScript>();

            canHitPlayer = player.checkObject(this.gameObject);

            if (canHitPlayer)
            {
                player.takeDamage(10);
            }

            print("hello");
        }
    }
}

