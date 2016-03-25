using UnityEngine;
using System.Collections;

public class cameraBehaviour : MonoBehaviour
{
    GameObject[] players;
    float posX = 0;
    float posY = 0;
    //Vector3 cameraPos = new Vector3(0,0,0);
    public Vector3 new_cameraPos = new Vector3(0, 0, 0);

    float minX = Mathf.Infinity;
    float maxX = -Mathf.Infinity;
    float minY = Mathf.Infinity;
    float maxY = -Mathf.Infinity;

    float cam_widthX;
    float cam_widthY;
    float cam_minX;
    float cam_minY;
    float cam_maxX;
    float cam_maxY;
    Vector3 relativePosition;

    float max_abs_x = 0;
    float max_abs_y = 0;

    float camSize = 0;

    public bool setBox = false;
    bool canMoveX = true;
    bool canMoveY = true;

    Camera mainCamera;

    void Awake()
    {
        mainCamera = this.transform.GetChild(0).GetComponent<Camera>();
    }

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        //		cameraPos = players[0].transform.position + players[1].transform.position
        //		+ players[2].transform.position + players[3].tram
    }

    // Update is called once per frame
    void Update()
    {
        cameraUpdate();
    }

    void cameraUpdate()
    {
        //cameraPos = new_cameraPos;

        //float difX;
        //float difY;

        //float difX1 = 0;
        //float difY1 = 0;

        foreach (GameObject player in players)
        {
            posX += player.transform.position.x;
            posY += player.transform.position.y;

            Vector3 tempPlayer = player.transform.position;     

            if (tempPlayer.x < minX)
                minX = tempPlayer.x;
            if (tempPlayer.x > maxX)
                maxX = tempPlayer.x;
            //Y Bounds
            if (tempPlayer.y < minY)
                minY = tempPlayer.y;
            if (tempPlayer.y > maxY)
                maxY = tempPlayer.y;
            //print (player.transform.position.y);
        }

        new_cameraPos = new Vector3(posX / players.Length, posY / players.Length, -1);
        posX = 0;
        posY = 0;

       if(canMoveX )
       {
           this.transform.position = new Vector3(new_cameraPos.x, transform.position.y, -1);
       }
       if(canMoveY)
       {
           this.transform.position = new Vector3(transform.position.x, new_cameraPos.y, -1);
       }
        
       

        //recent
        float sizeX = maxX - minX;
        float sizeY = maxY - minY;
        float screenW = 0;
        float screenH = 0;
       
        //float x_ratio = sizeX ;
        //float y_ratio = sizeY ;

        //		float x_ratio = sizeX / (Screen.width /2);
        //		float y_ratio = sizeY / (Screen.height /2);
        Vector3 screenMin = mainCamera.ScreenToWorldPoint(Vector3.zero);
        Vector3 screenMax = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        screenW = screenMax.x - screenMin.x;
        screenH = screenMax.y - screenMin.y;
        print(screenW);
        if (sizeX * ((float)Screen.height / (float)Screen.width) > sizeY)
        {

            camSize = sizeX * ((float)Screen.height / (float)Screen.width);

        }

        else
        {
            camSize = sizeY;
        }

        //camSize = (sizeX > sizeY ? sizeX : sizeY);
        mainCamera.orthographicSize = camSize;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 6, 10);

        //must be same as clamp max
        if (mainCamera.orthographicSize >= 10)
        {
            setBox = true;
            if(sizeX > screenW - (screenW * .05f))
            {
                canMoveX = false;
            }
            else
            {
                canMoveX = true;
            }

            if (sizeX > screenH - (screenH * .05f))
            {
                canMoveX = false;
            }
            else
            {
                canMoveX = true;
            }
        }

        else
        {
            setBox = false;
        }

        minX = Mathf.Infinity;
        maxX = -Mathf.Infinity;
        minY = Mathf.Infinity;
        maxY = -Mathf.Infinity;

        /*

		if (maxX > new_camera.x + (screenMax.x - screenMin.x)/2 || minX < new_camera.x - (screenMax.x - screenMin.x)/2){}
		 */

        //		Rect rect = new Rect((int)minX, (int)minY,
        //		                    (int)(maxX - minX), (int)(maxY - minY));
        //
        //		float widthdiff = ((float)Screen.width) / ((float)rect.width);
        //		float heightdiff = ((float)Screen.width) / ((float)rect.height);
        //		float Zoom = Mathf.Min(widthdiff, heightdiff);
        //
        //		this.GetComponent<Camera> ().orthographicSize = Zoom;




        //		Vector3 crossProduct = Vector3.Cross(new_cameraPos, new Vector3(0,1,0));
        //
        //		this.GetComponent<Camera> ().orthographicSize = -crossProduct.z * .01f;
        //
        //		print ("swag" + crossProduct.z);


        //		//print (cameraPos);
        //		//print ("cameraPos: " + this.transform.position);
        //		//this.GetComponent<Camera>().rect.
        //
        //		cam_widthX = Mathf.Abs (new_cameraPos.x * 2);
        //		cam_widthY = Mathf.Abs (new_cameraPos.y * 2);
        //
        //		cam_maxX = cam_widthX;
        //		cam_maxY = cam_widthY;
        //
        //		cam_minX = cam_widthX - new_cameraPos.x;
        //		cam_minX = cam_minX - new_cameraPos.x;
        //
        //		cam_minY = cam_widthY - new_cameraPos.y;
        //		cam_minY = cam_minY - new_cameraPos.y;

        //print ("ay" + cam_maxX);

        //		if(minX < cam_minX + 1 || maxX > cam_maxX - 1|| minY < cam_minY + 1 || maxY > cam_maxY - 1)
        //		{
        //			Zoom (-1);
        //		}

        //difX = minX - minX;
        //difY = maxY - minY;

        //if (difX1 == 0) {
        //difX1 = difX;
        //difY1 = difY;
        //}
        //		if(minX < cam_minX + 1 || maxX > cam_maxX - 1|| minY < cam_minY + 1 || maxY > cam_maxY - 1)
        //		//if(((difX > 15) || (difY > 15)) && ((difX > difX1) || (difY > difY1)))
        //		{
        //			Zoom (-.1f);
        //		}



        //difX1 = difX;
        //difY1 = difY;
        //Debug.Log (this.GetComponent<Camera> ().rect);
        //Debug.Log (this.GetComponent<Camera>().rect.xMax);
        //print (cam_maxX - cam_minX);

        //		if(new_cameraPos.magnitude > cameraPos.magnitude)
        //		{
        //			print("zoom out");
        //			this.GetComponent<Camera>().orthographicSize -= .03f;
        //		}
        //
        //		else if(new_cameraPos.magnitude < cameraPos.magnitude)
        //		{
        //			print("zoom in");
        //			this.GetComponent<Camera>().orthographicSize += .03f;
        //		}

        //		if((new_cameraPos.x + new_cameraPos.y) > (cameraPos.x + cameraPos.y))
        //		{
        //			print("zoom out");
        //			this.GetComponent<Camera>().orthographicSize -= .03f;
        //		}
        //		
        //		else if((new_cameraPos.x + new_cameraPos.y) < (cameraPos.x + cameraPos.y))
        //		{
        //			print("zoom i");
        //			this.GetComponent<Camera>().orthographicSize += .03f;
        //		}
    }

    //void ZoomOrthoCamera(Vector3 zoomTowards, float amount)
    void Zoom(float amount)
    {
        // Zoom camera
        mainCamera.orthographicSize -= amount;

        // Limit zoom
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 5, 15);
    }
}
