using UnityEngine;
[CreateAssetMenu(fileName = "C_MovementParametersHolder", menuName = "Parameters/C_MovementParametersHolder")]
public class C_MovementParametersHolder : ScriptableObject
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
}
