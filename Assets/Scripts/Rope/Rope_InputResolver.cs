using UnityEngine;

public class Rope_InputResolver : MonoBehaviour
{
    public void ResolveInput(Rope_InputController ic, Rope_Actions a, Rope_Parameters p, DistanceJoint2D d, Rope r, C_Actions ca)
    {
        if (ic.up.IsPressed()) a.Up(p, d);
        if (ic.down.IsPressed()) a.Down(p, d);
        if (ic.swingLeft.WasPressedThisFrame()) a.SwingLeft(p, d);
        if (ic.swingRight.WasPressedThisFrame()) a.SwingRight(p, d);
        if (ic.jump.WasPressedThisFrame()) a.Jump(r);
        //if (ic.rope.IsPressed()) ca.RopeLoad();
        //if (ic.rope.WasReleasedThisFrame()) ca.RopeRelease();
    }
}
