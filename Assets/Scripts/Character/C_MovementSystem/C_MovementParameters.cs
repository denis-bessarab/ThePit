using UnityEngine;

public class C_MovementParameters : MonoBehaviour
{
    [SerializeField] public LayerMask groundLayerMask;
    [SerializeField] public PhysicsMaterial2D characterMaterial;
    [SerializeField] public float verticalRaycastDistance;
    [SerializeField] public float horizontalRaycastDistance;
    [SerializeField] public float jumpPower;
    [SerializeField] public float wallJumpPowerForwardX;
    [SerializeField] public float wallJumpPowerForwardY;
    [SerializeField] public float wallJumpPowerBackwardX;
    [SerializeField] public float wallJumpPowerBackwardY;
    [SerializeField] public float slidingPower;
    [SerializeField] public float slidingTime;
    [SerializeField] public float runningSpeed;
    [SerializeField] public float sprintingSpeed;
    [SerializeField] public float hangingOnWallTime;
    [SerializeField] public float runningWallSpeed;
    [SerializeField] public float runningWallTime;
    [SerializeField] public float climbingUpSpeed;
    [SerializeField] public float fallDynamicGravity;
    [SerializeField] public float jumpDynamicGravity;
    [SerializeField] public float fallingVelocityMax;
    [SerializeField] public float horizontalVelocityMax;
    [SerializeField] public float additionalJumpPower;
    [SerializeField] public int jumpPowerAddTimesLimit;
    [SerializeField] public float airPositionAdjustmentPower;
    [SerializeField] public float horizontalJumpPower;
    [SerializeField] public float maxStamina;
    [SerializeField] public float jumpStaminaCost;
    [SerializeField] public float staminaRegenerationSpeed;
    [SerializeField] public float sprintingStaminaCost;
    [SerializeField] public float wallRunningStaminaCost;
    [SerializeField] public float wallJumpSoftStaminaCost;
    [SerializeField] public float wallJumpHardStaminaCost;
    [SerializeField] public float staticStaminaCost;
    [SerializeField] public float climbingStaminaCost;
    [SerializeField] public float everySpentStaminaDecreace;


    private void Reset()
    {
        var parameters = GetParameters();
        ApplyParameters(parameters);
    }

    private C_MovementParametersHolder GetParameters()
    {
        return Resources.Load("Parameters/C_MovementParametersHolder") as C_MovementParametersHolder;
    }

    private void ApplyParameters(C_MovementParametersHolder p)
    {
        groundLayerMask = p.groundLayerMask;
        characterMaterial = p.characterMaterial;
        verticalRaycastDistance = p.verticalRaycastDistance;
        horizontalRaycastDistance = p.horizontalRaycastDistance;
        jumpPower = p.jumpPower;
        wallJumpPowerForwardX = p.wallJumpPowerForwardX;
        wallJumpPowerForwardY = p.wallJumpPowerForwardY;
        wallJumpPowerBackwardX = p.wallJumpPowerBackwardX;
        wallJumpPowerBackwardY = p.wallJumpPowerBackwardY;
        slidingPower = p.slidingPower;
        slidingTime = p.slidingTime;
        runningSpeed = p.runningSpeed;
        sprintingSpeed = p.sprintingSpeed;
        hangingOnWallTime = p.hangingOnWallTime;
        runningWallSpeed = p.runningWallSpeed;
        runningWallTime = p.runningWallTime;
        climbingUpSpeed = p.climbingUpSpeed;
        fallDynamicGravity = p.fallDynamicGravity;
        jumpDynamicGravity = p.jumpDynamicGravity;
        fallingVelocityMax = p.fallingVelocityMax;
        horizontalVelocityMax = p.horizontalVelocityMax;
        additionalJumpPower = p.additionalJumpPower;
        jumpPowerAddTimesLimit = p.jumpPowerAddTimesLimit;
        airPositionAdjustmentPower = p.airPositionAdjustmentPower;
        horizontalJumpPower = p.horizontalJumpPower;
        maxStamina = p.maxStamina;
        jumpStaminaCost = p.jumpStaminaCost;
        staminaRegenerationSpeed = p.staminaRegenerationSpeed;
        sprintingStaminaCost = p.sprintingStaminaCost;
        wallRunningStaminaCost = p.wallRunningStaminaCost;
        wallJumpSoftStaminaCost = p.wallJumpSoftStaminaCost;
        wallJumpHardStaminaCost = p.wallJumpHardStaminaCost;
        staticStaminaCost = p.staticStaminaCost;
        climbingStaminaCost = p.climbingStaminaCost;
        everySpentStaminaDecreace = p.everySpentStaminaDecreace;
    }
}
