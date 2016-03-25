using UnityEngine;
using System.Collections;

public class homingMissile : MonoBehaviour
{
    public GameObject targetObj;
    public float speed = 6;

    /* Represents the homing sensitivity of the missile.
    Range [0.0,1.0] where 0 will disable homing and 1 will make it follow the target like crazy.
    This param is fed into the Slerp (defines the interpolation point to pick) */
    public float homingSensitivity = .5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        float x = targetObj.transform.position.x - this.transform.position.x;
        float y = targetObj.transform.position.y - this.transform.position.y;

        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), homingSensitivity);
        transform.Translate(speed * Time.deltaTime, 0, 0, Space.Self);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && /* take out for devon ---> */other.gameObject != GameObject.Find("bat"))
        {
            other.gameObject.GetComponent<PlayerScript>().takeDamage(50);
            Destroy(this.gameObject);
        }
    }
}

