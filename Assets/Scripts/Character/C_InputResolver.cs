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
            c.MovementContext == C_MovementContext.WallJumpSoftLeft ||
            c.MovementContext == C_MovementContext.WallJumpHardLeft ||
            c.MovementContext == C_MovementContext.WallJumpSoftRight ||
            c.MovementContext == C_MovementContext.WallJumpHardRight
            ) a.AirPositionAdjustment(m, rb, p);
    }
}
