using UnityEngine;

public class C_InputResolver : MonoBehaviour
{
    public void ResolveInput(C_InputController ic, C_Actions a)
    {
        if (ic.rope.IsPressed()) a.RopeLoad();
        if (ic.rope.WasReleasedThisFrame()) a.RopeRelease();
        if (ic.restart.WasPressedThisFrame()) a.Restart();
    }
}
