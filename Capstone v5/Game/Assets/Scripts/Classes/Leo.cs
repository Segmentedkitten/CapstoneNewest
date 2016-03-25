using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System.Collections;

public class Leo : PlayerScript
{
    public GameObject circlePrefab;
    public GameObject spinPrefab;
    GameObject spinObject;
    GameObject circleObject;
    bool aimingCircle = false;
    bool canLeap = false;
    Vector2 startPos;

    public GameObject leap_aoePrefab;
    GameObject leap_aoeObject;

    public GameObject healingCirclePrefab;
    GameObject healingCircleObject;
    float healingTime = 0;

    public bool hitSomething = false;

    float melee_waitTime = 0;
    GameObject meleeBox;
    float strength_increaseTime = 0;

    public bool spinning = false;
    float spinTime = 0;
    bool delayAttack = false;
    float attackDelay = .75f;

    protected override void Awake()
    {
        base.Awake();
        meleeBox = this.transform.FindChild("meleeObject").gameObject;
      

    }

    protected override void passive()
    {
        if (hitSomething)
        {
            heal(50);
            hitSomething = false;
        }
    }

    protected override void updateControls()
    {
        base.updateControls();
        float rsHorizontal = this.Player.GetAxis("moveHorizontalR");
        float rsVertical = this.Player.GetAxis("moveVerticalR");

        if (gameManager.Instance.inCombat)
        {
            if (circleObject)
            {
                circleObject.transform.Translate(new Vector3(rsHorizontal * .2f, rsVertical * .2f, 0));
            }

            if (canLeap)
            {
                if ((Vector3.Distance(this.transform.position, circleObject.transform.position) > .1f))
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, circleObject.transform.position, 7 * Time.deltaTime);
                }

                else
                {
                    Destroy(circleObject);

                    leap_aoeObject = (GameObject)Instantiate(leap_aoePrefab, this.transform.position, Quaternion.identity);  

                    canMove = true;
                    canLeap = false;
                }
            }

            if (healingTime > 0)
            {
               healingTime -= Time.deltaTime;
            }

            else
            {
                Destroy(healingCircleObject);
            }
           
            

         
            
            if (delayAttack)
            {

                if (attackDelay > 0)
                {
                    attackDelay -= Time.deltaTime;
                }
                else
                {
                    attackDelay = .75f;
                    delayAttack = false;
              
                }
            }
            
         

           
        }
    }

    protected override void basicAttack()
    {
        if(!delayAttack && !spinning)
        {

            meleeBox.GetComponent<meleeAttack>().setAttack(20, power);
            
            delayAttack = true;
        }
    }

    protected override void abilityOne()
    {
        if(!spinning)
        {
            meleeBox.GetComponent<meleeAttack>().setAttack(20, power * 2);
        }
        
    }

    protected override void abilityTwo()
    {
        if (!spinning)
        {
            spinObject = (GameObject)Instantiate(spinPrefab, transform.position, Quaternion.identity);
            spinObject.transform.parent = this.transform;
            spinning = true;
            
        }
      
    }

    protected override void abilityThree()
    {
        // if (playerRole == "RANGED_DPS")
        // {
        //if (!aimingCircle)
        //{
        //  circleObject = (GameObject)Instantiate(circlePrefab, this.transform.position, Quaternion.identity);
        //  circleObject.transform.SetParent(this.transform);
        //  aimingCircle = true;
        //}

        //else
        //{
        //  Destroy(circleObject);
        // aimingCircle = false;
        //}
        // }

        // if (playerRole == "MELEE_DPS")
        // {
        healingCircleObject = (GameObject)Instantiate(healingCirclePrefab, this.transform.position, Quaternion.identity);
        healingTime = 3;
        updateCharges(-.99f);
        //}
    }

    protected override void shoot()
    {
        if (aimingCircle)
        {
            updateCharges(-.99f);
            startPos = this.transform.position;
            circleObject.transform.SetParent(null);
            aimingCircle = false;
            canMove = false;
            canLeap = true;
        }
    }

    
}

			
		
	