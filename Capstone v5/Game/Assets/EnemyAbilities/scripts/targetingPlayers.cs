using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;

public class targetingPlayers : MonoBehaviour
{
    public List<Transform> targets;
    public Transform selectedTarget;

    public void initializeTargeting()
    {
        targets = new List<Transform>();
        selectedTarget = null;

        addAllTargets();
    }

    public void addAllTargets()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject target in go)
        {
            addTarget(target.transform);
        }
    }

    public void addTarget(Transform t)
    {
        targets.Add(t);
    }

    public void removeTarget(Transform currentTarget)
    {
        if (currentTarget == selectedTarget)
        {
            int index = targets.IndexOf(selectedTarget);
            moveTarget(index);
        }

        int i = targets.IndexOf(currentTarget);
        targets.RemoveAt(i);
    }

    void moveTarget(int i)
    {
        if (targets.Count > 1)
        {
            pickTarget();
        }
    }

    public void pickTarget()
    {
        //target will actually be based on priority/distance from enemy etc. Devon, whatever you're planning!
        int rnd = Random.Range(0, targets.Count);
        selectedTarget = targets[rnd];

        print(rnd);
    }
}