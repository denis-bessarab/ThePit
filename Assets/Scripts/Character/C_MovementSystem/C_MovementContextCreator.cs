using UnityEngine;

public class C_MovementContextCreator : MonoBehaviour
{
    public C_MovementContext UpdateMovementContext(MovementData m, GroundData g, C_MovementParameters p, C_MovementActions a, Character c, Rigidbody2D rb)
    {
        var vx = rb.linearVelocityX;
        var vy = rb.linearVelocityY;

        return c.MovementContext switch
        {
            //Idling
            C_MovementContext.RunningLeft when !m.left => C_MovementContext.Idling,
            C_MovementContext.SprintingLeft when !m.left => C_MovementContext.Idling,
            C_MovementContext.RunningRight when !m.right => C_MovementContext.Idling,
            C_MovementContext.SprintingRight when !m.right => C_MovementContext.Idling,
            C_MovementContext.Falling when g.groundBelow => C_MovementContext.Idling,
            C_MovementContext.SlidingLeft when g.groundBelow && a.slidingCoroutine == null => C_MovementContext.Idling,
            C_MovementContext.SlidingRight when g.groundBelow && a.slidingCoroutine == null => C_MovementContext.Idling,
            C_MovementContext.SlidingWallDownLeft when g.groundBelow => C_MovementContext.Idling,
            C_MovementContext.SlidingWallDownRight when g.groundBelow => C_MovementContext.Idling,
            C_MovementContext.HangingLeft when g.groundBelow && m.down => C_MovementContext.Idling,
            C_MovementContext.HangingRight when g.groundBelow && m.down => C_MovementContext.Idling,
            C_MovementContext.ClimbingUpLeft when a.climbingUpCoroutine == null => C_MovementContext.Idling,
            C_MovementContext.ClimbingUpRight when a.climbingUpCoroutine == null => C_MovementContext.Idling,

            //Sprinting Left
            C_MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft => C_MovementContext.SprintingLeft,
            C_MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft => C_MovementContext.SprintingLeft,
            C_MovementContext.SlidingLeft when g.groundBelow && a.slidingCoroutine == null && m.left => C_MovementContext.SprintingLeft,
            C_MovementContext.SlidingLeft when g.groundBelow && a.slidingCoroutine == null && m.left => C_MovementContext.SprintingLeft,
            C_MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft => C_MovementContext.SprintingLeft,
            C_MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft => C_MovementContext.SprintingLeft,
            C_MovementContext.HangingRight when g.groundBelow && m.left => C_MovementContext.SprintingLeft,
            C_MovementContext.WallJumpForwardLeft when g.groundBelow && m.left => C_MovementContext.SprintingLeft,

            //Sprinting Right
            C_MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight => C_MovementContext.SprintingRight,
            C_MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight => C_MovementContext.SprintingRight,
            C_MovementContext.SlidingRight when g.groundBelow && a.slidingCoroutine == null && m.right => C_MovementContext.SprintingRight,
            C_MovementContext.SlidingRight when g.groundBelow && a.slidingCoroutine == null && m.right => C_MovementContext.SprintingRight,
            C_MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight => C_MovementContext.SprintingRight,
            C_MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight => C_MovementContext.SprintingRight,
            C_MovementContext.HangingLeft when g.groundBelow && m.right => C_MovementContext.SprintingRight,
            C_MovementContext.WallJumpForwardRight when g.groundBelow && m.right => C_MovementContext.SprintingRight,

            //Jumping
            C_MovementContext.Idling when g.groundBelow && m.jump => C_MovementContext.Jumping,
            C_MovementContext.RunningLeft when m.jump => C_MovementContext.Jumping,
            C_MovementContext.RunningRight when m.jump => C_MovementContext.Jumping,
            C_MovementContext.SprintingLeft when m.jump => C_MovementContext.Jumping,
            C_MovementContext.SprintingRight when m.jump => C_MovementContext.Jumping,

            //Falling
            C_MovementContext.Idling when !g.groundBelow => C_MovementContext.Falling,
            C_MovementContext.Jumping when vy <= 0 && (vx == 0 || (!g.groundOnLeft && !g.groundOnRight)) => C_MovementContext.Falling,
            C_MovementContext.SlidingLeft when !g.groundBelow && a.slidingCoroutine == null => C_MovementContext.Falling,
            C_MovementContext.SlidingRight when !g.groundBelow && a.slidingCoroutine == null => C_MovementContext.Falling,
            C_MovementContext.WallJumpBackwardLeft when vy <= 0 => C_MovementContext.Falling,
            C_MovementContext.WallJumpBackwardRight when vy <= 0 => C_MovementContext.Falling,
            C_MovementContext.WallJumpForwardLeft when vy <= 0 && !g.groundOnLeft => C_MovementContext.Falling,
            C_MovementContext.WallJumpForwardRight when vy <= 0 && !g.groundOnRight => C_MovementContext.Falling,
            C_MovementContext.SprintingLeft when !g.groundBelow => C_MovementContext.Falling,
            C_MovementContext.SprintingRight when !g.groundBelow => C_MovementContext.Falling,
            C_MovementContext.SlidingWallDownLeft when !g.groundOnLeft => C_MovementContext.Falling,
            C_MovementContext.SlidingWallDownRight when !g.groundOnRight => C_MovementContext.Falling,

            //Sliding
            C_MovementContext.SprintingLeft when m.down => C_MovementContext.SlidingLeft,
            C_MovementContext.SprintingRight when m.down => C_MovementContext.SlidingRight,

            //Running wall up left
            C_MovementContext.Jumping when !g.groundBelow && m.left && g.groundOnLeft && g.groundAboveLeft1f => C_MovementContext.RunningWallUpLeft,
            C_MovementContext.WallJumpForwardLeft when g.groundOnLeft => C_MovementContext.RunningWallUpLeft,

            //Running wall up right
            C_MovementContext.Jumping when !g.groundBelow && m.right && g.groundOnRight && g.groundAboveRight1f => C_MovementContext.RunningWallUpRight,
            C_MovementContext.WallJumpForwardRight when g.groundOnRight => C_MovementContext.RunningWallUpRight,

            //Sliding wall down left
            C_MovementContext.HangingOnWallLeft when a.hangingOnWallCoroutine == null => C_MovementContext.SlidingWallDownLeft,
            C_MovementContext.RunningWallUpLeft when a.runningWallUpCoroutine == null => C_MovementContext.SlidingWallDownLeft,
            C_MovementContext.Falling when g.groundOnLeft => C_MovementContext.SlidingWallDownLeft,
            C_MovementContext.ForcedSlidingWallDownLeft when g.groundOnLeft && a.forcedSlidingWallDownCoroutine == null => C_MovementContext.SlidingWallDownLeft,

            //Sliding wall down right
            C_MovementContext.HangingOnWallRight when a.hangingOnWallCoroutine == null => C_MovementContext.SlidingWallDownRight,
            C_MovementContext.RunningWallUpRight when a.runningWallUpCoroutine == null => C_MovementContext.SlidingWallDownRight,
            C_MovementContext.Falling when g.groundOnRight => C_MovementContext.SlidingWallDownRight,
            C_MovementContext.ForcedSlidingWallDownRight when g.groundOnRight && a.forcedSlidingWallDownCoroutine == null => C_MovementContext.SlidingWallDownRight,

            //Wall jump backward left
            C_MovementContext.RunningWallUpRight when m.jump && !m.left => C_MovementContext.WallJumpBackwardLeft,

            //Wall jump backward right
            C_MovementContext.RunningWallUpLeft when m.jump && !m.right => C_MovementContext.WallJumpBackwardRight,

            //Wall jump forward left
            C_MovementContext.RunningWallUpRight when m.jump && m.left => C_MovementContext.WallJumpForwardLeft,
            C_MovementContext.HangingRight when m.jump && m.left => C_MovementContext.WallJumpForwardLeft,

            //Wall jump forward right
            C_MovementContext.RunningWallUpLeft when m.jump && m.right => C_MovementContext.WallJumpForwardRight,
            C_MovementContext.HangingLeft when m.jump && m.right => C_MovementContext.WallJumpForwardRight,

            //Hanging left
            C_MovementContext.Idling when g.groundOnLeft && g.groundAboveLeft && !g.groundAboveLeft0_2f && m.left => C_MovementContext.HangingLeft,
            C_MovementContext.RunningLeft when g.groundOnLeft && !g.groundAboveLeft1f && m.left => C_MovementContext.HangingLeft,
            C_MovementContext.SprintingLeft when g.groundOnLeft && !g.groundAboveLeft1f && m.left => C_MovementContext.HangingLeft,
            C_MovementContext.RunningWallUpLeft when g.groundAboveLeft && !g.groundAboveLeft0_1f => C_MovementContext.HangingLeft,

            //Hanging right
            C_MovementContext.Idling when g.groundOnRight && !g.groundAboveRight1f && m.right => C_MovementContext.HangingRight,
            C_MovementContext.RunningRight when g.groundOnRight && !g.groundAboveRight1f && m.right => C_MovementContext.HangingRight,
            C_MovementContext.SprintingRight when g.groundOnRight && !g.groundAboveRight1f && m.right => C_MovementContext.HangingRight,
            C_MovementContext.RunningWallUpRight when g.groundAboveRight && !g.groundAboveRight0_1f => C_MovementContext.HangingRight,

            //Climbing up left
            C_MovementContext.HangingLeft when m.up => C_MovementContext.ClimbingUpLeft,
            C_MovementContext.HangingLeft when m.jump => C_MovementContext.ClimbingUpLeft,

            //Climbing up right
            C_MovementContext.HangingRight when m.up => C_MovementContext.ClimbingUpRight,
            C_MovementContext.HangingRight when m.jump => C_MovementContext.ClimbingUpRight,

            //Forced sliding down left
            C_MovementContext.HangingLeft when m.down => C_MovementContext.ForcedSlidingWallDownLeft,

            //Forced sliding down right
            C_MovementContext.HangingRight when m.down => C_MovementContext.ForcedSlidingWallDownRight,

            _ => c.MovementContext,
        };
    }
}
