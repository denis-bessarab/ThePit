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
        if (ic.lmbAction.WasPressedThisFrame()) qab.LMBAction();
        if (ic.rmbAction.WasPressedThisFrame()) qab.RMBAction();
        if (ic.lmbAction.IsPressed()) qab.LMBHoldAction();
        if (ic.rmbAction.IsPressed()) qab.RMBHoldAction();
        if (ic.lmbAction.WasReleasedThisFrame()) qab.LMBReleaseAction();
        if (ic.rmbAction.WasReleasedThisFrame()) qab.RMBReleaseAction();
        if (ic.restart.WasPressedThisFrame()) a.Restart(lc, ic, c);
        if (ic.qab_1.WasPressedThisFrame()) qab.SetActiveCell(0);
        if (ic.qab_2.WasPressedThisFrame()) qab.SetActiveCell(1);
        if (ic.qab_3.WasPressedThisFrame()) qab.SetActiveCell(2);
        if (ic.qab_4.WasPressedThisFrame()) qab.SetActiveCell(3);
        if (ic.qab_5.WasPressedThisFrame()) qab.SetActiveCell(4);
        if (ic.qab_6.WasPressedThisFrame()) qab.SetActiveCell(5);
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
