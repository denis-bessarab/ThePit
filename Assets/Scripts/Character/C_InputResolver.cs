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
        C_UsableItemsController uic
        )
    {
        if (ic.lmbAction.WasPressedThisFrame()) uic.LMBAction();
        if (ic.rmbAction.WasPressedThisFrame()) uic.RMBAction();
        if (ic.lmbAction.IsPressed()) uic.LMBHoldAction();
        if (ic.rmbAction.IsPressed()) uic.RMBHoldAction();
        if (ic.lmbAction.WasReleasedThisFrame()) uic.LMBReleaseAction();
        if (ic.rmbAction.WasReleasedThisFrame()) uic.RMBReleaseAction();
        if (ic.restart.WasPressedThisFrame()) a.Restart(lc, ic, c);
        if (ic.inventory.WasPressedThisFrame()) a.OpenCloseInventory(i);
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
