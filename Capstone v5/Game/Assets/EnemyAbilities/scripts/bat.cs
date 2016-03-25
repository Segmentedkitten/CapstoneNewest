using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System.Collections;

public class bat : PlayerScript
{
    private bool usingBasic = false;
    public GameObject fireField;
    int currentTarget;

    targetingPlayers _targeting;

    public GameObject homingMissilePrefab;
    GameObject homingMissileObject;

    float melee_waitTime = 0;
    GameObject meleeBox;

    protected override void Awake()
    {
        base.Awake();
        _targeting = this.GetComponent<targetingPlayers>();
        meleeBox = this.transform.FindChild("meleeCollision").gameObject;
        meleeBox.SetActive(false);
    }

    protected override void updateControls()
    {
        base.updateControls();

        if (melee_waitTime > 0)
        {
            melee_waitTime -= Time.deltaTime;
        }

        else
        {
            meleeBox.SetActive(false);
        }
    }

    public override void startCombat()
    {
        this.GetComponent<targetingPlayers>().initializeTargeting();
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
        _targeting.pickTarget();

        homingMissileObject = (GameObject)Instantiate(homingMissilePrefab, this.transform.position, Quaternion.identity);

        int i = _targeting.targets.IndexOf(_targeting.selectedTarget);
        homingMissileObject.GetComponent<homingMissile>().targetObj = _targeting.targets[i].gameObject;

        //updateCharges(-.99f);
    }
}