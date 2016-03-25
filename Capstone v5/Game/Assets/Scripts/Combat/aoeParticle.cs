using UnityEngine;
using System.Collections;

public class aoeParticle : MonoBehaviour
{
    enemyScript enemy;
    PlayerScript player;
    public Aquarius aquaRef;
    float _damage = 0;
    float _healing = 0;

    void Awake()
    {

    }
    public void Initialize(float damage, float healing)
    {
        _damage = damage;
        _healing = healing;

    }

    //not supported for 2d yet....so checking against 3d box around sprite
    void OnParticleCollision(GameObject other)
    {
        if (this.gameObject.tag != "lightAOE")
        {
            if (other.tag == "enemy")
            {
                print("hit");
                enemy = other.GetComponentInParent<enemyScript>();

                if(enemy.checkObject(this.transform.parent.gameObject))
                {
                    enemy.takeDamage(_damage);
                }
                
            }
        }

        else
        {
            if (other.tag == "player3DBOX")
            {
                player = other.GetComponentInParent<PlayerScript>();

                player.checkObject(this.transform.parent.gameObject);
            }
        }
    }

    void Update()
    {
        if (this.GetComponent<ParticleSystem>())
        {
            if (!this.GetComponent<ParticleSystem>().IsAlive())
            {
                if (this.gameObject.tag != "lightAOE")
                {
                    GameObject[] go = GameObject.FindGameObjectsWithTag("enemyObj");

                    foreach (GameObject obj in go)
                    {
                        if (obj.GetComponent<enemyScript>().objects.Count > 0)
                        {
                            obj.GetComponent<enemyScript>().removeObject(this.transform.parent.gameObject);
                        }
                    }
                }

                else
                {
                    GameObject[] go = GameObject.FindGameObjectsWithTag("Player");

                    foreach (GameObject obj in go)
                    {
                        if (obj.GetComponent<PlayerScript>().objects.Count > 0)
                        {
                            obj.GetComponent<PlayerScript>().removeObject(this.transform.parent.gameObject);
                        }
                    }
                }

                Destroy(this.transform.parent.gameObject);
            }
        }
    }
}
