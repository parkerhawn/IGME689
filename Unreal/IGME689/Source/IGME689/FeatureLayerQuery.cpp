// Fill out your copyright notice in the Description page of Project Settings.


#include "FeatureLayerQuery.h"

#include "ArcGISMapsSDK/API/GameEngine/Layers/ArcGIS3DObjectSceneLayer.h"
#include "ArcGISMapsSDK/Components/ArcGISLocationComponent.h"
using namespace Esri::GameEngine::Geometry;
#include <rapidjson/reader.h> 

// Sets default values
AFeatureLayerQuery::AFeatureLayerQuery()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	
}

// Called when the game starts or when spawned
void AFeatureLayerQuery::BeginPlay()
{
	Super::BeginPlay();
	ProcessRequest();
}

// Called every frame
void AFeatureLayerQuery::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void AFeatureLayerQuery::OnResponseReceived(FHttpRequestPtr Request, FHttpResponsePtr Response,
	bool bSuccessfullyConnected)
{
	if (!bSuccessfullyConnected)
	{
		return;
	}
	
	
	TSharedPtr<FJsonObject> responseObject;
	const auto ResponseBody = Response->GetContentAsString();
	auto Reader = TJsonReaderFactory<>::Create(ResponseBody);
	
	if (FJsonSerializer::Deserialize(Reader, responseObject))
	{
		auto featureObjects = responseObject->GetArrayField(TEXT("features"));
		for (auto feature: featureObjects)
		{
			FProperties currentFeature;
			auto coordinates = feature->AsObject()->GetObjectField(TEXT("geometry"))->GetArrayField(TEXT("coordinates"));
			auto objectID = feature->AsObject()->GetObjectField(TEXT("properties"))->GetIntegerField("OBJECTID");
			auto areaLenght = feature->AsObject()->GetObjectField(TEXT("properties"))->GetNumberField("Shape__Area");
			auto shapeLength = feature->AsObject()->GetObjectField(TEXT("properties"))->GetIntegerField("Shape__Length");
			
			currentFeature.objectID = objectID;
			currentFeature.areaLength = areaLenght;
			currentFeature.shapeLength = shapeLength;
			
			// loop through each geometry value
			for (int i = 0; i< coordinates.Num(); i++)
			{
				auto thisGeometry = coordinates[i]->AsArray();
				FGeometries geometry;
				
				// loop through each struct of coordinates
				for (int j = 0; j < thisGeometry.Num()-1; j++)
				{
					// assign the coordinates to a new geometries objects that resets every loop
					auto coordinatesArray = thisGeometry[j]->AsArray();
					FGeometries currentCoord;
					float xCoord = coordinatesArray[0]->AsNumber();
                   	float yCoord = coordinatesArray[1]->AsNumber();
					
					currentCoord.geometry.Add(xCoord);
                    currentCoord.geometry.Add(yCoord);
					UnrealCoordinates.Add(FVector(xCoord, yCoord,1000.0f));
					UE_LOG(LogTemp, Warning, TEXT("%i"), UnrealCoordinates.Num());
					currentFeature.Geometries.Add(currentCoord);
				}
			}
			features.Add(currentFeature);
		}
	}
}

void AFeatureLayerQuery::ProcessRequest()
{
	FHttpRequestRef Request = FHttpModule::Get().CreateRequest();
	Request->OnProcessRequestComplete().BindUObject(this, &AFeatureLayerQuery::OnResponseReceived);
	Request->SetURL(webLink);
	Request->SetVerb("Get");
	Request->ProcessRequest();
}

