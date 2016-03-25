using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System.Collections;

public class Libra : PlayerScript {
	
	private bool usingBasic = false;
    public GameObject pushPrefab;
    private GameObject pushObject;
    public GameObject conePrefab;
    private GameObject coneObject;
    public GameObject aimPrefab;
    public GameObject pullPrefab;
    private GameObject pullObject;
    GameObject aimObject;
    bool aimActive = false;
	int currentTarget;
    Vector2 coneVect = Vector2.zero;

    bool pushActive = false;
	targeting targetRef;
    bool passiveSwap = true;
    GameObject meleeChild;
    public bool isBasicAttacking = false;
    bool delayAttack = false;
    float attackDelay = .75f;
 
    float coneLifetime = 2;

	
	protected override void Awake()
	{
		base.Awake ();
        meleeChild = transform.GetChild(0).gameObject;
		
	}
	
	protected override void updateControls()
	{
		base.updateControls ();

        if(passiveSwap)
        {
            currentDefense = .75f; // new calcuations here when stats are worked out
            currentHealingPower = healingPower;

        }
        else if(!passiveSwap)
        {
            currentDefense = defense;
            currentHealingPower = 75;
        }

        if(aimActive)
        {
            aim();
        }



        // Timers and such for the Libra's generated objects
        if(coneObject != null)
        {
            if (coneLifetime > 0)
            {
                coneLifetime -= Time.deltaTime * 3;
            }

            else
            {
                coneLifetime = 2f;
                Destroy(coneObject);
            }

        }
        if(delayAttack)
        {

            if(attackDelay > 0)
            {
               attackDelay -= Time.deltaTime;
            }
            else
            {
                attackDelay = .75f;
                delayAttack = false;
            }
        }


        if (pushActive)
        {
            if (pushObject.gameObject.transform.localScale.x >= 3 && pushObject.gameObject.transform.localScale.y >= 3)
            {

                Destroy(pushObject);
                pushActive = false;

            }
            else
            {

                pushObject.gameObject.transform.localScale += new Vector3(.3f, .3f, .3f);
            }
        }


	}

    public override void meleeComplete()
    {

        isBasicAttacking = false;
    }
   
  
	protected override void basicAttack()
	{
        if (aimObject != null)
        {
            Destroy(aimObject);
            aimActive = false;
        }
        if (!isBasicAttacking && !delayAttack)
        {
            meleeChild.GetComponent<meleeAttack>().setAttack(20, 50);
            delayAttack = true;
        }
	}
	
	protected override void abilityOne()
	{
        base.abilityOne();
        if(aimObject != null)
        {
            Destroy(aimObject);
            aimActive = false;
        }
        if (!isBasicAttacking)
        {
            meleeChild.GetComponent<meleeAttack>().setAttack(20,150);
        }
        topAnimator.SetTrigger("spell");
        //taunt
		
	}
	
	protected override void abilityTwo()
	{
        if (!aimActive)
        {
            canFlip = false;
            aimObject = Instantiate(aimPrefab);
            aimObject.transform.position = this.transform.position;
            aimObject.transform.SetParent(this.transform);
            aimActive = true;
        }

        else
        {
            Destroy(aimObject);
            aimActive = false;
        }
		
	}
	
	protected override void abilityThree()
	{
        base.abilityThree();
        if (aimObject != null)
        {
            Destroy(aimObject);
            aimActive = false;
        }
        if (facingRight)
        {
            pullObject = (GameObject)Instantiate(pullPrefab, transform.position, Quaternion.identity);
        }
        else if(!facingRight)
        {
            pullObject = (GameObject)Instantiate(pullPrefab, transform.position, Quaternion.identity);
            pullObject.transform.localScale = new Vector3(pullObject.transform.localScale.x * -1, pullObject.transform.localScale.y, pullObject.transform.localScale.z);
        }
        pullObject.transform.parent = transform;
        topAnimator.SetTrigger("spell");

      
		
	}
    protected override void abilityThree1()
    {
        base.abilityThree1();
        if (aimObject != null)
        {
            Destroy(aimObject);
            aimActive = false;
        }

         if (!pushActive)
         {
              pushObject = (GameObject)Instantiate(pushPrefab, transform.position, Quaternion.identity);

              pushActive = true;
         }



    }

    protected override void aim()
    {

        //Used for aiming the skillshot
        float rsHorizontal = this.Player.GetAxis("moveHorizontalR") * speed;
        float rsVertical = this.Player.GetAxis("moveVerticalR") * speed;
        float trigAxis = Player.GetAxis("rightTrigger0");

        if (aimActive)
        {
            Vector2 direction = new Vector2(rsHorizontal, rsVertical);

            if (direction.magnitude > 0)
            {
                float angle = Mathf.Atan2(rsVertical, rsHorizontal) * (Mathf.Rad2Deg);
                aimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            }
            direction.Normalize();

            coneVect = direction;
        }


        if (trigAxis > .5)
        {
            shoot();
        }



    }

    protected override void shoot()
    {
        //Only used when aiming is active, allows the play to initiate the ability
        if (aimActive)
        {
            coneObject = Instantiate(conePrefab);
            coneObject.transform.parent = transform;
            coneObject.transform.position = transform.position;
            coneObject.transform.rotation = aimObject.transform.rotation;

            updateCharges(-.66f);
            Destroy(aimObject);
            aimActive = false;
            canFlip = true ;
        }

    }
}
