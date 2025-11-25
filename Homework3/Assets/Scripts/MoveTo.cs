using Esri.ArcGISMapsSDK.Components;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveTo : MonoBehaviour
{

    [SerializeField] Object goal;
    [SerializeField] int speed;

    float goalLength;
    Vector3 goalPosition;
    Vector3 goalDirection;
    // Start is called before the first frame update
    void Start()
    {
        goalPosition = goal.GetComponent<ArcGISLocationComponent>().transform.position;
        goalLength = (this.transform.position - goalPosition).magnitude;  
        goalDirection = (this.transform.position - goalPosition).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (goalLength > 1 )
        {
            float step = Time.deltaTime * speed;
            this.transform.position = Vector3.MoveTowards(this.transform.position, goalPosition, step);
        }
        
    }
}
