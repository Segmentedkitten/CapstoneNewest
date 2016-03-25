using UnityEngine;
using System.Collections;

public class arrow : MonoBehaviour
{
    public Virgo virgoRef;
    float arrowLifetime = 3f;
    public GameObject enemy;
    float arrowSpeed = 20;
    public float _arrowDmg = 0;
    public float _arrowHeal = 0;
    public bool targetedArrow = false;

    // Use this for initialization
    void Start () 
	{
        if (targetedArrow)
        {
            float x = enemy.transform.position.x - this.transform.position.x;
            float y = enemy.transform.position.y - this.transform.position.y;

            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    public void Initialize(float damage, float healing)
    {
        _arrowDmg = damage;
        _arrowHeal = healing;


    }
	
	// Update is called once per frame
	void Update () 
	{
        if (targetedArrow)
        {
            float x = enemy.transform.position.x - this.transform.position.x;
            float y = enemy.transform.position.y - this.transform.position.y;

            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        else
        {
            if (arrowLifetime > 0)
            {
                arrowLifetime -= Time.deltaTime * 3;
            }

            else
            {
                Destroy(this.gameObject);
                arrowLifetime = 3f;
            }
        }

        this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position + this.transform.right * (Time.deltaTime * arrowSpeed));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.tag == "straightArrow")
        {
            if (other.tag == "enemyCollider")
            {
                enemyScript enemy = other.GetComponentInParent<enemyScript>();
                enemy.takeDamage(_arrowDmg);
            }

            if (other.tag == "Player")
            {
                PlayerScript player = other.GetComponentInParent<PlayerScript>();
                player.heal(_arrowHeal);
            }
        }

        if (this.tag == "healingArrow")
        {
            if (other.tag == "Player")
            {
                PlayerScript player = other.GetComponentInParent<PlayerScript>();
                player.heal(_arrowHeal);
            }
            if(other.tag == "enemyCollider")
            {
                enemyScript enemy = other.GetComponentInParent<enemyScript>();
                enemy.takeDamage(_arrowDmg);

            }
        }

        if (this.tag == "basicArrow")
        {
            if (other.tag == "enemyCollider")
            {
                if (enemy == other.transform.parent.gameObject)
                {
                    enemy.GetComponent<enemyScript>().takeDamage(_arrowDmg);
                    virgoRef.hitSomething = true;
                    Destroy(this.gameObject);
                }
            }
        }

        if (this.tag == "fireArrow")
        {
            if (other.tag == "enemyCollider")
            {
                if (enemy == other.transform.parent.gameObject)
                {
                  
                    enemy.GetComponent<enemyScript>().takeDamage(_arrowDmg);
                    virgoRef.make_fireAoe(enemy);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
