using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System.Collections;

public class lavaSlug : PlayerScript
{
    public GameObject lavaPoolPrefab;
    GameObject lavaPoolObject;
    bool startDisappear = false;
    bool endDisappear = false;
    float hiddenTime = 0;

    public GameObject lavaProjectilePrefab;
    GameObject lavaProjectileObject;

    float initalYScale;

    protected override void Awake()
    {
        base.Awake();
        initalYScale = this.transform.localScale.y;
    }

    protected override void updateControls()
    {
        base.updateControls();

        if (startDisappear)
        {
            if (this.transform.localScale.y > 0)
            {
                this.transform.localScale -= new Vector3(0, Time.deltaTime, 0);
            }

            else
            {
                hidden();
            }
        }

        if (endDisappear)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;

            if (this.transform.localScale.y < initalYScale)
            {
                this.transform.localScale += new Vector3(0, Time.deltaTime, 0);
            }

            else
            {
                visible();
            }
        }
    }

    public override void startCombat()
    {

    }

    protected override void basicAttack()
    {
        if (lavaProjectileObject == null)
        {
            lavaProjectileObject = (GameObject)Instantiate(lavaProjectilePrefab, new Vector2(this.transform.position.x, this.transform.position.y + (this.transform.localScale.y / 2)), Quaternion.identity);
            lavaProjectileObject.GetComponent<projectile>().gotopos = GameObject.Find("Aquarius").transform.position;
            lavaProjectileObject.GetComponent<projectile>().attributes(20, 15);
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
        if (lavaPoolObject == null)
        {
            lavaPoolObject = (GameObject)Instantiate(lavaPoolPrefab, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            canMove = false;
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            //canbeHit = false     //will be a variable in enemy class determining if enemy can be hit
            startDisappear = true;
            hiddenTime = 3f;
        }

        //updateCharges(-.99f);
    }

    void hidden()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;

        if (hiddenTime > 0)
        {
            hiddenTime -= Time.deltaTime;
        }

        else
        {
            startDisappear = false;
            endDisappear = true;
        }
    }

    void visible()
    {
        Destroy(lavaPoolObject);
        //canbeHit = true     //will be a variable in enemy class determining if enemy can be hit
        canMove = true;
        endDisappear = false;
    }
}