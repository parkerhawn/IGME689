using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentBreakForce;
    private float steerAngle;

    private bool isBreaking;

    
    [SerializeField] private WheelCollider fLWheelCollider;
    [SerializeField] private WheelCollider fRWheelCollider;
    [SerializeField] private WheelCollider bLWheelCollider;
    [SerializeField] private WheelCollider bRWheelCollider;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(fLWheelCollider, fLWheelCollider.transform);
        UpdateSingleWheel(fRWheelCollider, fRWheelCollider.transform);
        UpdateSingleWheel(bLWheelCollider, bLWheelCollider.transform);
        UpdateSingleWheel(bRWheelCollider, bRWheelCollider.transform);

    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);
        transform.rotation = rotation;
        transform.position = position;
    }

    private void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        fLWheelCollider.steerAngle = steerAngle;
        fRWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
        fLWheelCollider.motorTorque = verticalInput * motorForce;
        fRWheelCollider.motorTorque = verticalInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        if (isBreaking)
        {
            ApplyBreaking();
        }
    }

private void ApplyBreaking()
    {
        fRWheelCollider.brakeTorque = currentBreakForce;
        fLWheelCollider.brakeTorque = currentBreakForce;
        bRWheelCollider.brakeTorque = currentBreakForce;
        bLWheelCollider.brakeTorque = currentBreakForce;

    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.S);
    }
}
