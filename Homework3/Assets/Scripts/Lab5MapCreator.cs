using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Samples.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Extent;
using Esri.GameEngine.Geometry;
using Esri.Unity;
using Unity.Mathematics;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;

[ExecuteAlways]

public class Lab_Dev_Scene : MonoBehaviour
{
    public string APIKey = "AAPTxy8BH1VEsoebNVZXo8HurFIZ2YGIeKVjOP7wJjTmS1wYkuAiEY1DOM6RKY3lbHfhBbcoOJ0ZNL6NnNqogSuOg6EhDXviYOtIOoYrHZ7B3XnSfFLq8JGgt0IR0_3tokxfe0O2acIqNeBmcEitVz2xZvsHrsITFcjYSMFUQdyt5OZvhOB1XDZxsZChhwhMi8NjJzukl-q1Kviwl5P5wCUvZgd4TmjQrtyyDwvm_9xr9iU.AT1_3alqZOkJ";
    private ArcGISMapComponent arcGISMapComponent;

    //camera 35.76692775839835, -82.18222837601473
    //35.762863326496394, -82.17781926983247
    //35.76083662471189, -82.16942600790118
    private ArcGISPoint geographicCoordinates = new ArcGISPoint(-82.18101, 35.76446, 850, ArcGISSpatialReference.WGS84());

    private ArcGISCameraComponent cameraComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateArcGISMapComponent();
        CreateArcGISCamera();
        //CreateSkyComponent();
        CreateArcGISMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateArcGISMapComponent()
    {
        //arcGISMapComponent = FindObjectOfType<ArcGISMapComponent>();
        //new
        arcGISMapComponent = FindFirstObjectByType<ArcGISMapComponent>();

        if (!arcGISMapComponent)
        {
            var arcGISMapGameObject = new GameObject("ArcGISMap");
            arcGISMapComponent = arcGISMapGameObject.AddComponent<ArcGISMapComponent>();
        }

        arcGISMapComponent.OriginPosition = geographicCoordinates;
        arcGISMapComponent.MapType = Esri.GameEngine.Map.ArcGISMapType.Local;

        arcGISMapComponent.MapTypeChanged += new ArcGISMapComponent.MapTypeChangedEventHandler(CreateArcGISMap);
    }
    public void CreateArcGISMap()
    {
        var arcGISMap = new Esri.GameEngine.Map.ArcGISMap(arcGISMapComponent.MapType);
        arcGISMap.Basemap = new Esri.GameEngine.Map.ArcGISBasemap(Esri.GameEngine.Map.ArcGISBasemapStyle.ArcGISImagery, APIKey);

        arcGISMap.Elevation = new Esri.GameEngine.Map.ArcGISMapElevation(new Esri.GameEngine.Elevation.ArcGISImageElevationSource("https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer", "Terrain 3D", ""));

        var layer_1 = new Esri.GameEngine.Layers.ArcGISImageLayer("https://www.arcgis.com/home/item.html?id=b3a3743cda50473ba5df14eaf14e5d18", "MyLayer_1", 1.0f, true, "");
        arcGISMap.Layers.Add(layer_1);

        //var layer_2 = new Esri.GameEngine.Layers.ArcGISImageLayer("https://www.arcgis.com/home/item.html?id=1c28f1944276445a99f5b554c3c07771", "MyLayer_2", 1.0f, true, "");
        // arcGISMap.Layers.Add(layer_2);

        // var layer_3 = new Esri.GameEngine.Layers.ArcGISImageLayer("https://www.arcgis.com/home/item.html?id=7de726fabcc149f185b0bf8acde82878", "MyLayer_3", 1.0f, true, "");
        // arcGISMap.Layers.Add(layer_3);

        // var buildingLayer = new Esri.GameEngine.Layers.ArcGIS3DObjectSceneLayer("https://www.arcgis.com/home/item.html?id=a457834a6cb449cd958502d6e98ba305", "Building Layer", 1.0f, true, "");
        // arcGISMap.Layers.Add(buildingLayer);

        arcGISMapComponent.EnableExtent = true;

        //Busick, NC 35.770104043015294, -82.1848379425855
        var extentCenter = new Esri.GameEngine.Geometry.ArcGISPoint(-82.1848379425855, 35.770104043015294, 0, ArcGISSpatialReference.WGS84());
        var extent = new ArcGISExtentRectangle(extentCenter, 1000, 1000);
        //var extent = new ArcGISExtentCircle (extentCenter, 1000);

        arcGISMap.ClippingArea = extent;

        arcGISMapComponent.View.Map = arcGISMap;


    }

    private void CreateArcGISCamera()
    {
        cameraComponent = Camera.main.gameObject.GetComponent<ArcGISCameraComponent>();

        if (!cameraComponent)
        {
            var cameraGameObject = Camera.main.gameObject;

            cameraGameObject.transform.SetParent(arcGISMapComponent.transform, false);

            cameraComponent = cameraGameObject.AddComponent<ArcGISCameraComponent>();

            cameraGameObject.AddComponent<ArcGISCameraControllerComponent>();

            cameraGameObject.AddComponent<ArcGISRebaseComponent>();
        }

        var cameraLocationComponent = cameraComponent.GetComponent<ArcGISLocationComponent>();

        if (!cameraLocationComponent)
        {
            cameraLocationComponent = cameraComponent.gameObject.AddComponent<ArcGISLocationComponent>();

            cameraLocationComponent.Position = geographicCoordinates;
            //https://developers.arcgis.com/unity/maps/camera/
            //headng pitch roll
            //delete location object attached to camera - code will re-generate
            //run game an observe camera movement to get the values wanted
            cameraLocationComponent.Rotation = new ArcGISRotation(330, 50, 4);
        }
    }


}