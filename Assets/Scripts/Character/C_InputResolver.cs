using UnityEngine;

public class C_InputResolver : MonoBehaviour
{
    public void ResolveInput(C_InputController ic, C_Actions a, Character c, Rigidbody2D rb, MovementData m, C_MovementParameters p)
    {
        if (ic.rope.IsPressed()) a.RopeLoad();
        if (ic.rope.WasReleasedThisFrame()) a.RopeRelease();
        if (ic.restart.WasPressedThisFrame()) a.Restart();
        if (
            c.MovementContext == C_MovementContext.Falling || 
            c.MovementContext == C_MovementContext.Jumping ||
            c.MovementContext == C_MovementContext.WallJumpBackwardLeft ||
            c.MovementContext == C_MovementContext.WallJumpForwardLeft ||
            c.MovementContext == C_MovementContext.WallJumpBackwardRight ||
            c.MovementContext == C_MovementContext.WallJumpForwardRight
            ) a.AirPositionAdjustment(m, rb, p);
    }
}
