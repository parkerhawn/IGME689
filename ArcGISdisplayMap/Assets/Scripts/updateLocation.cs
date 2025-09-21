using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esri.GameEngine.MapView;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;

public class updateLocation : MonoBehaviour
{

    private ArcGISCameraComponent arcGISCamera;
    private ArcGISMapComponent arcGISMap;
    private ArcGISLocationComponent locationComponent;

    private void Awake()
    {
        arcGISMap = GetComponent<ArcGISMapComponent>();
        arcGISCamera = arcGISMap.GetComponentInChildren<ArcGISCameraComponent>();
        locationComponent = arcGISMap.GetComponentInChildren<ArcGISLocationComponent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(locationComponent.Position.X);
        Debug.Log(locationComponent.Position.Y);
    }

    // Update is called once per frame
    void Update()
    {
        locationComponent.Position = new Esri.GameEngine.Geometry.ArcGISPoint(locationComponent.Position.X + 1, locationComponent.Position.Y + 3, locationComponent.Position.Z, ArcGISSpatialReference.WGS84());
        Debug.Log(locationComponent.Position.X);
        Debug.Log(locationComponent.Position.Y);
    }
}
