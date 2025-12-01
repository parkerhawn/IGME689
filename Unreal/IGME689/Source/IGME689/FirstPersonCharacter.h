// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Camera/CameraComponent.h"
#include "GameFramework/Character.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "InputActionValue.h"
#include "FirstPersonCharacter.generated.h"

class UAnimBlueprint;
class UInputMappingContext;
class UInputAction;
class UInputComponent;

UCLASS()
class IGME689_API AFirstPersonCharacter : public ACharacter
{
	GENERATED_BODY()

public:
	// Sets default values for this character's properties
	AFirstPersonCharacter();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	UPROPERTY (EditAnywhere, BlueprintReadOnly, Category = Input)
	TObjectPtr<UInputMappingContext> FirstPersonContext;

	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Input)
	TObjectPtr<UInputAction> MoveAction;

	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Input)
	TObjectPtr<UInputAction> JumpAction;

	UPROPERTY (EditAnywhere, BlueprintReadOnly, Category = Input)
	UInputAction* LookAction;

	
public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	// Called to bind functionality to input
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

	UFUNCTION()
	void Move(const FInputActionValue& Value);

	UFUNCTION()
	void Look (const FInputActionValue& Value);

	UPROPERTY(EditAnywhere, Category = Camera)
	UCameraComponent* FirstPersonCameraComponent;

	UPROPERTY(EditAnywhere, Category = Camera)
	FVector FirstPersonCameraOffset = FVector(2.8f, 5.9f, 0.0f);

	UPROPERTY(EditAnywhere, Category = Camera)
	float FirstPersonFieldOfView = 70.0f;

	UPROPERTY(EditAnywhere, Category = Camera)
	float FirstPersonScale = 0.6f;

	UPROPERTY (VisibleAnywhere, Category = Mesh)
	USkeletalMeshComponent* FirstPersonMeshComponent;

	
};
