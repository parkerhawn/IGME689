using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeLocation : MonoBehaviour
{
    private ArcGISMapComponent mapComponent;
    private ArcGISCameraComponent cameraComponent;
    [SerializeField] private ArcGISPoint location_one = new ArcGISPoint(-86.7816, 36.1627, 0, ArcGISSpatialReference.WGS84());
    [SerializeField] private ArcGISPoint location_two = new ArcGISPoint(-73.9656, 40.7826, 0, ArcGISSpatialReference.WGS84());
    private CycleLocation changeLocationAction;

    private void Awake()
    {
        mapComponent = FindFirstObjectByType<ArcGISMapComponent>();
        cameraComponent = mapComponent.GetComponentInChildren<ArcGISCameraComponent>();
        changeLocationAction = new CycleLocation();
    }

    private void OnEnable()
    {
        changeLocationAction.Enable();
        changeLocationAction.CycleLocationActions.CycleLocationForward.started += CycleForward;
        changeLocationAction.CycleLocationActions.CycleLocationBackward.started += CycleBackward;
    }

    private void OnDisable()
    {
        changeLocationAction.Disable();
        changeLocationAction.CycleLocationActions.CycleLocationForward.started -= CycleForward;
        changeLocationAction.CycleLocationActions.CycleLocationBackward.started -= CycleBackward;

    }

    private void CycleBackward(InputAction.CallbackContext ctx)
    {
        Debug.LogWarning("Input action triggered backward");

        // change the location of the map to show the new location
        mapComponent.OriginPosition= location_one;
        var cameraLocationComp = cameraComponent.GetComponent<ArcGISLocationComponent>();
        cameraLocationComp.Position = new ArcGISPoint(-86.7816, 36.1627, 2000, ArcGISSpatialReference.WGS84());
        cameraLocationComp.Rotation = new ArcGISRotation(0, 90, 0);
    }

    private void CycleForward(InputAction.CallbackContext ctx)
    {
        mapComponent.OriginPosition = location_two;
        var cameraLocationComp = cameraComponent.GetComponent<ArcGISLocationComponent>();
        cameraLocationComp.Position = new ArcGISPoint(-73.9656, 40.7826, 2000, ArcGISSpatialReference.WGS84());
        cameraLocationComp.Rotation = new ArcGISRotation(0, 90, 0);
        Debug.LogWarning("Input action triggered forward");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
