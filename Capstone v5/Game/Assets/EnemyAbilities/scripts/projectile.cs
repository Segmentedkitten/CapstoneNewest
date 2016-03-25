using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour
{
    bool move = false;
    public Vector2 gotopos;

    float speed;
    float damage;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(move)
        {
            this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position + this.transform.right * (Time.deltaTime * speed));
        }
    }

    public void attributes(float _speed, float _damage)
    {
        speed = _speed;
        damage = _damage;
        setRotation();
    }

    void setRotation()
    {
        float x = gotopos.x - this.transform.position.x;
        float y = gotopos.y - this.transform.position.y;

        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        move = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && /* take out for devon ---> */other.gameObject != GameObject.Find("lavaSlug") && other.gameObject != GameObject.Find("spiritTag"))
        {
            other.gameObject.GetComponent<PlayerScript>().takeDamage(damage);
            Destroy(this.gameObject);
        }

        if(other.tag == "combatWall")
        {
            Destroy(this.gameObject);
        }
    }
}
