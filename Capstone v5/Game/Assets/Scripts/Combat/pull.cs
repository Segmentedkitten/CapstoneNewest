using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pull : MonoBehaviour {

   
    float pullTime = 1.5f;
    bool pullHit = false;
   
  
    GameObject parentObj;
    GameObject enemyHit;
    

	// Use this for initialization
	void Start () {

        parentObj = transform.parent.gameObject;
        transform.parent.gameObject.GetComponent<Libra>().canFlip = false;
	
	}
	
	// Update is called once per frame
	void Update () {

       
        if(pullHit)
        {
            GetComponent<Rigidbody2D>().Sleep();

                
                Vector2 direction = enemyHit.transform.position - parentObj.transform.position;

                if (direction.magnitude > 1)
                {
                    transform.position = Vector2.Lerp(transform.position, parentObj.transform.position, Time.deltaTime * 3f);
                    enemyHit.transform.position = Vector2.Lerp(enemyHit.transform.position, parentObj.transform.position, Time.deltaTime * 3f);


                }
                else
                {
                    transform.parent.gameObject.GetComponent<Libra>().canFlip = true;
                    Destroy(gameObject);                  

                }       
           
            
        }
        else
        {
            if (pullTime > 0)
            {
                pullTime -= Time.deltaTime * 3;
            }

            else
            {
                transform.parent.gameObject.GetComponent<Libra>().canFlip = true;
                Destroy(gameObject);
               
            }
            if (transform.parent.gameObject.GetComponent<Libra>().facingRight)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * Time.deltaTime * 1750);
            }else if(!transform.parent.gameObject.GetComponent<Libra>().facingRight)
            {
                //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                GetComponent<Rigidbody2D>().AddForce(-Vector2.right * Time.deltaTime * 1750);
            }
                       
        }

       
     
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "enemyCollider")
        {
            enemyHit = other.gameObject.transform.parent.gameObject;
            pullHit = true;
            Destroy(GetComponent<BoxCollider2D>());
           
     
           

        }


    }
}
