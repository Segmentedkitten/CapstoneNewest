using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System.Collections;

public class tikiMan : PlayerScript
{
    private bool usingBasic = false;
    public GameObject fireField;
    int currentTarget;

    public GameObject fireBreathPrefab;
    GameObject fireBreathObj;
    GameObject firePoint;

    float melee_waitTime = 0;
    GameObject meleeBox;

    protected override void Awake()
    {
        base.Awake();
        firePoint = this.transform.FindChild("FirePoint").gameObject;
        meleeBox = this.transform.FindChild("meleeCollision").gameObject;
        meleeBox.SetActive(false);
    }

    protected override void updateControls()
    {
        base.updateControls();

        if(fireBreathObj)
        { 
            if(this.transform.localScale.x > 0)
            {
               fireBreathObj.transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            else if(this.transform.localScale.x < 0)
            {
               fireBreathObj.transform.localEulerAngles = new Vector3(0, 0, -180);
            }

            fireBreathObj.transform.position = firePoint.transform.position;
        }

        if (melee_waitTime > 0)
        {
            melee_waitTime -= Time.deltaTime;
        }

        else
        {
            meleeBox.SetActive(false);
        }
    }

    protected override void basicAttack()
    {
        if (!meleeBox.activeSelf)
        {
            meleeBox.GetComponent<enemyMelee>().removeFromLists();
            meleeBox.SetActive(false);
            meleeBox.SetActive(true);

            melee_waitTime = 1;
        }
    }

    protected override void abilityOne()
    {

    }

    protected override void abilityTwo()
    {

    }

    protected override void abilityThree()
    {
        if (fireBreathObj == null)
        {
            fireBreathObj = (GameObject)Instantiate(fireBreathPrefab, firePoint.transform.position, Quaternion.identity);
        }

        //updateCharges(-.99f);
    }
}

