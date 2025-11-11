// Fill out your copyright notice in the Description page of Project Settings.


#include "APIMapCreator.h"
#include "ArcGISMapsSDK/Components/ArcGISLocationComponent.h"
#include "ArcGISMapsSDK/Components/ArcGISMapComponent.h"
#include "Kismet/GameplayStatics.h"
#include "Kismet/KismetSystemLibrary.h"
// Sets default values
AAPIMapCreator::AAPIMapCreator()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	locationComponent = CreateDefaultSubobject<UArcGISLocationComponent>("LocationComponent");
	locationComponent -> SetupAttachment(GetRootComponent());
}

// Called when the game starts or when spawned
void AAPIMapCreator::BeginPlay()
{
	Super::BeginPlay();

	auto map =  UGameplayStatics::GetActorOfClass(GetWorld(), UArcGISMapComponent::StaticClass());
	mapComponent = Cast<UArcGISMapComponent>(map);
}

// Called every frame
void AAPIMapCreator::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);
	
}

