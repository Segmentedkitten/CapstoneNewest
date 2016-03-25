using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;

public class targeting : MonoBehaviour 
{
	public List<Transform> targets;
	public Transform selectedTarget;

	public GameObject targetPrefab;
	public GameObject targetObject;
 
	private Player player; // The Rewired Player
	bool trigger_leftPressed = false;
	bool trigger_rightPressed = false;
	
	Vector2 endPoint;	

	public bool targetMode = false;	

	bool cycleRight = false;

	//public int targetingIndex;
	
	void Start () 
	{
		
	}

    void Awake()
    {
        player = this.GetComponent<PlayerScript>().Player;
       

        

    }

    public void initializeTargeting()
    {
        targets = new List<Transform>();
        selectedTarget = null;

        targetObject = Instantiate(targetPrefab);
        targetObject.SetActive(false);

        addAllEnemies();
        sort();

        selectedTarget = targets[0];

        targetObject.transform.position = selectedTarget.position;
        if (player.id == 0)
        {
            targetObject.GetComponent<SpriteRenderer>().color = Color.green;
        }

        else if (player.id == 1)
        {
            targetObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        else if (player.id == 2)
        {
            targetObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        }

        else if (player.id == 3)
        {
            targetObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

     


    }

	public void addAllEnemies()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag("enemyObj");

		foreach(GameObject enemy in go)
		{
			addTarget(enemy.transform);		
		}
	}

	public void addTarget(Transform enemy)
	{
		targets.Add (enemy);
	}

	public void setMode()
	{
		if(targets.Count > 0)
		{
			if (!targetMode) 
			{
				

				selectTarget();
				targetMode = true;
			}

			else
			{
				deselectTarget();
				targetMode = false;
			}
		}
	}

	private void targetEnemy()
	{
		if(targetMode)
		{
			sort();
			int index = targets.IndexOf(selectedTarget);
		
			if(cycleRight)
			{
				if(index < targets.Count - 1)
				{
					index++;
				}

			

//				else
//				{
//					index = 0;
//				}
			}

			else
			{
				if(index > 0)
				{
					index--;
				}

			

//				else
//				{
//					index = targets.Count - 1;
//				}
			}

			selectedTarget = targets[index];
			targetObject.transform.position = new Vector2(selectedTarget.position.x,selectedTarget.position.y);


		}
	}

	public void selectTarget()
	{
		if (selectedTarget != null ) {
			targetObject.SetActive (true);
		} 
	}

	public void deselectTarget ()
	{

		targetObject.SetActive (false);
	}

	public void removeTarget(Transform currentEnemy)
	{
		if (currentEnemy == selectedTarget) 
		{
			int index = targets.IndexOf(selectedTarget);
			moveTarget(index);
		}

		int i = targets.IndexOf(currentEnemy);
		targets.RemoveAt(i);
	}

	void moveTarget(int i)
	{
		if(targets.Count > 1)
		{
			if(i > 0)
			{
				selectedTarget = targets[i - 1];
				
				//targetingIndex--;
				//.Log ("It is now: " + targetingIndex);
			}
			
			else
			{
				selectedTarget = targets[i + 1];
			
				//targets.RemoveAt (targetingIndex);
			}
			

			
			if(selectedTarget != null)
			{
				targetObject.transform.position = selectedTarget.position;
			}
		}
		
		else if(targets.Count == 1)
		{
			targetMode = false;
			deselectTarget();
		}
	}

	private void sort()
	{
		Transform temp;

		for(int i = 0; i < targets.Count; i++)
		{
			for(int j = i + 1; j < targets.Count; j++)
			{
				if(targets[j].position.x < targets[i].position.x)
				{
					temp = targets[i];
					targets[i] = targets[j];
					targets[j] = temp;
				}

				else if(targets[j].position.x == targets[i].position.x)
				{
					if(targets[j].position.y > targets[i].position.y)
					{
						temp = targets[i];
						targets[i] = targets[j];
						targets[j] = temp;
					}
				}
			}
		}		
	}

	// Update is called once per frame
	void Update () 
	{

		if (targetMode) {
			float l_triggerValue = player.GetAxis ("leftBumper");
			float r_triggerValue = player.GetAxis ("rightBumper");

			if (l_triggerValue >= .9f) {
				if (!trigger_leftPressed) {				
               
					cycleRight = false;
					targetEnemy ();
					trigger_leftPressed = true;
				}
			} else {
				trigger_leftPressed = false;
			}


			if (r_triggerValue >= .9f) {
				if (!trigger_rightPressed) {
                
					cycleRight = true;
					targetEnemy ();
					trigger_rightPressed = true;
				}
			} else {
				trigger_rightPressed = false;
			}
		}
	}
}
