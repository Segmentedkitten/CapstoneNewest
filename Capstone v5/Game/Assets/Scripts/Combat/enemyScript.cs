using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class enemyScript : MonoBehaviour 
{

	// List Initialization


    //Canvas Stuff
    Image health;
    float healthScale = 1;
    bool removeHealth = false;
    //end canvas
    [HeaderAttribute("Stat Settings")]
   //Stats add more if needed, make public to edit in Unity Editor
    public float hpAmount = 500;
    public float defense = 0; // Scale from 0 to 1, 1 being the highest 
    public float attack = 80;
    //End stats

    [HideInInspector]
    public List<GameObject> objects;
    public List<GameObject> objectsToRemove = new List<GameObject>();

    //Caching the transfom
    Transform myTransform;

    //Targets for decision making
    GameObject[] playerList;
    public Transform targetPlayer;

    public float moveSpeed = 1.0f;
    float rotationSpeed = 3.0f;

    Vector3 newRandomTarPosition;

    //Timer variables for deciding what to target
    public float timeSwitch = 20;
    float timeSwitchMax = 20;
    float timeSwitchDec = 5;

    //For rotation and movement  
    Vector3 vectorToTarget;
    float angle;
    float offset = 0.0f;
    Quaternion q;
    GameObject enemyCanvas;
    public GameObject damageTextPrefab;
    List<GameObject> damageNums = new List<GameObject>();


    //For "random" target choice
    public int choice;

    public bool contHit = false;
    bool coRoutRunning = false;
    float totalHitAmt = 0;
    //attacks
    bool canHitPlayer = false;
    GameObject contHitText;

	void Awake () 
    {
        //cache for easy access
        myTransform = transform;

        //targetPlayer = playerList[0].transform;
        CreateRandTarPoint();
        decideTarget();

        health = this.transform.GetChild(0).FindChild("health").GetComponent<Image>();
        enemyCanvas = this.transform.GetChild(0).gameObject;
        
	}
	
	// Update is called once per frame
	public virtual void Update () 
    {

        if (timeSwitch <= 0)
        {
            CreateRandTarPoint();
            decideTarget();
            timeSwitch = timeSwitchMax;
            moveSpeed = 1.0f;
        }

        //pathfinding around walls
        avoidWalls();        
        //targetRotatePlayer();
        decideAttack();

        //Move forward
       // myTransform.position += myTransform.right * moveSpeed * Time.deltaTime;

        //Decrease timer
        timeSwitch -= timeSwitchDec * Time.deltaTime;

        if(removeHealth)
        {
            if(health.fillAmount > healthScale)
            {
                health.fillAmount -= 1 * Time.deltaTime;
            }
            else
            {

                removeHealth = false;
            }

        }

        if(health.fillAmount <= 0)
        {
            die();
        }
        CBTManager();
	
	}

    public void die()
    {
        targeting[] t = FindObjectsOfType(typeof(targeting)) as targeting[];

        foreach (targeting target in t)
        {
            target.GetComponent<targeting>().removeTarget(this.gameObject.transform);
        }
        transform.parent.GetComponent<CombatManager>().Enemies.Remove(this.gameObject);
        Destroy(this.gameObject);

    }

    //Just checking to see of the projectile/AoE has already hit the enemy this frame
    public bool checkObject(GameObject currentObject)
    {
        foreach (GameObject newObject in objects)
        {
            if (currentObject == newObject)
            {
                return false;
            }
        }

        objects.Add(currentObject);
        return true;
    }


    //Removing from list to start for next frame
    public void removeObject(GameObject currentObject)
    {
        foreach (GameObject Objs in objects)
        {
            if (currentObject == Objs)
            {
                objectsToRemove.Add(currentObject);
            }
        }

        foreach (GameObject toRemove in objectsToRemove)
        {
            objects.Remove(toRemove);
        }

        objectsToRemove.Clear();

    }


    public virtual void takeDamage(float amount)
    {
        amount = amount * (1 - (defense));
        float percentRemove;
        percentRemove = amount / hpAmount;
        healthScale -= percentRemove;
        removeHealth = true;
        GameObject dText = Instantiate(damageTextPrefab) as GameObject;

        dText.transform.SetParent(enemyCanvas.transform);
        dText.transform.localScale = enemyCanvas.transform.localScale;
        dText.transform.localScale = this.transform.localScale * .5f;
        dText.transform.localPosition = new Vector2(1, 0);
        StartCoroutine("flashRed");
        if (!contHit)
        {
            dText.GetComponent<Text>().text = (amount.ToString());
            dText.GetComponent<Animator>().SetTrigger("Hit");


        }
        else if (contHit)
        {
            totalHitAmt += amount;
            print(totalHitAmt);

        }


    }

    public virtual void takeDamageCont(float amount)
    {

        amount = amount * (1 - (defense));
        float percentRemove;
        percentRemove = amount / hpAmount;
        healthScale -= percentRemove;
        removeHealth = true;
       
         totalHitAmt += amount;
        if(!coRoutRunning)
        {
            StartCoroutine("displayTickDmg");
        }
    }
    public void makeNewContText()
    {
       contHitText = Instantiate(damageTextPrefab) as GameObject;
       contHitText.transform.SetParent(enemyCanvas.transform);
       contHitText.transform.localScale = enemyCanvas.transform.localScale;
       contHitText.transform.localScale = this.transform.localScale * .5f;
       contHitText.transform.localPosition = new Vector2(1, 0);
    }
    IEnumerator displayTickDmg()
    {
        coRoutRunning = true;
        contHitText.GetComponent<Text>().text = (Mathf.Round(totalHitAmt).ToString());
        yield return new WaitForSeconds(.4f);
       
        coRoutRunning = false;



    }

    public void endContHit() 
    {
        StopCoroutine("displayTickDmg");
        coRoutRunning = false;
        Destroy(contHitText);
        GameObject dText = Instantiate(damageTextPrefab) as GameObject;
        dText.transform.SetParent(enemyCanvas.transform);
        dText.transform.localScale = enemyCanvas.transform.localScale;
        dText.transform.localScale = this.transform.localScale * .5f;
        dText.transform.localPosition = new Vector2(1, 0);
        dText.GetComponent<Text>().text = (Mathf.Round(totalHitAmt).ToString());
        dText.GetComponent<Animator>().SetTrigger("Hit");
        totalHitAmt = 0;

    
    }



    // // // Add AI stuffs here // // //




    // // // -----------------// // //

    public void setTarget(Transform target)
    {
        targetPlayer = target;
    }

    IEnumerator flashRed()
    {
        Color myColor = this.GetComponent<SpriteRenderer>().color;
        Color otherColor = new Color(255, 0, 0);
          
        this.GetComponent<SpriteRenderer>().color = otherColor;
        yield return new WaitForSeconds(.2f);
        this.GetComponent<SpriteRenderer>().color = myColor;
 
    } 

    public void CBTManager()
    {
        for (int i = 0; i < damageNums.Count; i++)
        {

          //  if (damageNums[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !damageNums[i].GetComponent<Animator>().IsInTransition(0))
           // {
           //     Destroy(damageNums[i].gameObject);
            //    damageNums.RemoveAt(i);
                
           // }
        }
        //Destroy the nums

    }

    public virtual void OnTriggerEnter2D(Collider2D other)
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

    public virtual void OnTriggerExit2D(Collider2D other)
    {


    }

    void decideTargetPlayer()
    {
        int tIndex = Random.Range(0, 4);

        while (playerList.Length < tIndex)
        {
            tIndex = Random.Range(0, 4);
        }

        targetPlayer = playerList[tIndex].transform;
    }

    void decideTarget()
    {
        choice = Random.Range(1, 4);
    }

    //create a random target point
    //min and max tars are arbitrarely set
    void CreateRandTarPoint()
    {
        //Random movement variables
        float minTarX = -10.0f;
        float maxTarX = 10.0f;
        float minTarY = -10.0f;
        float maxTarY = 10.0f;
        float tarX;
        float tarY;

        tarX = Random.Range(minTarX, maxTarX);
        tarY = Random.Range(minTarY, maxTarY);

        newRandomTarPosition = new Vector3(tarX, tarY, 0);
    }

    //Rotate towards chosen player
    void targetRotatePlayer()
    {
        vectorToTarget = targetPlayer.position - myTransform.position;
        angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + offset;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, q, Time.deltaTime * rotationSpeed);
    }

    //Rotate towards chosen random spot
    void targetRotateRand()
    {
        vectorToTarget = newRandomTarPosition - myTransform.position;
        angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + offset;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, q, Time.deltaTime * rotationSpeed);
    }

    void avoidWalls()
    {
        //Pathfinding
        RaycastHit2D rayHit;
        float maxDistance = 1.0f;
        RaycastHit2D[] allHits;
        float changeOffset = 2.0f;
        float dist = 0.0f;
        float distPrev = 0.0f;
        float xSq;
        float ySq;

        xSq = (targetPlayer.position.x - myTransform.position.x) * (targetPlayer.position.x - myTransform.position.x);
        ySq = (targetPlayer.position.y - myTransform.position.y) * (targetPlayer.position.y - myTransform.position.y);

        dist = Mathf.Sqrt(xSq + ySq);

        //Debug.DrawRay(myTransform.position, myTransform.right, Color.green, 5.0f, false);
        //Get an array of everything hit by the raycast
        allHits = Physics2D.RaycastAll(myTransform.position, myTransform.right, maxDistance);

        //The raycast always hits its parent collider fist, so allHits is never null
        if (allHits.GetLength(0) > 1)
        {
            //set rayHit to second collider hit
            rayHit = allHits[1];

            //If near a wall
            if (rayHit.collider.tag == "Wall")
            {
                //Add offset to the angle
                offset += changeOffset;
            }
            else
            {
                //Else reset the offset
                offset = 0.0f;
            }
        }
        //If ray is not hitting anything
        //And there is an offset, slowly decrease it
        else if (offset > 0)
        {
            offset -= 2.0f;
        }
        //else ensure the offset doesn't fall below 0
        else offset = 0.0f;

        distPrev = dist;
    }

    void decideAttack()
    {

    }


}
