using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]

public class CarController : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject PATH;
    private Transform[] PathPoints;

    public float minDistance = 10;

    public int index = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();  
        PathPoints= new Transform[PATH.transform.childCount];
        for (int i = 0; i < PathPoints.Length; i ++)
        {
            PathPoints[i] = PATH.transform.GetChild(i);
        }
    }

    private void Update()
    {
        roam();
    }

    private void roam()
    {
        if (Vector3.Distance(transform.position, PathPoints[index].position) < minDistance)
        {
            if (index > 0 && index < PathPoints.Length)
            {
                index += 1;
            }
            else
            {
                index = 0;
            }
        }

        agent.SetDestination(PathPoints[index].position);
    }
}
