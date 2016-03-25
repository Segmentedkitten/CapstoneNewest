using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System.Collections;

public class spiritTag : PlayerScript
{
    public GameObject stalactitePrefab;
    GameObject stalactiteObject;
    Camera mainCamera;
    float spawnWaitTime = 0;
    float abilityLength = 8;

    bool spawnStalactites = false;

    public GameObject spiritProjectilePrefab;
    GameObject spiritProjectileObject;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
    }

    protected override void updateControls()
    {
        base.updateControls();

        if(spawnStalactites)
        {
            if (abilityLength > 0)
            {
                abilityLength -= Time.deltaTime;

                if (spawnWaitTime > 0)
                {
                    spawnWaitTime -= Time.deltaTime;
                }

                else
                {
                    stalactiteObject = Instantiate(stalactitePrefab);

                    Vector2 cameraBound = mainCamera.ScreenToWorldPoint(new Vector2(mainCamera.pixelWidth, mainCamera.pixelHeight));
                    float posx = Random.Range(mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x, cameraBound.x);
                    float posy = cameraBound.y + (stalactiteObject.transform.localScale.y / 2);

                    stalactiteObject.transform.position = new Vector2(posx, posy);


                    spawnWaitTime = Random.Range(.1f, .7f);
                }
            }

            else
            {
                spawnStalactites = false;
            }
        }
    }

    public override void startCombat()
    {

    }

    protected override void basicAttack()
    {
        if (spiritProjectileObject == null)
        {
            spiritProjectileObject = (GameObject)Instantiate(spiritProjectilePrefab, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            spiritProjectileObject.GetComponent<projectile>().gotopos = GameObject.Find("Aquarius").transform.position;
            spiritProjectileObject.GetComponent<projectile>().attributes(12, 20);
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
        if (!spawnStalactites)
        {
            spawnWaitTime = 0;
            abilityLength = 8;

            spawnStalactites = true;
        }

        //updateCharges(-.99f);
    }
}
