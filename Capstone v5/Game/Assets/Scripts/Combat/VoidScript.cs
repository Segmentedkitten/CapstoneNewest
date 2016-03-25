using UnityEngine;
using System.Collections;

public class VoidScript : MonoBehaviour {
		
	float count = 5;
	bool countStart = false;


	// Use this for initialization
	void Start () {
	
		countStart = true;
	}


	
	// Update is called once per frame
	void Update () {

		if (countStart) {

			if(count >0)
			{
				count -= Time.deltaTime;

			}
			else{
				Destroy(this.gameObject);
			}

		}
	
	}


	void OnTriggerStay2D(Collider2D other)
	{

		if (other.tag == "enemyObj") {


			other.GetComponent<enemyScript>().takeDamage(3);
		}

	}
}
