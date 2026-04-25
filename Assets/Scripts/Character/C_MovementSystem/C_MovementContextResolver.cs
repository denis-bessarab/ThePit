using UnityEngine;

public class C_MovementContextResolver : MonoBehaviour
{
    public void ResolveMovementContext(
        Character c, 
        C_MovementActions a, 
        C_MovementParameters p, 
        Rigidbody2D rb, 
        CapsuleCollider2D col, 
        C_LifeCycle lc,
        C_StaminaManager sm
        )
    {
        col.sharedMaterial = null;
        switch (c.MovementContext)
        {
            case C_MovementContext.Idling:
                col.sharedMaterial = p.characterMaterial;
                break;
            case C_MovementContext.RunningLeft:
                a.RunningLeft(p, rb, c);
                break;
            case C_MovementContext.RunningRight:
                a.RunningRight(p, rb, c);
                break;
            case C_MovementContext.SprintingLeft:
                sm.SpendStamina(p.sprintingStaminaCost * Time.deltaTime);
                a.SprintLeft(p,rb, c);
                break;
            case C_MovementContext.SprintingRight:
                sm.SpendStamina(p.sprintingStaminaCost * Time.deltaTime);
                a.SprintRight(p,rb, c);
                break;
            case C_MovementContext.Jumping:
                if (a.jumpingCoroutine != null)
                {
                    Debug.Log("Coroutine is running");
                    return;
                }
                sm.SpendStamina(p.jumpStaminaCost);
                a.jumpingCoroutine = StartCoroutine(a.Jump(p, c, rb));
                break;
            case C_MovementContext.SlidingLeft:
                if (a.slidingCoroutine != null) return;
                a.slidingCoroutine = StartCoroutine(a.SlideLeft(p, rb));
                break;
            case C_MovementContext.SlidingRight:
                if (a.slidingCoroutine != null) return;
                a.slidingCoroutine = StartCoroutine(a.SlideRight(p, rb));
                break;
            case C_MovementContext.HangingOnWallLeft:
            case C_MovementContext.HangingOnWallRight:
                if (a.hangingOnWallCoroutine != null) return;
                a.hangingOnWallCoroutine = StartCoroutine(a.HangingOnWall(p, rb));
                break;
            case C_MovementContext.RunningWallUpLeft:
            case C_MovementContext.RunningWallUpRight:
                if (a.runningWallUpCoroutine != null)
                {
                    sm.SpendStamina(p.wallRunningStaminaCost * Time.deltaTime);
                    return;
                }
                a.runningWallUpCoroutine = StartCoroutine(a.RunningWallUp(p, rb, c));
                break;
            case C_MovementContext.SlidingWallDownLeft:
                rb.gravityScale = 1f;
                break;
            case C_MovementContext.SlidingWallDownRight:
                rb.gravityScale = 1f;
                break;
            case C_MovementContext.Falling:
                if (a.fallingCoroutine != null) return;
                a.fallingCoroutine = StartCoroutine(a.Fall(p,c,rb,lc));
                break;
            case C_MovementContext.WallJumpSoftLeft:
                if (a.wallJumpCoroutine != null) return;
                sm.SpendStamina(p.wallJumpSoftStaminaCost);
                a.wallJumpCoroutine = StartCoroutine(a.WallJumpSoft(Vector2.left,p,c,rb));
                break;
            case C_MovementContext.WallJumpSoftRight:
                if (a.wallJumpCoroutine != null) return;
                sm.SpendStamina(p.wallJumpSoftStaminaCost);
                a.wallJumpCoroutine = StartCoroutine(a.WallJumpSoft(Vector2.right,p,c,rb));
                break;
            case C_MovementContext.WallJumpHardLeft:
                if (a.wallJumpCoroutine != null) return;
                sm.SpendStamina(p.wallJumpHardStaminaCost);
                a.wallJumpCoroutine = StartCoroutine(a.WallJumpHard(Vector2.left, p, c, rb));
                break;
            case C_MovementContext.WallJumpHardRight:
                if (a.wallJumpCoroutine != null) return;
                sm.SpendStamina(p.wallJumpHardStaminaCost);
                a.wallJumpCoroutine = StartCoroutine(a.WallJumpHard(Vector2.right, p, c, rb));
                break;
            case C_MovementContext.HangingLeft:
            case C_MovementContext.HangingRight:
                sm.SpendStamina(p.staticStaminaCost * Time.deltaTime);
                if (a.hangingCoroutine != null) return;
                a.hangingCoroutine = StartCoroutine(a.Hanging(p, rb, c));
                break;
            case C_MovementContext.ClimbingUpLeft:
                if (a.climbingUpCoroutine != null) return;
                sm.SpendStamina(p.climbingStaminaCost);
                a.climbingUpCoroutine = StartCoroutine(a.ClimbingUp(Vector2.left,p,rb,col, c));
                break;
            case C_MovementContext.ClimbingUpRight:
                if (a.climbingUpCoroutine != null) return;
                sm.SpendStamina(p.climbingStaminaCost);
                a.climbingUpCoroutine = StartCoroutine(a.ClimbingUp(Vector2.right,p,rb,col, c));
                break;
            case C_MovementContext.ForcedSlidingWallDownLeft:
            case C_MovementContext.ForcedSlidingWallDownRight:
                rb.gravityScale = 1f;
                if (a.forcedSlidingWallDownCoroutine != null) return;
                a.forcedSlidingWallDownCoroutine = StartCoroutine(a.ForcedSlidingWallDown());
                break;
            case C_MovementContext.StepLeft:
                if (a.stepCoroutine != null) return;
                a.stepCoroutine = StartCoroutine(a.Step(Vector2.left, rb, c, p));
                break;
            case C_MovementContext.StepRight:
                if (a.stepCoroutine != null) return;
                a.stepCoroutine = StartCoroutine(a.Step(Vector2.right, rb, c, p));
                break;
            case C_MovementContext.StepDownLeft:
                if (a.stepDownCoroutine != null) return;
                a.stepDownCoroutine = StartCoroutine(a.StepDown(Vector2.left, rb, c, p));
                break;
            case C_MovementContext.StepDownRight:
                if (a.stepDownCoroutine != null) return;
                a.stepDownCoroutine = StartCoroutine(a.StepDown(Vector2.right, rb, c, p));
                break;
            case C_MovementContext.ClimbingDownLeft:
                if (a.climbingDownCoroutine != null) return;
                a.climbingDownCoroutine = StartCoroutine(a.ClimbDown(Vector2.left, rb, c, p));
                break;
            case C_MovementContext.ClimbingDownRight:
                if (a.climbingDownCoroutine != null) return;
                a.climbingDownCoroutine = StartCoroutine(a.ClimbDown(Vector2.right, rb, c, p));
                break;
            case C_MovementContext.CliffHangLeft:
                sm.SpendStamina(p.staticStaminaCost * Time.deltaTime);
                if (a.cliffHangCoroutine != null) return;
                a.cliffHangCoroutine = StartCoroutine(a.CliffHang(Vector2.left, rb, c, p));
                break;
            case C_MovementContext.CliffHangRight:
                sm.SpendStamina(p.staticStaminaCost * Time.deltaTime);
                if (a.cliffHangCoroutine != null) return;
                a.cliffHangCoroutine = StartCoroutine(a.CliffHang(Vector2.right, rb, c, p));
                break;
        }
    }
}
