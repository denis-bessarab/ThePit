using UnityEngine;

public class C_MovementContextCreator : MonoBehaviour
{
    public C_MovementContext UpdateMovementContext(
        MovementData m, 
        GroundData g, 
        C_MovementParameters p, 
        C_MovementActions a, 
        Character c, 
        Rigidbody2D rb,
        Rope r,
        C_StaminaManager sm
        )
    {
        if (r != null) return UpdateRopeContext(m, g, p, a, c, rb, r, sm);
        return UpdateDefaultContext(m, g, p, a, c, rb, r, sm);
    }

    private C_MovementContext UpdateDefaultContext(
        MovementData m,
        GroundData g,
        C_MovementParameters p,
        C_MovementActions a,
        Character c,
        Rigidbody2D rb,
        Rope r,
        C_StaminaManager sm
        )
    {
        return c.MovementContext switch
        {
            //Idling
            C_MovementContext.RunningLeft when !m.left => C_MovementContext.Idling,
            C_MovementContext.SprintingLeft when !m.left => C_MovementContext.Idling,
            C_MovementContext.RunningRight when !m.right => C_MovementContext.Idling,
            C_MovementContext.SprintingRight when !m.right => C_MovementContext.Idling,
            C_MovementContext.Falling when g.groundBelow => C_MovementContext.Idling,
            C_MovementContext.Jumping when g.groundBelow && m.vx == 0 && m.vy == 0 => C_MovementContext.Idling,
            C_MovementContext.SlidingLeft when g.groundBelow && a.slidingCoroutine == null && m.vx == 0 => C_MovementContext.Idling,
            C_MovementContext.SlidingRight when g.groundBelow && a.slidingCoroutine == null && m.vx == 0 => C_MovementContext.Idling,
            C_MovementContext.SlidingWallDownLeft when g.groundBelow => C_MovementContext.Idling,
            C_MovementContext.SlidingWallDownRight when g.groundBelow => C_MovementContext.Idling,
            C_MovementContext.HangingLeft when g.groundBelow && m.down => C_MovementContext.Idling,
            C_MovementContext.HangingRight when g.groundBelow && m.down => C_MovementContext.Idling,
            C_MovementContext.ClimbingUpLeft when a.climbingUpCoroutine == null => C_MovementContext.Idling,
            C_MovementContext.ClimbingUpRight when a.climbingUpCoroutine == null => C_MovementContext.Idling,
            C_MovementContext.StepLeft when a.stepCoroutine == null => C_MovementContext.Idling,
            C_MovementContext.StepRight when a.stepCoroutine == null => C_MovementContext.Idling,
            C_MovementContext.StepDownLeft when a.stepDownCoroutine == null => C_MovementContext.Idling,
            C_MovementContext.StepDownRight when a.stepDownCoroutine == null => C_MovementContext.Idling,
            C_MovementContext.ClimbingDownLeft when a.climbingDownCoroutine == null => C_MovementContext.Idling,
            C_MovementContext.ClimbingDownRight when a.climbingDownCoroutine == null => C_MovementContext.Idling,

            //Running Left
            C_MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft => C_MovementContext.RunningLeft,
            C_MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft => C_MovementContext.RunningLeft,
            C_MovementContext.SlidingLeft when g.groundBelow && a.slidingCoroutine == null && m.left => C_MovementContext.RunningLeft,
            C_MovementContext.SlidingLeft when g.groundBelow && a.slidingCoroutine == null && m.left => C_MovementContext.RunningLeft,
            C_MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft => C_MovementContext.RunningLeft,
            C_MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft => C_MovementContext.RunningLeft,
            C_MovementContext.HangingRight when g.groundBelow && m.left => C_MovementContext.RunningLeft,
            C_MovementContext.WallJumpHardLeft when g.groundBelow && m.left => C_MovementContext.RunningLeft,
            C_MovementContext.StepLeft when m.left && a.stepCoroutine == null => C_MovementContext.RunningLeft,
            C_MovementContext.StepDownLeft when m.left && a.stepDownCoroutine == null => C_MovementContext.RunningLeft,
            C_MovementContext.ClimbingDownLeft when m.left && a.climbingDownCoroutine == null => C_MovementContext.RunningLeft,

            //Running Right
            C_MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight => C_MovementContext.RunningRight,
            C_MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight => C_MovementContext.RunningRight,
            C_MovementContext.SlidingRight when g.groundBelow && a.slidingCoroutine == null && m.right => C_MovementContext.RunningRight,
            C_MovementContext.SlidingRight when g.groundBelow && a.slidingCoroutine == null && m.right => C_MovementContext.RunningRight,
            C_MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight => C_MovementContext.RunningRight,
            C_MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight => C_MovementContext.RunningRight,
            C_MovementContext.HangingLeft when g.groundBelow && m.right => C_MovementContext.RunningRight,
            C_MovementContext.WallJumpHardRight when g.groundBelow && m.right => C_MovementContext.RunningRight,
            C_MovementContext.StepRight when m.right && a.stepCoroutine == null => C_MovementContext.RunningRight,
            C_MovementContext.StepDownRight when m.right && a.stepDownCoroutine == null=> C_MovementContext.RunningRight,
            C_MovementContext.ClimbingDownRight when m.right && a.climbingDownCoroutine == null => C_MovementContext.RunningRight,

            //Sprinting Left
            C_MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.SlidingLeft when g.groundBelow && a.slidingCoroutine == null && m.left && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.SlidingLeft when g.groundBelow && a.slidingCoroutine == null && m.left && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.HangingRight when g.groundBelow && m.left && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.WallJumpHardLeft when g.groundBelow && m.left && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.StepLeft when m.left && a.stepCoroutine == null && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.StepDownLeft when m.left && a.stepDownCoroutine == null && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.ClimbingDownLeft when m.left && a.climbingDownCoroutine == null && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingLeft,
            C_MovementContext.RunningLeft when m.sprint => C_MovementContext.SprintingLeft,

            //Sprinting Right
            C_MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.SlidingRight when g.groundBelow && a.slidingCoroutine == null && m.right && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.SlidingRight when g.groundBelow && a.slidingCoroutine == null && m.right && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.HangingLeft when g.groundBelow && m.right && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.WallJumpHardRight when g.groundBelow && m.right && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.StepRight when m.right && a.stepCoroutine == null && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.StepDownRight when m.right && a.stepDownCoroutine == null && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.ClimbingDownRight when m.right && a.climbingDownCoroutine == null && m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,
            C_MovementContext.RunningRight when m.sprint && sm.IsEnoughStamina(p.sprintingStaminaCost) => C_MovementContext.SprintingRight,

            //Jumping
            C_MovementContext.Idling when g.groundBelow && m.jump && sm.IsEnoughStamina(p.jumpStaminaCost) => C_MovementContext.Jumping,
            C_MovementContext.RunningLeft when m.jump && sm.IsEnoughStamina(p.jumpStaminaCost) => C_MovementContext.Jumping,
            C_MovementContext.RunningRight when m.jump && sm.IsEnoughStamina(p.jumpStaminaCost) => C_MovementContext.Jumping,
            C_MovementContext.SprintingLeft when m.jump && sm.IsEnoughStamina(p.jumpStaminaCost) => C_MovementContext.Jumping,
            C_MovementContext.SprintingRight when m.jump && sm.IsEnoughStamina(p.jumpStaminaCost) => C_MovementContext.Jumping,
            C_MovementContext.Rope when r == null && m.vy > 0 && sm.IsEnoughStamina(p.jumpStaminaCost) => C_MovementContext.Jumping,

            //Falling
            C_MovementContext.Idling when !g.groundBelow => C_MovementContext.Falling,
            //C_MovementContext.Jumping when m.vy <= 0 && (m.vx == 0 || (!g.groundOnLeft && !g.groundOnRight)) => C_MovementContext.Falling,
            C_MovementContext.Jumping when m.vy <= 0 && (!(g.groundOnLeft && m.left) || !(g.groundOnRight && m.right)) => C_MovementContext.Falling,
            C_MovementContext.SlidingLeft when !g.groundBelow && a.slidingCoroutine == null => C_MovementContext.Falling,
            C_MovementContext.SlidingRight when !g.groundBelow && a.slidingCoroutine == null => C_MovementContext.Falling,
            C_MovementContext.WallJumpSoftLeft when m.vy <= 0 => C_MovementContext.Falling,
            C_MovementContext.WallJumpSoftRight when m.vy <= 0 => C_MovementContext.Falling,
            C_MovementContext.WallJumpHardLeft when m.vy <= 0 => C_MovementContext.Falling,
            C_MovementContext.WallJumpHardRight when m.vy <= 0 => C_MovementContext.Falling,
            C_MovementContext.SprintingLeft when !g.groundBelow => C_MovementContext.Falling,
            C_MovementContext.SprintingRight when !g.groundBelow => C_MovementContext.Falling,
            C_MovementContext.SlidingWallDownLeft when !g.groundOnLeft => C_MovementContext.Falling,
            C_MovementContext.SlidingWallDownRight when !g.groundOnRight => C_MovementContext.Falling,
            C_MovementContext.ForcedSlidingWallDownLeft when !g.groundOnLeft && a.forcedSlidingWallDownCoroutine == null => C_MovementContext.Falling,
            C_MovementContext.ForcedSlidingWallDownRight when !g.groundOnRight && a.forcedSlidingWallDownCoroutine == null => C_MovementContext.Falling,
            C_MovementContext.Rope when r == null && m.vy <= 0 => C_MovementContext.Falling,
            C_MovementContext.HangingLeft when !sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.Falling,
            C_MovementContext.HangingRight when !sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.Falling,

            //Running wall up left
            C_MovementContext.Jumping when !g.groundBelow && m.left && g.groundTopLeft && g.groundBottomLeft && sm.IsEnoughStamina(p.wallRunningStaminaCost) => C_MovementContext.RunningWallUpLeft,
            C_MovementContext.WallJumpHardLeft when g.groundTopLeft && g.groundBottomLeft && m.vy > 0 &&  sm.IsEnoughStamina(p.wallRunningStaminaCost) => C_MovementContext.RunningWallUpLeft,

            //Running wall up right
            C_MovementContext.Jumping when !g.groundBelow && m.right && g.groundTopRight && g.groundBottomRight && sm.IsEnoughStamina(p.wallRunningStaminaCost) => C_MovementContext.RunningWallUpRight,
            C_MovementContext.WallJumpHardRight when g.groundTopRight && g.groundBottomRight && m.vy > 0 && sm.IsEnoughStamina(p.wallRunningStaminaCost) => C_MovementContext.RunningWallUpRight,

            //Sliding wall down left
            C_MovementContext.HangingOnWallLeft when a.hangingOnWallCoroutine == null => C_MovementContext.SlidingWallDownLeft,
            C_MovementContext.RunningWallUpLeft when a.runningWallUpCoroutine == null => C_MovementContext.SlidingWallDownLeft,
            C_MovementContext.Falling when g.groundOnLeft && g.groundAboveLeft1f => C_MovementContext.SlidingWallDownLeft,
            C_MovementContext.ForcedSlidingWallDownLeft when g.groundOnLeft && a.forcedSlidingWallDownCoroutine == null => C_MovementContext.SlidingWallDownLeft,

            //Sliding wall down right
            C_MovementContext.HangingOnWallRight when a.hangingOnWallCoroutine == null => C_MovementContext.SlidingWallDownRight,
            C_MovementContext.RunningWallUpRight when a.runningWallUpCoroutine == null => C_MovementContext.SlidingWallDownRight,
            C_MovementContext.Falling when g.groundOnRight && g.groundAboveRight1f => C_MovementContext.SlidingWallDownRight,
            C_MovementContext.ForcedSlidingWallDownRight when g.groundOnRight && a.forcedSlidingWallDownCoroutine == null => C_MovementContext.SlidingWallDownRight,

            //Wall jump soft left
            C_MovementContext.RunningWallUpRight when m.jump && !m.left && sm.IsEnoughStamina(p.wallJumpSoftStaminaCost) => C_MovementContext.WallJumpSoftLeft,
            C_MovementContext.HangingRight when m.jump && !m.left && sm.IsEnoughStamina(p.wallJumpSoftStaminaCost) => C_MovementContext.WallJumpSoftLeft,
            C_MovementContext.SlidingWallDownRight when m.jump && !m.left && sm.IsEnoughStamina(p.wallJumpSoftStaminaCost) => C_MovementContext.WallJumpSoftLeft,
            C_MovementContext.ForcedSlidingWallDownRight when m.jump && !m.left && sm.IsEnoughStamina(p.wallJumpSoftStaminaCost) => C_MovementContext.WallJumpSoftLeft,

            //Wall jump soft right
            C_MovementContext.RunningWallUpLeft when m.jump && !m.right && sm.IsEnoughStamina(p.wallJumpSoftStaminaCost) => C_MovementContext.WallJumpSoftRight,
            C_MovementContext.HangingLeft when m.jump && !m.right && sm.IsEnoughStamina(p.wallJumpSoftStaminaCost) => C_MovementContext.WallJumpSoftRight,
            C_MovementContext.SlidingWallDownLeft when m.jump && !m.right && sm.IsEnoughStamina(p.wallJumpSoftStaminaCost) => C_MovementContext.WallJumpSoftRight,
            C_MovementContext.ForcedSlidingWallDownLeft when m.jump && !m.right && sm.IsEnoughStamina(p.wallJumpSoftStaminaCost) => C_MovementContext.WallJumpSoftRight,

            //Wall jump hard left
            C_MovementContext.RunningWallUpRight when m.jump && m.left && sm.IsEnoughStamina(p.wallJumpHardStaminaCost) => C_MovementContext.WallJumpHardLeft,
            C_MovementContext.HangingRight when m.jump && m.left && sm.IsEnoughStamina(p.wallJumpHardStaminaCost) => C_MovementContext.WallJumpHardLeft,
            C_MovementContext.SlidingWallDownRight when m.jump && m.left && sm.IsEnoughStamina(p.wallJumpHardStaminaCost) => C_MovementContext.WallJumpHardLeft,
            C_MovementContext.ForcedSlidingWallDownRight when m.jump && m.left && sm.IsEnoughStamina(p.wallJumpHardStaminaCost) => C_MovementContext.WallJumpHardLeft,

            //Wall jump hard right
            C_MovementContext.RunningWallUpLeft when m.jump && m.right && sm.IsEnoughStamina(p.wallJumpHardStaminaCost) => C_MovementContext.WallJumpHardRight,
            C_MovementContext.HangingLeft when m.jump && m.right && sm.IsEnoughStamina(p.wallJumpHardStaminaCost) => C_MovementContext.WallJumpHardRight,
            C_MovementContext.SlidingWallDownLeft when m.jump && m.right && sm.IsEnoughStamina(p.wallJumpHardStaminaCost) => C_MovementContext.WallJumpHardRight,
            C_MovementContext.ForcedSlidingWallDownLeft when m.jump && m.right && sm.IsEnoughStamina(p.wallJumpHardStaminaCost) => C_MovementContext.WallJumpHardRight,

            //Hanging left
            C_MovementContext.RunningWallUpLeft when g.groundTopLeft && !g.groundAboveLeft0_1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingLeft,
            C_MovementContext.Falling when g.groundOnLeft && g.groundTopLeft && !g.groundAboveLeft1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingLeft,
            C_MovementContext.Falling when !g.groundBottomLeft && g.groundTopLeft && !g.groundAboveLeft0_1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingLeft,
            C_MovementContext.Jumping when !g.groundBottomLeft && g.groundTopLeft && !g.groundAboveLeft0_1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingLeft,
            C_MovementContext.Jumping when g.groundBottomLeft && g.groundTopLeft && !g.groundAboveLeft0_1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingLeft,
            C_MovementContext.CliffHangRight when a.cliffHangCoroutine == null && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingLeft,
            C_MovementContext.WallJumpHardLeft when !g.groundBottomLeft && g.groundTopLeft && !g.groundAboveLeft0_1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingLeft,

            //Hanging right
            C_MovementContext.RunningWallUpRight when g.groundTopRight && !g.groundAboveRight0_1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingRight,
            C_MovementContext.Falling when g.groundOnRight && g.groundTopRight && !g.groundAboveRight1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingRight,
            C_MovementContext.Falling when !g.groundBottomRight && g.groundTopRight && !g.groundAboveRight0_1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingRight,
            C_MovementContext.Jumping when !g.groundBottomRight && g.groundTopRight && !g.groundAboveRight0_1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingRight,
            C_MovementContext.Jumping when g.groundBottomRight && g.groundTopRight && !g.groundAboveRight0_1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingRight,
            C_MovementContext.CliffHangLeft when a.cliffHangCoroutine == null && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingRight,
            C_MovementContext.WallJumpHardRight when !g.groundBottomRight && g.groundTopRight && !g.groundAboveRight0_1f && sm.IsEnoughStamina(p.staticStaminaCost * Time.deltaTime) => C_MovementContext.HangingRight,

            //Climbing up left
            C_MovementContext.HangingLeft when m.up && sm.IsEnoughStamina(p.climbingStaminaCost) => C_MovementContext.ClimbingUpLeft,
            C_MovementContext.SprintingLeft when g.groundOnLeft && m.left && !g.groundAboveLeft0_1f && g.groundTopLeft && sm.IsEnoughStamina(p.climbingStaminaCost) => C_MovementContext.ClimbingUpLeft,
            C_MovementContext.RunningLeft when g.groundOnLeft && m.left && !g.groundAboveLeft0_1f && g.groundTopLeft && sm.IsEnoughStamina(p.climbingStaminaCost) => C_MovementContext.ClimbingUpLeft,
            C_MovementContext.Idling when g.groundOnLeft && m.left && !g.groundAboveLeft0_1f && g.groundTopLeft && sm.IsEnoughStamina(p.climbingStaminaCost) => C_MovementContext.ClimbingUpLeft,

            //Climbing up right
            C_MovementContext.HangingRight when m.up && sm.IsEnoughStamina(p.climbingStaminaCost) => C_MovementContext.ClimbingUpRight,
            C_MovementContext.SprintingRight when g.groundOnRight && m.right && !g.groundAboveRight0_1f && g.groundTopRight && sm.IsEnoughStamina(p.climbingStaminaCost) => C_MovementContext.ClimbingUpRight,
            C_MovementContext.RunningRight when g.groundOnRight && m.right && !g.groundAboveRight0_1f && g.groundTopRight && sm.IsEnoughStamina(p.climbingStaminaCost) => C_MovementContext.ClimbingUpRight,
            C_MovementContext.Idling when g.groundOnRight && m.right && !g.groundAboveRight0_1f && g.groundTopRight && sm.IsEnoughStamina(p.climbingStaminaCost) => C_MovementContext.ClimbingUpRight,

            //Climbing down left
            C_MovementContext.SprintingLeft when m.left && !g.groundBeneathLeft1f && !g.groundBeneathLeft2f && g.groundBeneathLeft3f && m.down=> C_MovementContext.ClimbingDownLeft,
            C_MovementContext.RunningLeft when m.left && !g.groundBeneathLeft1f && !g.groundBeneathLeft2f && g.groundBeneathLeft3f && m.down=> C_MovementContext.ClimbingDownLeft,
            C_MovementContext.Idling when m.left && !g.groundBeneathLeft1f && !g.groundBeneathLeft2f && g.groundBeneathLeft3f && m.down => C_MovementContext.ClimbingDownLeft,

            //Climbing down right
            C_MovementContext.SprintingRight when m.right && !g.groundBeneathRight1f && !g.groundBeneathRight2f && g.groundBeneathRight3f && m.down => C_MovementContext.ClimbingDownRight,
            C_MovementContext.RunningRight when m.right && !g.groundBeneathRight1f && !g.groundBeneathRight2f && g.groundBeneathRight3f && m.down => C_MovementContext.ClimbingDownRight,
            C_MovementContext.Idling when m.right && !g.groundBeneathRight1f && !g.groundBeneathRight2f && g.groundBeneathRight3f && m.down => C_MovementContext.ClimbingDownRight,

            //Forced sliding down left
            C_MovementContext.HangingLeft when m.down => C_MovementContext.ForcedSlidingWallDownLeft,
            C_MovementContext.RunningWallUpLeft when m.down => C_MovementContext.ForcedSlidingWallDownLeft,

            //Forced sliding down right
            C_MovementContext.HangingRight when m.down => C_MovementContext.ForcedSlidingWallDownRight,
            C_MovementContext.RunningWallUpRight when m.down => C_MovementContext.ForcedSlidingWallDownRight,

            //Step left
            C_MovementContext.SprintingLeft when !g.groundTopLeft && g.groundBottomLeft => C_MovementContext.StepLeft,
            C_MovementContext.RunningLeft when !g.groundTopLeft && g.groundBottomLeft => C_MovementContext.StepLeft,
            C_MovementContext.Idling when !g.groundTopLeft && g.groundBottomLeft && m.left => C_MovementContext.StepLeft,

            //Step right
            C_MovementContext.SprintingRight when !g.groundTopRight && g.groundBottomRight => C_MovementContext.StepRight,
            C_MovementContext.RunningRight when !g.groundTopRight && g.groundBottomRight => C_MovementContext.StepRight,
            C_MovementContext.Idling when !g.groundTopRight && g.groundBottomRight && m.right => C_MovementContext.StepRight,
            
            //Step Down left
            C_MovementContext.SprintingLeft when !g.groundBeneathLeft1f && g.groundBeneathLeft2f && m.left && !g.groundBottomLeft && m.down => C_MovementContext.StepDownLeft,
            C_MovementContext.RunningLeft when !g.groundBeneathLeft1f && g.groundBeneathLeft2f && m.left && !g.groundBottomLeft && m.down => C_MovementContext.StepDownLeft,
            C_MovementContext.Idling when !g.groundBeneathLeft1f && g.groundBeneathLeft2f && m.left && !g.groundBottomLeft && m.down => C_MovementContext.StepDownLeft,

            //Step Down right
            C_MovementContext.SprintingRight when !g.groundBeneathRight1f && g.groundBeneathRight2f && m.right && !g.groundBottomRight && m.down => C_MovementContext.StepDownRight,
            C_MovementContext.RunningRight when !g.groundBeneathRight1f && g.groundBeneathRight2f && m.right && !g.groundBottomRight && m.down => C_MovementContext.StepDownRight,
            C_MovementContext.Idling when !g.groundBeneathRight1f && g.groundBeneathRight2f && m.right && !g.groundBottomRight && m.down => C_MovementContext.StepDownRight,

            //Cliff Hang left
            C_MovementContext.SprintingLeft when !g.groundBeneathLeft1f && !g.groundBeneathLeft2f && m.left && !g.groundBottomLeft && m.down => C_MovementContext.CliffHangLeft,
            C_MovementContext.RunningLeft when !g.groundBeneathLeft1f && !g.groundBeneathLeft2f && m.left && !g.groundBottomLeft && m.down => C_MovementContext.CliffHangLeft,
            C_MovementContext.Idling when !g.groundBeneathLeft1f && !g.groundBeneathLeft2f && m.left && !g.groundBottomLeft && m.down => C_MovementContext.CliffHangLeft,

            //Cliff Hang right
            C_MovementContext.SprintingRight when !g.groundBeneathRight1f && !g.groundBeneathRight2f && m.right && !g.groundBottomRight && m.down => C_MovementContext.CliffHangRight,
            C_MovementContext.RunningRight when !g.groundBeneathRight1f && !g.groundBeneathRight2f && m.right && !g.groundBottomRight && m.down => C_MovementContext.CliffHangRight,
            C_MovementContext.Idling when !g.groundBeneathRight1f && !g.groundBeneathRight2f && m.right && !g.groundBottomRight && m.down => C_MovementContext.CliffHangRight,

            _ => c.MovementContext,
        };
    }

    private C_MovementContext UpdateRopeContext(
        MovementData m,
        GroundData g,
        C_MovementParameters p,
        C_MovementActions a,
        Character c,
        Rigidbody2D rb,
        Rope r,
        C_StaminaManager sm

        )
    {
        return c.MovementContext switch
        {
            //Rope
            _ when r != null => C_MovementContext.Rope,
            //C_MovementContext.RunningRight when r != null => C_MovementContext.Rope,
            //C_MovementContext.SprintingLeft when r != null => C_MovementContext.Rope,
            //C_MovementContext.SprintingRight when r != null => C_MovementContext.Rope,
            //C_MovementContext.Jumping when r != null => C_MovementContext.Rope,
            //C_MovementContext.Falling when r != null => C_MovementContext.Rope,
            //C_MovementContext.WallJumpHardLeft when r != null => C_MovementContext.Rope,
            //C_MovementContext.WallJumpHardRight when r != null => C_MovementContext.Rope,
            //C_MovementContext.WallJumpSoftLeft when r != null => C_MovementContext.Rope,
            //C_MovementContext.WallJumpSoftRight when r != null => C_MovementContext.Rope,
            //C_MovementContext.CliffHangLeft when r != null => C_MovementContext.Rope,
            //C_MovementContext.CliffHangRight when r != null => C_MovementContext.Rope,
            //C_MovementContext.HangingLeft when r != null => C_MovementContext.Rope,
            //C_MovementContext.HangingRight when r != null => C_MovementContext.Rope,

            _ => c.MovementContext,
        };
    }
}
