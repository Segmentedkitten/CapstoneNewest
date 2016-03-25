using UnityEngine;
using System.Collections;

public class stalactite : MonoBehaviour
{
    Camera mainCamera;
    float minY;

    void Awake()
    {
        mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
        minY = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).y - (this.transform.localScale.y / 2);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < minY)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerScript player = other.GetComponentInParent<PlayerScript>();
            player.takeDamage(30);

            Destroy(this.gameObject);
        }
    }
}
