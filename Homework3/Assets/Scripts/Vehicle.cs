using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;

public class Vehicle : MonoBehaviour
{
    // fields

    // private
    private Vector3 movementDirection;
    private Vector3 velocity, acceleration;
    private Quaternion turning;
    public Transform model;

    // public
    public Rigidbody rbody;
    public float maxSpeed, minSpeed;
    public float accelerationRate, decelerationRate;
    public float turnSpeed;

    public LayerMask terrainLayerMask;
    Vector3 rayOrigin;
    RaycastHit terrainHit;
    Vector3 normal;
    public GameObject arrow;


    // Start is called before the first frame update
    void Start()
    {
        model = this.transform.GetChild(0);


    }

    public void FixedUpdate()
    {
        // raycast the vehicle's position on the terrain
        rayOrigin = transform.position;
        rayOrigin.y = 121f;
        Physics.Raycast(rayOrigin, Vector3.down, out terrainHit, 120, terrainLayerMask);

        // find the normal of each specific point on the terrain
        normal = Vector3.up;

        // reset the acceleration
        acceleration = Vector3.zero;

        // player is driving
        if (movementDirection.z != 0f)
        {

            // use input to calc current acceleration this frame
            acceleration = transform.forward * (movementDirection.z * accelerationRate * Time.fixedDeltaTime);

            // add acceleration to the velocity
            velocity += acceleration * Time.fixedDeltaTime;

            // make the sure the velocity doesn't infinitely increase
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }

        // vehicle slowing down
        else
        {
            // remove a percentage of the velocity based on time
            velocity *= 1f - (decelerationRate * Time.fixedDeltaTime);

            // stop the vehicle when it reaches a certain speed
            if (velocity.magnitude < minSpeed)
            {
                velocity = Vector3.zero;
            }
        }

        Vector3 terrainPos = model.position;
        if (terrainHit.transform != null)
        {
            terrainPos = terrainHit.point;
        }

        // have the position of the terrain point to the vehicle's position on the terrain



        //model.up = normal;
        model.forward = Vector3.Cross(model.right, model.up);
        // model.Rotate(model.right.x, model.up.y, model.forward.z);

        if (Vector3.Normalize(velocity) != transform.forward)
        {
            turning = Quaternion.Euler(0f, movementDirection.x * -1, 0f);
        }
        else
        {
            turning = Quaternion.Euler(0f, movementDirection.x, 0f);
        }

        model.LookAt(model.position + model.forward, normal);

        if (velocity.magnitude > 0.1)
        {
            // turn the vehicle's velocity
            velocity = turning * velocity;
            rbody.Move(terrainPos + velocity, turning * transform.rotation);


        }

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movementDirection = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(rayOrigin, Vector3.down * 120f);
    }

}