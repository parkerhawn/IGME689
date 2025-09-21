using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Renderer;
using Esri.GameEngine.Attributes;
using Esri.GameEngine.Layers;
using Esri.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UISliderControl : MonoBehaviour
{
    public enum AttributeTpye
    {
        None,
        CNSTRCT_YR
    };
    

    private ArcGISMapComponent mapComponent;
    private Esri.GameEngine.Attributes.ArcGISAttributeProcessor attributeProcessor;
    private ArcGISRenderer layerRenderer;
    [SerializeField] private TextMeshProUGUI sliderText = null;
    [SerializeField] private float maxSliderAmount = 2025.0f;
    [SerializeField] public ArcGIS3DObjectSceneLayer buildingLayer;
    public delegate void SetLayerAttributesEventHandler(Esri.GameEngine.Layers.ArcGIS3DObjectSceneLayer layer);
    public event SetLayerAttributesEventHandler SetLayerAttributes;
    public Material buildingMaterial;

    private void Awake()
    {
        mapComponent  = GetComponent<ArcGISMapComponent>();
        buildingLayer = mapComponent.GetComponentInChildren<ArcGIS3DObjectSceneLayer>();
        layerRenderer = buildingLayer.MaterialReference.GetComponent<ArcGISRenderer>();
        Setup3DAttributesFloatAndIntegerType(buildingLayer);
        

        
    }
    public void SliderChange(float value)
    {
        float localValue = value * maxSliderAmount;
        sliderText.text = localValue.ToString("0");
        //buildingLayer.DefinitionExpression

    }

    // This function is an example of how to use attributes WITHOUT the attribute processor
    private void Setup3DAttributesFloatAndIntegerType(ArcGIS3DObjectSceneLayer layer)
    {

        var layerAttributes = ArcGISImmutableArray<String>.CreateBuilder();
        layerAttributes.Add("CNSTRCT_YR");
        layer.SetAttributesToVisualize(layerAttributes.MoveToArray());


        layer.MaterialReference = buildingMaterial;
    }

    private void Setup3DAttributesOtherType(Esri.GameEngine.Layers.ArcGIS3DObjectSceneLayer layer)
    {
        // We want to set up an array with the attributes we want to forward to the material
        // Because NAME is of type esriFieldTypeString/AttributeType.string, we will need to configure the AttributeProcessor to pass meaningful values to the material
        var layerAttributes = ArcGISImmutableArray<String>.CreateBuilder();
        layerAttributes.Add("CNSTRCT_YR");

        var renderAttributeDescriptions = ArcGISImmutableArray<Esri.GameEngine.Attributes.ArcGISVisualizationAttributeDescription>.CreateBuilder();
        renderAttributeDescriptions.Add(new Esri.GameEngine.Attributes.ArcGISVisualizationAttributeDescription("IsBuildingOfInterest", Esri.GameEngine.Attributes.ArcGISVisualizationAttributeType.Float32));

        attributeProcessor = new Esri.GameEngine.Attributes.ArcGISAttributeProcessor();

        attributeProcessor.ProcessEvent += (ArcGISImmutableArray<Esri.GameEngine.Attributes.ArcGISAttribute> layerNodeAttributes, ArcGISImmutableArray<Esri.GameEngine.Attributes.ArcGISVisualizationAttribute> renderNodeAttributes) =>
        {
            // Buffers will be provided in the same order they appear in the layer metadata
            // If layerAttributes contained an additional element, it would be at inputAttributes.At(1)
            var nameAttribute = layerNodeAttributes.At(0);

            // The outputVisualizationAttributes array expects that its data is indexed the same way as the attributeDescriptions above
            var isBuildingOfInterestAttribute = renderNodeAttributes.At(0);

            var isBuildingOfInterestBuffer = isBuildingOfInterestAttribute.Data;
            var isBuildingOfInterestData = isBuildingOfInterestBuffer.Reinterpret<float>(sizeof(byte));

            // Go over each attribute and if its name is one of the four buildings of interest set its "isBuildingOfInterest" value to 1, otherwise set it to 0
            ForEachString(nameAttribute, (string element, Int32 index) =>
            {
                isBuildingOfInterestData[index] = IsBuildingOfInterest(element);
            });
        };
        // Pass the layer attributes, attribute descriptions and the attribute processor to the layer
        layer.SetAttributesToVisualize(layerAttributes.MoveToArray(), renderAttributeDescriptions.MoveToArray(), attributeProcessor);

        // In Unity, open this material in the Shader Graph to view its implementation
        // In general, you can use this function in other scripts to change the material that is used to render the buildings
        layer.MaterialReference = buildingMaterial;

    }
        private void ForEachString(Esri.GameEngine.Attributes.ArcGISAttribute attribute, Action<string, Int32> predicate)
    {
        unsafe
        {
            var buffer = attribute.Data;
            var unsafePtr = NativeArrayUnsafeUtility.GetUnsafePtr(buffer);
            var metadata = (int*)unsafePtr;

            var count = metadata[0];

            // First integer = number of string on this array
            // Second integer = sum(strings length)
            // Next N-values (N = value of index 0 ) = Length of each string

            IntPtr stringPtr = (IntPtr)unsafePtr + (2 + count) * sizeof(int);

            for (var i = 0; i < count; ++i)
            {
                string element = null;

                // If the length of the string element is 0, it means the element is null
                if (metadata[2 + i] > 0)
                {
                    element = Marshal.PtrToStringAnsi(stringPtr, metadata[2 + i] - 1);
                }

                predicate(element, i);

                stringPtr += metadata[2 + i];
            }
        }
    }


    private int IsBuildingOfInterest(string element)
    {
        if (element == null)
        {
            return 0;
        }
        else if (element.Equals("CNSTRCT_YR"))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

}
