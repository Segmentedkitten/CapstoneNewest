using UnityEngine;
using System.Collections;

public class meleeAttack : MonoBehaviour
{
  
    bool isAttacking = false;
    float _time = 0;
    float startTime = 0;
    float _damage = 0;

    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (isAttacking)
        {
            if (_time > 0)
            {
                _time--;

            }
            else
            {
                _time = startTime;
                isAttacking = false;
                gameObject.SetActive(false);
                transform.parent.GetComponent<PlayerScript>().meleeComplete();
                removeFromLists();
            }
        }

    }


    public void setAttack(float time, float damage)
    {
        //Gets this from player, starts melee up
        startTime = time;
        _time = time;
        _damage = damage;
        gameObject.SetActive(true);
        isAttacking = true;
    }

    public void removeFromLists()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("enemyObj");

        foreach (GameObject enemyObj in go)
        {
            if (enemyObj.GetComponent<enemyScript>().objects.Count > 0)
            {
                enemyObj.GetComponent<enemyScript>().removeObject(this.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemyCollider")
        {
            
            enemyScript enemy = other.GetComponentInParent<enemyScript>();
          
          
        
            if(enemy.checkObject(this.gameObject))
            {
                enemy.takeDamage(_damage);
            }
        }
    }
}