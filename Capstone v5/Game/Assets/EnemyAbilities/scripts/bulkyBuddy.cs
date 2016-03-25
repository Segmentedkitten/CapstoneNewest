using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System.Collections;

public class bulkyBuddy : PlayerScript
{
    private bool usingBasic = false;
    public GameObject fireField;

    public bool charging = false;
    Vector3 storedPos;
    Vector3 direction;
    float dashSpeed = 50;

    chargeAttack triggerRef;

    float melee_waitTime = 0;
    GameObject meleeBox;

    protected override void Awake()
    {
        base.Awake();
        triggerRef = this.transform.FindChild("chargeTrigger").GetComponent<chargeAttack>();
        meleeBox = this.transform.FindChild("meleeCollision").gameObject;
        meleeBox.SetActive(false);
    }

    protected override void updateControls()
    {
        base.updateControls();

        if (charging)
        {
            if ((Vector3.Distance(this.transform.position, storedPos) > .5f))
            {
                this.GetComponent<Rigidbody2D>().velocity += new Vector2(direction.x, direction.y) * Time.deltaTime * dashSpeed;
                print(this.GetComponent<Rigidbody2D>().velocity);
            }

            else
            {
                triggerRef.removeTrigger_fromList();
                canMove = true;
                print(Vector3.Distance(this.transform.position, storedPos));
                charging = false;
            }
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

    public override void startCombat()
    {

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
        if (!charging)
        {
            GameObject _player = GameObject.Find("Aquarius");
            dashToPlayer(_player);
            charging = true;
        }

        //updateCharges(-.99f);
    }

    public void dashToPlayer(GameObject player)
    {
        //Note: enemy should probably only dash to player if theres a clear path ahead

        storedPos = player.transform.position;
        direction = (storedPos - this.transform.position).normalized;
        canMove = false;

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}