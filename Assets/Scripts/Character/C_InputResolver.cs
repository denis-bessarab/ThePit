using UnityEngine;

public class C_InputResolver : MonoBehaviour
{
    public void ResolveInput(
        C_InputController ic,
        C_Actions a,
        Character c,
        Rigidbody2D rb,
        MovementData m,
        C_MovementParameters p,
        C_LifeCycle lc,
        PGS_Inventory i,
        QuickAccessBar qab
        )
    {

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
