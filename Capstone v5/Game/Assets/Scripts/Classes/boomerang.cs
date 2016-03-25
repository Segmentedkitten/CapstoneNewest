using UnityEngine;
using System.Collections;

public class boomerang : MonoBehaviour
{
    public Aquarius playerRef;

    GameObject boomerangObj;
    bool turnAroundBoomerang;
    Vector2 storedPlayerPos;
    float distance;

    float boomerangSpeed = 50;
    bool hitAly = false;

    // Use this for initialization
    void Start()
    {
        storedPlayerPos = playerRef.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position + this.transform.right * (Time.deltaTime * boomerangSpeed));

        distance = Vector3.Distance(storedPlayerPos, this.transform.position);
        //print("distance is: " + boomerang.distance);

        if (distance > 15)
        {
            turnAroundBoomerang = true;
        }

        //print(boomerang.turnAroundBoomerang);
        //print("rotation" + boomerang.boomerangObj.transform.rotation);

        if (turnAroundBoomerang)
        {
            float x = playerRef.transform.position.x - this.transform.position.x;
            float y = playerRef.transform.position.y - this.transform.position.y;

            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerRef.gameObject)
        {
            if (turnAroundBoomerang)
            {
                if (hitAly)
                {
                    playerRef.checkCrit();

                    PlayerScript player = playerRef.gameObject.GetComponentInParent<PlayerScript>();
                    player.heal(75 * playerRef.critAmount);
                }

                Destroy(this.gameObject);
            }
        }

        else if (other.tag == "Player")
        {
            playerRef.checkCrit();

            PlayerScript player = other.GetComponentInParent<PlayerScript>();
            player.heal(150 * playerRef.critAmount);
            hitAly = true;
        }
    }
}
