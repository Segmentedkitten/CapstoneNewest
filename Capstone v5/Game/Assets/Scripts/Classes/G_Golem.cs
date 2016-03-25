using UnityEngine;
using System.Collections;
using Rewired;

public class G_Golem : MonoBehaviour {
    [HeaderAttribute("Stats")]
	public float _speed = 2;	
	Player myPlayer;
    bool targetingEnemy = false;
    float lazercount = 20;
    [HeaderAttribute("Ignore")]
    public LineRenderer line;
    public Material lineMat;
    public bool isIdle = false;

	// Use this for initialization
	void Awake () {
		this.gameObject.SetActive (false);
		myPlayer = transform.parent.GetComponent<PlayerScript> ().Player;

        line = this.GetComponent<LineRenderer>();
        line.SetVertexCount(2);
        line.material = lineMat;
        line.SetWidth(0.1f, 0.1f);
        line.enabled = false;	
	}
	
	// Update is called once per frame
	void Update () {

		if (isIdle) {
			calculateIdleMove ();
		} else {
			//updateControls();
			    
		}

        if (targetingEnemy)
        {
            if (lazercount > 0)
            {
                lazercount--;
            }
            else
            {
                lazercount = 20;
                line.enabled = false;
                targetingEnemy = false;
            }
        }



	
	}
	void updateControls ()
	{		
		float moveHorizontal = myPlayer.GetAxis("moveHorizontalR") * (_speed * .03f);
		float moveVertical = myPlayer.GetAxis("moveVerticalR") * (_speed * .03f);

		transform.Translate(new Vector2(moveHorizontal, moveVertical));

		transform.localPosition = new Vector2 (Mathf.Clamp (transform.localPosition.x, -50, 50), Mathf.Clamp (transform.localPosition.y, -30, 30));

	}

    public void targetEnemy(Vector2 newVect)
    {
        targetingEnemy = true;
        line.enabled = true;
        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, newVect);

    }


	void calculateIdleMove()
	{
		float dx = this.transform.position.x - this.transform.parent.transform.position.x;
		float dy = this.transform.position.y - this.transform.parent.transform.position.y;
		float rot = Mathf.Atan2 (dy, dx);
		rot += 1.57f;
		dy = transform.position.y + (_speed * Mathf.Sin (rot));
		dx = transform.position.x + ( _speed * Mathf.Cos (rot));

		this.transform.position =  Vector2.Lerp(transform.position, new Vector2(dx, dy), Time.deltaTime );

	}
}
