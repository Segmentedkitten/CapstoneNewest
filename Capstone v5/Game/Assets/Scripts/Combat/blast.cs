using UnityEngine;
using System.Collections;

public class blast : MonoBehaviour {


	GameObject myPlayer;
    bool attackSet = false;
    Vector2 attackVect = Vector2.zero;
    float projLifetime = 2f;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (attackSet)
        {
            if (attackVect.x != 0 && attackVect.y != 0)
            {
                this.GetComponent<Rigidbody2D>().AddForce(attackVect * Time.deltaTime * 1750, ForceMode2D.Force);

            }
            else
            {

               this.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Time.deltaTime * 1750, ForceMode2D.Force);

            }
            if (projLifetime > 0)
            {
                projLifetime -= Time.deltaTime * 3;
            }

            else
            {
                projLifetime = 2f;
                Destroy(gameObject);
            }
        }
       
	}

    public void setAttack(Vector2 pos, Transform parent)
    {
        myPlayer = parent.gameObject;
        attackSet = true;
        attackVect = pos;

    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "enemyCollider")
		{
			enemyScript enemy = other.GetComponentInParent<enemyScript>();
            enemy.takeDamage(50);

			myPlayer.transform.GetChild(0).transform.position = this.transform.position;
			myPlayer.transform.GetChild(0).GetComponent<G_Golem>().isIdle = false;
			Destroy(this.gameObject);
           
        }

		
	}

	void OnTriggerExit2D(Collider2D other)
	{

	}
}
