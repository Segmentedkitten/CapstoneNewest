

using UnityEngine;
using System.Collections;
using Rewired;

//[RequireComponent(typeof(CharacterController))]
public class playerController : MonoBehaviour
{
    private Player player; // The Rewired Player
    public int playerId; // The Rewired player id of this character

    public float speed = 6f;

    public float normalSpeed = 6f;



    bool onSaveCircle = false;
    bool onLoadCircle = false;
    bool onItem = false;

    cameraBehaviour camBehaviour;
    Camera mainCamera;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);
        camBehaviour = GameObject.Find("cameraFollow").transform.GetComponent<cameraBehaviour>();
        mainCamera = camBehaviour.transform.GetChild(0).GetComponent<Camera>();
    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        handleMovement();

        if (camBehaviour.setBox)
        {
            Vector2 cameraBound = mainCamera.ScreenToWorldPoint(new Vector2(mainCamera.pixelWidth, mainCamera.pixelHeight));
            //Vector2 cameraBoundY = camBehaviour.GetComponent<Camera>().ScreenToWorldPoint(new Vector2(camBehaviour.gameObject.GetComponent<Camera>().rect.yMin, camBehaviour.gameObject.GetComponent<Camera>().rect.yMax));

            float posX = this.transform.position.x;
            float posY = this.transform.position.y;

            posX = Mathf.Clamp(posX, mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + (this.transform.lossyScale.x / 2), cameraBound.x - (this.transform.lossyScale.x / 2));
            posY = Mathf.Clamp(posY, mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).y + (this.transform.lossyScale.y), cameraBound.y - (this.transform.lossyScale.y));

            this.transform.position = new Vector3(posX, posY);
        }
    }

    public void equipItem(item _item)
    {
        if (_item.agility > 0)
        {
            speed += _item.agility;
        }

        print(speed);
    }

    public void de_equipItem()
    {
        if (speed > normalSpeed)
        {
            speed = normalSpeed;
        }
    }

    void handleMovement()
    {
        float moveHorizontal = player.GetAxis("moveHorizontal") * speed;
        float moveVertical = player.GetAxis("moveVertical") * speed;

        GetComponent<Rigidbody2D>().velocity = new Vector2(moveHorizontal, moveVertical);
    }

    void OnTriggerEnter2D(Collider2D other)
    {


        if (other.tag == "saveCircle")
        {
            onSaveCircle = true;
        }

        if (other.tag == "loadCircle")
        {
            onLoadCircle = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "item")
        {
            onItem = false;
        }

        if (other.tag == "saveCircle")
        {
            onSaveCircle = false;
        }

        if (other.tag == "loadCircle")
        {
            onLoadCircle = false;
        }
    }
}