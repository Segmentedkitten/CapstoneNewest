using UnityEngine;
using System.Collections;

public class enemyParticles : MonoBehaviour
{
    void Awake()
    {

    }

    //not supported for 2d yet....so checking against 3d box around sprite
    void OnParticleCollision(GameObject other)
    {
        if (this.gameObject.tag == "fireBreath")
        {
            print("ayyy");
            other.GetComponentInParent<PlayerScript>().takeDamage(.5f);
        }
    }

    void Update()
    {
        if (this.GetComponent<ParticleSystem>())
        {
            if (!this.GetComponent<ParticleSystem>().IsAlive())
            {
               Destroy(this.transform.parent.gameObject);
            }
        }
    }
}