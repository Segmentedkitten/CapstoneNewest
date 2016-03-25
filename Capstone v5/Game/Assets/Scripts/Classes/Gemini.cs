using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System.Collections;

public class Gemini : PlayerScript {
	
	private bool usingBasic = false;
    [HeaderAttribute("Prefabs")]
    public GameObject swapAoE;
	public GameObject fireField;
	public GameObject aimPrefab;
	public GameObject blastPrefab;
	public GameObject voidField;
    public GameObject circlePrefab;
    public Material lineMat;
    GameObject circleObject;
	int currentTarget;
    targeting _targeting;
	GameObject aimObject;
	GameObject blastObject;
	GameObject pet;
	GameObject voidObject;
	Vector2 blastVect;
    Vector3 lineVect;
	LineRenderer line;
	targeting targetRef;
	float projLifetime = 3f;
    float lazercount = 20;
	bool aimActive = false;
    bool aimingCircle = false;
    bool disableAA = false;

	
	protected override void Awake()
	{
		base.Awake ();
        //Initializing the line renderer
        _targeting = this.GetComponent<targeting>();
		line = this.GetComponent<LineRenderer> ();        
		line.SetVertexCount (2);       
		line.material = lineMat;        
		line.SetWidth (0.1f, 0.1f);
        // // //
		
		targetRef = this.GetComponent<targeting> ();
		pet = transform.GetChild (0).gameObject;
	}
	
	protected override void updateControls()
	{
        RaycastHit[] hits;
        Vector3 direction = this.transform.position - lineVect;
        direction.Normalize();
        
		base.updateControls ();

        float rsHorizontal = this.Player.GetAxis("moveHorizontalR");
        float rsVertical = this.Player.GetAxis("moveVerticalR");
	

        if (gameManager.Instance.inCombat) {
			if (Player.GetButtonDown ("select") && targetRef != null) {

				_targeting.targetMode = !this.GetComponent<targeting> ().targetMode;
				if (!_targeting.targetMode) {
					_targeting.deselectTarget ();
				} else {
					_targeting.selectTarget ();
				}
                
			}
           
		} else {

			line.enabled = false;
		}

       


		if(aimActive || aimingCircle)
		{
			aim ();
		}
		
		



        if (!disableAA)
        {
            if (Player.GetButtonUp("bottomButton"))
            {
                usingBasic = false;
            }
            if (usingBasic)
            {
            }
            else
            {
                line.enabled = false;
            }
        }
        else
        {
            

            if (lazercount > 0)
            {
                line.enabled = true;
                line.SetPosition(0, this.transform.position);
                line.SetPosition(1, lineVect);
                lazercount--;

                hits = Physics.RaycastAll(this.transform.position, direction, 100f);
               
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];
                    if (hit.collider.tag == "enemyObj")
                    {
                        hit.collider.GetComponent<GameObject>().GetComponent<enemyScript>().takeDamage(50);
                    }

                }
            }
            else
            {
                GameObject aoe;
                lazercount = 15;
                disableAA = false;
                aoe = (GameObject)Instantiate(swapAoE, lineVect, Quaternion.identity);
                aoe.GetComponentInChildren<aoeParticle>().Initialize(power * a32_dmg_scale, 0);

            }


        }
		
	}

	public override void startCombat()
	{
		this.GetComponent<targeting> ().initializeTargeting ();

		pet.SetActive (true);
	}

	public override void endCombat()
	{
		pet.SetActive (false);
		if (aimObject != null) {
			Destroy(aimObject);
		}
		if (blastObject != null) {
			Destroy(blastObject);
		}
        if (circleObject != null)
        {
            Destroy(circleObject);       
        }

	}


	public override void takeDamage(float amount)
	{
		amount = amount / 2;
		base.takeDamage (amount);


	}

	protected override void basicAttack()
	{
        //Firing mah lazer at the target
        if (!disableAA)
        {

            if (aimObject != null)
            {
                Destroy(aimObject);
            }
            if (circleObject != null)
            {
                Destroy(circleObject);
            }
            currentTarget = targetRef.targets.IndexOf(targetRef.selectedTarget);


            if (targetRef.targetMode && targetRef.targets.Count > 0 && targetRef.selectedTarget != null)
            {
                usingBasic = true;

                targetRef.targets[currentTarget].GetComponent<enemyScript>().takeDamage(.75f) ;
                line.enabled = true;
                line.SetPosition(0, this.transform.position);
                line.SetPosition(1, targetRef.targetObject.transform.position);
            }

            else
            {
                line.enabled = false;
            }
        }
	}
	
	protected override void abilityOne()
	{
		if (aimObject != null) {
			Destroy(aimObject);
		}
        if (circleObject != null)
        {
            Destroy(circleObject);
        }
		if(!aimActive)
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
	
	protected override void abilityTwo()
	{
		base.abilityTwo ();
		if (aimObject != null) {
			Destroy(aimObject);
		}
        if (circleObject != null)
        {
            Destroy(circleObject);
        }
        topAnimator.SetTrigger("spell");
        //Swapping positions code
		Vector2 tempPetPos = pet.transform.position;
		Vector2 tempMyPos = transform.position;
		Object petAoe, myAoe;


		transform.position = tempPetPos;
		pet.transform.position = tempMyPos;

		petAoe = Instantiate (swapAoE, tempMyPos, Quaternion.identity);
		myAoe = Instantiate (swapAoE, tempPetPos, Quaternion.identity);

	}
	
	protected override void abilityThree()
	{
		
        if(circleObject != null)
        {
            Destroy(circleObject);
        }
		//voidObject = (GameObject)Instantiate (voidField, pet.transform.position, Quaternion.identity);
        if (!aimingCircle)
        {
            canFlip = false;
            circleObject = Instantiate(circlePrefab);
            circleObject.transform.position = this.transform.position;
            circleObject.transform.SetParent(this.transform);
            aimingCircle = true;
        }

        else
        {
            Destroy(circleObject);
            aimingCircle = false;
        }

	}

    protected override void abilityThree1()
    {
        base.abilityThree1();

        if (!aimingCircle)
        {
            canFlip = false;
            circleObject = Instantiate(circlePrefab);
            circleObject.transform.position = this.transform.position;
            circleObject.transform.SetParent(this.transform);
            aimingCircle = true;
        }

        else
        {
            Destroy(circleObject);
            aimingCircle = false;
        }


    }

	protected override void aim()
	{
        //aim for skillshot
      

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

                blastVect = direction;
            }


            //aim for circles
            if (aimingCircle)
            {

                circleObject.transform.Translate(new Vector3(rsHorizontal * .05f, rsVertical * .05f, 0));


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
            blastObject = (GameObject)Instantiate(blastPrefab, transform.position, aimObject.transform.rotation);
            blastObject.GetComponent<blast>().setAttack(blastVect, this.transform);
         
            
            updateCharges(-.33f);
            Destroy(aimObject);
            aimActive = false;
            topAnimator.SetTrigger("spell");
            canFlip = true;
        }

        if(aimingCircle)
        {
           
            disableAA = true;
            pet.GetComponent<G_Golem>().targetEnemy(circleObject.transform.position);
            lineVect = circleObject.transform.position;
         
            updateCharges(-.99f);
            Destroy(circleObject);
            aimingCircle = false;
            topAnimator.SetTrigger("spell");
            canFlip = true;

        }

	}

 


}
