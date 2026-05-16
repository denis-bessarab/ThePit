using System.Collections;
using UnityEngine;

public class C_Actions : MonoBehaviour
{
    //[Header("Parameters")]
    //[SerializeField] private float ropePower;
    
    //public Coroutine ropeLoadCoroutine;

    //public void RopeLoad()
    //{
    //    ropeLoadCoroutine ??= StartCoroutine(RopeLoadCoroutine());
    //}

    //private IEnumerator RopeLoadCoroutine()
    //{
    //    ropePower = 1;

    //    while (ropePower < 3)
    //    {
    //        ropePower += Time.deltaTime;
    //        yield return null;
    //    }
    //}

    //public void RopeRelease()
    //{
    //    if(ropeLoadCoroutine != null)
    //    {
    //        StopCoroutine(ropeLoadCoroutine);
    //        ropeLoadCoroutine = null;
    //        if (ropePower < 1.5) return;
    //    }

    //    var mousePos = C_Utility.GetMousePosition();
    //    var dir = C_Utility.GetDirectionToPointer(transform.position, mousePos);

    //    var ropeBall = Instantiate(Resources.Load("Prefabs/Rope/RopeBall") as GameObject);
    //    ropeBall.transform.position = transform.position;
    //    var rb = ropeBall.GetComponent<Rigidbody2D>();
    //    var force = 10 * ropePower * dir;
    //    rb.AddForceAtPosition(force, ropeBall.transform.position, ForceMode2D.Impulse);
    //}

    public void Restart(C_LifeCycle lc, C_InputController ic, Character c)
    {
        var startPosition = lc.resurrectionPosition;
        transform.position = startPosition;
        ic.enabled = false;
        ic.enabled = true;
        c.MovementContext = C_MovementContext.Idling;
        c.rope = null;
    }

    public void AirPositionAdjustment(MovementData m, Rigidbody2D rb, C_MovementParameters p)
    {
        if(m.left && m.vx > 0) rb.AddForceAtPosition(new Vector2(-p.airPositionAdjustmentPower,0), transform.position, ForceMode2D.Force);
        if(m.right && m.vx < 0) rb.AddForceAtPosition(new Vector2(p.airPositionAdjustmentPower, 0), transform.position, ForceMode2D.Force);
    }

    public void OpenCloseInventory(PGS_Inventory i)
    {
        if(i.IsInventoryOpen) i.CloseInventory();
        else i.OpenInventory();
    }
}
