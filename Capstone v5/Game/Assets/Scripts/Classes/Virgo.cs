using UnityEngine;
using Rewired;
using System.Collections.Generic;

public class Virgo : PlayerScript
{
    bool canBe_Released = false;
    int currentTarget;
    targeting _targeting;
    targeting targetRef;
    float ba_waitTime = 1f;
    
    
    [HeaderAttribute("Prefabs")]
    public GameObject aimPrefab;
    public GameObject basic_arrowPrefab;
    public GameObject arrowPrefab;
    public GameObject arrow_healPrefab;
    public GameObject fire_arrowPrefab;
    public GameObject circlePrefab;
    public GameObject lineaimPrefab;
    public GameObject fire_aoePrefab;

    GameObject aimObject;
    bool aimActive = false;   
    GameObject basic_arrowObject;   
    GameObject arrowObject;  
    GameObject arrow_healObject;    
    GameObject fire_arrowObject;
    bool aimingFire = false;
    float passiveLength = 0;
    float minSpeed;
    public GameObject speedPrefab;
    GameObject speedObject;   
    GameObject circleObject;
    float invisiblityDuration = 2f;
    bool isInvisible = false;
    bool aimingCircle = false;

    
    GameObject lineaimObject;
    bool aimingLines = false;
    List<GameObject> lines = new List<GameObject>();
    List<GameObject> healingArrows = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> arrows = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public bool hitSomething = false;

    
    GameObject fire_aoeObject;

    protected override void Awake()
    {
        base.Awake();
        minSpeed = speed;
        _targeting = this.GetComponent<targeting>();
        targetRef = this.GetComponent<targeting>();
    }

    protected override void passive()
    {
        if (hitSomething)
        {
            if (speedObject == null)
            {
                speedObject = (GameObject)Instantiate(speedPrefab, this.transform.position, Quaternion.identity);
                speedObject.transform.parent = this.transform;
            }

            else
            {
                speedObject.GetComponent<ParticleSystem>().Play();
            }

            passiveLength = 2.5f;
            hitSomething = false;
        }

        if (passiveLength > 0)
        {
            if (speed < minSpeed + 4)
            {
                speed += Time.deltaTime * 4;
            }

            passiveLength -= Time.deltaTime;
        }

        else
        {
            if (speed > minSpeed)
            {
                speed -= Time.deltaTime * 4;
            }

            if (speedObject != null)
            {
                speedObject.GetComponent<ParticleSystem>().Stop();

                if (speedObject.GetComponent<ParticleSystem>().isStopped)
                {
                    if (speedObject != null)
                    {
                        Destroy(speedObject);
                    }
                }
            }
        }
    }


    protected override void updateControls()
    {
        base.updateControls();
        
        

        if (gameManager.Instance.inCombat)
        {
            if (Player.GetButtonUp("bottomButton") && targetRef != null)
            {
                if (canBe_Released)
                {
                    _targeting.targetMode = !this.GetComponent<targeting>().targetMode;
                    _targeting.deselectTarget();
                    ba_waitTime = 1f;
                    canBe_Released = false;
                }
            }

        
            if (aimActive || aimingLines || circleObject || aimingFire)
            {
                aim();
            }

            if (isInvisible)
            {
                if (invisiblityDuration > 0)
                {
                    invisiblityDuration -= Time.deltaTime;
                }

                else
                {
                    invisiblityDuration = 2f;
                    changeVisiblity();
                }
            }
        }
    }

    public override void startCombat()
    {
        this.GetComponent<targeting>().initializeTargeting();
    }

    protected override void basicAttack()
    {
        if (aimObject != null)
        {
            Destroy(aimObject);
        }
        if (aimingLines != null)
        {
            Destroy(lineaimObject);

        }
        currentTarget = targetRef.targets.IndexOf(targetRef.selectedTarget);
        ba_waitTime -= Time.deltaTime;

        if (ba_waitTime <= 0)
        {
            if (targetRef.targetMode && targetRef.targets.Count > 0 && targetRef.selectedTarget != null)
            {
                basic_arrowObject = (GameObject)Instantiate(basic_arrowPrefab, this.transform.position, Quaternion.identity);
                basic_arrowObject.GetComponent<arrow>().Initialize(power * basic_scale, 0);
                //basic_arrowObject.transform.parent = this.transform;
                basic_arrowObject.GetComponent<arrow>().targetedArrow = true;
                basic_arrowObject.GetComponent<arrow>().virgoRef = this.transform.GetComponent<Virgo>();

                int i = targetRef.targets.IndexOf(targetRef.selectedTarget);
                basic_arrowObject.GetComponent<arrow>().enemy = targetRef.targets[i].gameObject;
            }

            ba_waitTime = 1f;
        }

        if (!canBe_Released)
        {
            _targeting.targetMode = !this.GetComponent<targeting>().targetMode;
            _targeting.selectTarget();

            canBe_Released = true;
        }
    }

    protected override void abilityOne()
    {
        if (aimObject != null)
        {
            Destroy(aimObject);
        }
        if(aimingLines != null)
        {
            Destroy(lineaimObject);

        }
        if (!aimingLines)
        {
            canFlip = false;
            lineaimObject = (GameObject)Instantiate(lineaimPrefab, this.transform.position, Quaternion.identity);
            lineaimObject.transform.SetParent(this.transform);

            aimingLines = true;
        }

        else
        {
            Destroy(lineaimObject);
            aimingLines = false;
        }
    }

    protected override void abilityTwo()
    {
        if(aimObject != null)
        {
            Destroy(aimObject);
        }
        if (aimingLines != null)
        {
            Destroy(lineaimObject);

        }
        if (!aimingCircle)
        {
            canFlip = false;
            circleObject = (GameObject)Instantiate(circlePrefab, this.transform.position, Quaternion.identity);
            circleObject.transform.SetParent(this.transform);

            aimingCircle = true;
        }

        else
        {
            Destroy(circleObject);
            aimingCircle = false;
        }
    }

    protected override void abilityThree()
    {
        if (aimObject != null)
        {
            Destroy(aimObject);
        }
        if (playerRole == ENUMS.ROLETYPE.SUPPORT)
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

        if (playerRole == ENUMS.ROLETYPE.RANGED_DPS)
        {
            currentTarget = targetRef.targets.IndexOf(targetRef.selectedTarget);

            if (!_targeting.targetMode)
            {
                _targeting.selectTarget();
            }

            else
            {
                _targeting.deselectTarget();
            }

            aimingFire = !aimingFire;
            _targeting.targetMode = !this.GetComponent<targeting>().targetMode;
        }
    }

    protected override void aim()
    {
        float rsHorizontal = this.Player.GetAxis("moveHorizontalR");
        float rsVertical = this.Player.GetAxis("moveVerticalR");
        float trigAxis = Player.GetAxis("rightTrigger0");

        Vector2 direction = new Vector2(rsHorizontal, rsVertical);
        if (aimActive || aimingLines)
        {
            if (direction.magnitude > 0)
            {
                float angle = Mathf.Atan2(rsVertical * speed, rsHorizontal * speed) * (Mathf.Rad2Deg);

                if (aimActive)
                {
                    aimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }

                else if (aimingLines)
                {
                    lineaimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
            }
        }
        if(circleObject)
        {
           circleObject.transform.Translate(new Vector3(rsHorizontal * .2f, rsVertical * .2f, 0));
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
            arrowObject = (GameObject)Instantiate(arrowPrefab, this.transform.position, aimObject.transform.rotation);
            arrowObject.GetComponent<arrow>().Initialize(power * a32_dmg_scale, healingPower * a31_heal_scale);
            //arrowObject.transform.parent = this.transform;

            updateCharges(-.99f);
            Destroy(aimObject);
            aimActive = false;
            canFlip = true;
        }

        if (aimingFire)
        {
            fire_arrowObject = (GameObject)Instantiate(fire_arrowPrefab, this.transform.position, Quaternion.identity);
            //fire_arrowObject.transform.parent = this.transform;
            fire_arrowObject.GetComponent<arrow>().Initialize(power * a32_dmg_scale, 0);
            fire_arrowObject.GetComponent<arrow>().targetedArrow = true;
            fire_arrowObject.GetComponent<arrow>().virgoRef = this.transform.GetComponent<Virgo>();

            int i = targetRef.targets.IndexOf(targetRef.selectedTarget);
            fire_arrowObject.GetComponent<arrow>().enemy = targetRef.targets[i].gameObject;

            _targeting.deselectTarget();
            _targeting.targetMode = false;

            updateCharges(-.99f);
            aimingFire = false;
            canFlip = true;
        }

        if (aimingCircle)
        {
            this.transform.position = circleObject.transform.position;

            updateCharges(-.66f);
            Destroy(circleObject);
            changeVisiblity();
            aimingCircle = false;
        }

        if (aimingLines)
        {
            lines.Clear();
            lines.AddRange(GameObject.FindGameObjectsWithTag("line"));

            foreach (GameObject line in lines)
            {
                arrow_healObject = (GameObject)Instantiate(arrow_healPrefab, line.transform.position, line.transform.rotation);
                arrow_healObject.GetComponent<arrow>().Initialize(power * a1_dmg_scale,healingPower* a1_heal_scale);
                //arrow_healObject.transform.parent = this.transform;
            }

            updateCharges(-.33f);
            Destroy(lineaimObject);
            aimingLines = false;
            canFlip = true;
        }
    }

    void changeVisiblity()
    {
        Color32 color = this.GetComponent<SpriteRenderer>().color;
        

        if (color.a == 255)
        {
            color.a = 100;
        }

        else
        {
            color.a = 255;
        }

        this.GetComponent<SpriteRenderer>().color = color;

        isInvisible = !isInvisible;
    }

    public void clearLists(GameObject obj)
    {
        int i = arrows.IndexOf(obj);

        arrows.Remove(obj);
        enemies.RemoveAt(i);

        if (arrows.Count > 0 && enemies.Count > 0)
        {
            print("You don fooked up m8");
        }
    }

    public void make_fireAoe(GameObject enemy)
    {
        fire_aoeObject = (GameObject)Instantiate(fire_aoePrefab, enemy.transform.position, Quaternion.identity);
        fire_aoeObject.GetComponentInChildren<aoeParticle>().Initialize(a32_dmg_scale * power, healingPower * a32_heal_scale);
    }
}

