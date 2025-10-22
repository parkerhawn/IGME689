using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarAI : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> path = null;
    [SerializeField]
    private float ArriveDistance = 0.3f, lastpointArriveDistance = 0.1f;
    [SerializeField]
    private float turningAngleOffset = 5;
    [SerializeField]
    private Vector3 currentTargetPosition;

    private int index = 0;

    // way to stop the car
    private bool stop;

    public bool Stop
    {
        get { return stop; }
        set { stop = value; }
    }

    [field: SerializeField]
    public UnityEvent<Vector2> OnDrive { get; set; }

    private void Start()
    {
        if (path == null || path.Count == 0)
        {
            Stop = true;
        }
        else
        {
            currentTargetPosition = path[index];
        }
    }

    public void SetPath(List<Vector3> path)
    {
        if (path.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        this.path = path;
        index = 0;
        currentTargetPosition = this.path[index];

        // point we need to face
        Vector3 relativePoint = transform.InverseTransformPoint(this.path[index + 1]);

        // calculate the angle we need to move the car so it faces the correct direction
        float angle = Mathf.Atan2(relativePoint.x, relativePoint.z) * Mathf.Rad2Deg;

        // car is only rotated on y axis
        transform.rotation = Quaternion.Euler(0, angle, 0);
        Stop = false;

    }

    public void Update()
    {
        CheckIfArrived();
        Drive();
    }

    private void Drive()
    {
        if (Stop)
        {
            OnDrive?.Invoke(Vector2.zero);
        }
        else
        {
            Vector3 relativePoint = transform.InverseTransformPoint(currentTargetPosition);
            float angle = Mathf.Atan2(relativePoint.x, relativePoint.y) * Mathf.Rad2Deg;
            var rotateCar = 0;

            if (angle > turningAngleOffset)
            {
                rotateCar = 1;
            }
            else if (angle < turningAngleOffset) 
            {
                rotateCar = -1;
            }
            OnDrive?.Invoke(new Vector2(rotateCar, 1));
        }
    }

    private void CheckIfArrived()
    {
        if (Stop == false)
        {
            var distanceToCheck = ArriveDistance;
            if (index == path.Count- 1)
            {
                distanceToCheck = lastpointArriveDistance;
            }
            if (Vector3.Distance(currentTargetPosition, transform.position) < distanceToCheck)
            {
                SetNextTargetIndex();
            }
        }
    }

    private void SetNextTargetIndex()
    {
        index++;

        // check if the index is outside the bounds of the path
        if (index >= path.Count)
        {
            Stop = true;
            Destroy(gameObject);
        }
        else
        {
            currentTargetPosition = path[index];
        }
    }
}
