using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Aquarius : PlayerScript
{
    float melee_waitTime = 0;
    GameObject meleeBox;

    public GameObject bind_aoePrefab;
    GameObject bind_aoeObject;

    public GameObject light_aoePrefab;
    GameObject light_aoeObject;

    public GameObject aimPrefab;
    GameObject aimObject;
    bool aimActive = false;

    public GameObject boomerangPrefab;
    GameObject boomerangObject;

    bool crit = false;
    public int critAmount = 1;
    public float percentNum = 0.9f;
    bool addTimerFill = false;
    bool delayAttack = false;
    bool isBasicAttaking = false;
    float attackDelay = .75f;
    
    protected override void Awake()
    {
        base.Awake();
        meleeBox = this.transform.FindChild("meleeObject").gameObject;
       
    }

    protected override void passive()
    {
        
    }

    public void checkCrit()
    {
        if (Random.value > percentNum) //%30 percent chance (1 - 0.7 is 0.3)
        { //code here
            critAmount = 2;
            crit = true;
        }

        else
        {
            critAmount = 1;
            crit = false;
        }

        if (percentNum <= 0)
        {
            percentNum = 0.9f;
        }

        print(crit);
    }

    public void addFill()
    {
        if (timerFill.fillAmount <= .66f)
        {
            timerFill.fillAmount = timerFill.fillAmount + .33f;
        }

        else
        {
            timerFill.fillAmount = 1;
        }
    }

    protected override void updateControls()
    {
        base.updateControls();
     

        if(aimActive)
        {
            aim();
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

    public override void meleeComplete()
    {

        isBasicAttacking = false;
    }
   

    protected override void basicAttack()
    {
        
        if (!isBasicAttacking && !delayAttack)
        {
            
            meleeBox.GetComponent<meleeAttack>().setAttack(20, 50);
            isBasicAttacking = true;
            delayAttack = true;
        }
      
    }

    protected override void abilityOne()
    {
        if (!aimActive)
        {
            canFlip = false;
            aimObject = (GameObject)Instantiate(aimPrefab, this.transform.position, Quaternion.identity);
            aimObject.transform.SetParent(this.transform);
            aimActive = true;
        }

        else
        {

            Destroy(aimObject);
            aimActive = false;
        }
    }

    protected override void abilityTwo()
    {
        percentNum = 0;
        addTimerFill = true;

      
    }

    protected override void abilityThree()
    {

        updateCharges(-.99f);

  

        light_aoeObject = (GameObject)Instantiate(light_aoePrefab, this.transform.position, Quaternion.identity);
        light_aoeObject.transform.GetChild(1).GetComponent<aoeParticle>().aquaRef = this.GetComponent<Aquarius>();

        if (addTimerFill)
        {
            addFill();
            addTimerFill = false;
        }
        topAnimator.SetTrigger("spell");

       
    }


    protected override void aim()
    {
        float rsHorizontal = this.Player.GetAxis("moveHorizontalR");
        float rsVertical = this.Player.GetAxis("moveVerticalR");
        float trigAxis = Player.GetAxis("rightTrigger0");
        if (aimActive)
        {
            Vector2 direction = new Vector2(rsHorizontal, rsVertical);

            if (direction.magnitude > 0)
            {
                float angle = Mathf.Atan2(rsVertical * speed, rsHorizontal * speed) * (Mathf.Rad2Deg);

                if (aimActive)
                {
                    aimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
            }
        }
        

            if (trigAxis > .5)
        {
            shoot();
        }
        
        
    }

    protected override void shoot()
    {
        if (aimActive)
        {
            boomerangObject = (GameObject)Instantiate(boomerangPrefab, this.transform.position, aimObject.transform.rotation);
            //boomerangObject.transform.parent = this.transform;
            boomerangObject.GetComponent<boomerang>().playerRef = this.GetComponent<Aquarius>();

            if (addTimerFill)
            {
                addFill();
                addTimerFill = false;
            }

            updateCharges(-.33f);
            Destroy(aimObject);
            aimActive = false;
            canFlip = true;
            topAnimator.SetTrigger("spell");
        }
    }

    public bool isBasicAttacking { get; set; }
}

		
