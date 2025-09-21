using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class ConvertCoords : MonoBehaviour
{
    private GameObject tree;
    private ArcGISMapComponent map;
    public Esri.ArcGISMapsSDK.Components.ArcGISLocationComponent arcGISlocation; 

    private void Awake()
    {
        tree = GameObject.FindWithTag("Tree");
        
        map = FindFirstObjectByType<ArcGISMapComponent>();
        
        ArcGISPoint point = arcGISlocation.Position;
        
        Debug.LogWarning ($"Longitude: {point.X} Latitude: {point.Y}");
            
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
