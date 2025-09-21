using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esri.Unity;
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Extent;
using Esri.GameEngine.Geometry;
using UnityEngine.InputSystem;


public class LayerToggle : MonoBehaviour
{

    [SerializeField] ArcGISMapComponent mapComponent;
    [SerializeField] ArcGISLayerInstanceData layerData;
    private void Awake()
    {
        mapComponent = GetComponent<ArcGISMapComponent>();
        layerData = mapComponent.Layers[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
      
        if (layerData != null)
        {
            layerData.IsVisible = !layerData.IsVisible;
            
        }
        else
        {
            Debug.LogWarning("Target Layer not assigned");
        }
    }
}
