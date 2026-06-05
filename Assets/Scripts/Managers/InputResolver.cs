using UnityEngine;

public class InputResolver : MonoBehaviour
{
    public void ResolveInput(InputManager im)
    {
        //GLOBAL ACTIONS
        if (im.pause.WasPressedThisFrame() && im.onPauseMenu.Count > 0) im.onPauseMenu[0].Invoke();
        if (im.restart.WasPressedThisFrame() && im.onRestart.Count > 0) im.onRestart[0].Invoke();

        //CHARACTER UI
        if(im.inventory.WasPressedThisFrame() && im.onInventory.Count > 0) im.onInventory[0].Invoke();

        //QAB
        if(im.qab_1.WasPressedThisFrame() && im.onQab1.Count > 0) im.onQab1[0].Invoke(0);
        if(im.qab_2.WasPressedThisFrame() && im.onQab2.Count > 0) im.onQab2[0].Invoke(1);
        if(im.qab_3.WasPressedThisFrame() && im.onQab3.Count > 0) im.onQab3[0].Invoke(2);
        if(im.qab_4.WasPressedThisFrame() && im.onQab4.Count > 0) im.onQab4[0].Invoke(3);
        if(im.qab_5.WasPressedThisFrame() && im.onQab5.Count > 0) im.onQab5[0].Invoke(4);
        if(im.qab_6.WasPressedThisFrame() && im.onQab6.Count > 0) im.onQab6[0].Invoke(5);

        //CHARACTER ACTIONS
        if (im.left.WasPressedThisFrame() && im.onLeft.Count > 0) im.onLeft[0].Invoke();
        if (im.left.IsPressed() && im.onLeftHold.Count > 0) im.onLeftHold[0].Invoke();
        if (im.left.WasReleasedThisFrame() && im.onLeftRelease.Count > 0) im.onLeftRelease[0].Invoke();

        if (im.right.WasPressedThisFrame() && im.onRight.Count > 0) im.onRight[0].Invoke();
        if (im.right.IsPressed() && im.onRightHold.Count > 0) im.onRightHold[0].Invoke();
        if (im.right.WasReleasedThisFrame() && im.onRightRelease.Count > 0) im.onRightRelease[0].Invoke();

        if (im.jump.WasPressedThisFrame() && im.onJump.Count > 0) im.onJump[0].Invoke();
        if (im.jump.IsPressed() && im.onJumpHold.Count > 0) im.onJumpHold[0].Invoke();
        if (im.jump.WasReleasedThisFrame() && im.onJumpRelease.Count > 0) im.onJumpRelease[0].Invoke();

        if (im.down.WasPressedThisFrame() && im.onDown.Count > 0) im.onDown[0].Invoke();
        if (im.down.IsPressed() && im.onDownHold.Count > 0) im.onDownHold[0].Invoke();
        if (im.down.WasReleasedThisFrame() && im.onDownRelease.Count > 0) im.onDownRelease[0].Invoke();

        if (im.up.WasPressedThisFrame() && im.onUp.Count > 0) im.onUp[0].Invoke();
        if (im.up.IsPressed() && im.onUpHold.Count > 0) im.onUpHold[0].Invoke();
        if (im.up.WasReleasedThisFrame() && im.onUpRelease.Count > 0) im.onUpRelease[0].Invoke();

        if (im.shift.WasPressedThisFrame() && im.onShift.Count > 0) im.onShift[0].Invoke();
        if (im.shift.IsPressed() && im.onShiftHold.Count > 0) im.onShiftHold[0].Invoke();
        if (im.shift.WasReleasedThisFrame() && im.onShiftRelease.Count > 0) im.onShiftRelease[0].Invoke();

        //MOUSE ACTIONS
        if (im.lmbAction.WasPressedThisFrame() && im.onLMB.Count > 0) im.onLMB[0].Invoke();
        if (im.rmbAction.WasPressedThisFrame() && im.onRMB.Count > 0) im.onRMB[0].Invoke();

        if (im.lmbAction.IsPressed() && im.onLMBHold.Count > 0) im.onLMBHold[0].Invoke();
        if (im.rmbAction.IsPressed() && im.onRMBHold.Count > 0) im.onRMBHold[0].Invoke();

        if (im.lmbAction.WasReleasedThisFrame() && im.onLMBRelease.Count > 0) im.onLMBRelease[0].Invoke();
        if (im.rmbAction.WasReleasedThisFrame() && im.onRMBRelease.Count > 0) im.onRMBRelease[0].Invoke();
    }
}
