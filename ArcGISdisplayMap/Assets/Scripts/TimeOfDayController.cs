using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Experimental.GlobalIllumination;
using System;

public class TimeOfDayController : MonoBehaviour
{
    [SerializeField] private DirectionalLight sun;

    private double offset = -90;

    [Header("Time Variables")]
    [Range(0f, 0.1f)]
    [SerializeField] private float speed = 0.1f;

    [Range(0f, 24f)]
    [SerializeField] private double startTime = 6.0f;

    [Range(0f, 24f)]
    [SerializeField] private double stopTime = 10.0f;

    [Range(0f, 24f)]
    [SerializeField] private double sunRise;

    [Range(0f, 24f)]
    [SerializeField] private double sunSet;

    [Range(0f, 24f)]
    [SerializeField] private double time;

    private DateTime date;

    private void RotateSky()
    {
        time = System.DateTime.Now.Hour;
        var rotationCalculation =  time / 24 * 360;
        time += speed;

        if (time >= 24.0)
        {
            time = 0.0f;
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, (float)rotationCalculation);
        }
    }

    private void Awake()
    {
        sun = GetComponent<DirectionalLight>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        RotateSky();
    }
}
