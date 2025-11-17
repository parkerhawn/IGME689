// Fill out your copyright notice in the Description page of Project Settings.


#include "FeatureLayerQuery.h"

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
	
	UE_LOG(LogTemp, Warning, TEXT("%s"), *ResponseBody);

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
			
			UE_LOG(LogTemp, Warning, TEXT("%i"), coordinates.Num());
			
			// loop through each geometry value
			for (int i = 0; i< coordinates.Num(); i++)
			{
				auto thisGeometry = coordinates[i]->AsArray();
				FGeometries geometry;
				UE_LOG(LogTemp, Warning, TEXT("%i"), thisGeometry.Num());
				
				// loop through each struct of coordinates
				for (int j = 0; j < thisGeometry.Num()-1; j++)
				{
					auto coordinatesArray = thisGeometry[j]->AsArray();
					for (int k = 0; k < 1; k ++)
					{
						float xCoord = coordinatesArray[k]->AsNumber();
						float yCoord = coordinatesArray[k+1]->AsNumber();
						UE_LOG(LogTemp, Warning, TEXT("%f"), xCoord);
						UE_LOG(LogTemp, Warning, TEXT("%f"), yCoord);
						geometry.geometry.Add(xCoord);
						geometry.geometry.Add(yCoord);
					}
					
					currentFeature.Geometries.Add(geometry);
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

