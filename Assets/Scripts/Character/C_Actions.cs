using System.Collections;
using UnityEngine;

public class C_Actions : MonoBehaviour
{
    [SerializeField] private float ropePower;
    
    public Coroutine ropeLoadCoroutine;

    public void RopeLoad()
    {
        ropeLoadCoroutine ??= StartCoroutine(RopeLoadCoroutine());
    }

    private IEnumerator RopeLoadCoroutine()
    {
        ropePower = 1;

        while (ropePower < 3)
        {
            ropePower += Time.deltaTime;
            yield return null;
        }
    }

    public void RopeRelease()
    {
        if(ropeLoadCoroutine != null)
        {
            StopCoroutine(ropeLoadCoroutine);
            ropeLoadCoroutine = null;
            if (ropePower < 1.5) return;
        }

        var mousePos = C_Utility.GetMousePosition();
        var dir = C_Utility.GetDirectionToPointer(transform.position, mousePos);

        var ropeBall = Instantiate(Resources.Load("Prefabs/Rope/RopeBall") as GameObject);
        ropeBall.transform.position = transform.position;
        var rb = ropeBall.GetComponent<Rigidbody2D>();
        var force = dir * 10 * ropePower;
        rb.AddForceAtPosition(force, ropeBall.transform.position, ForceMode2D.Impulse);
    }

    public void Restart()
    {
        var startPosition = GetComponent<C_LifeCycle>().resurrectionPosition;
        transform.position = startPosition;
        var ic = GetComponent<C_InputController>();
        ic.enabled = false;
        ic.enabled = true;
        var c = GetComponent<Character>();
        c.MovementContext = C_MovementContext.Idling;
        c.rope = null;
    }

    public void AirPositionAdjustment(MovementData m, Rigidbody2D rb, C_MovementParameters p)
    {
        if(m.left && m.vx > 0) rb.AddForceAtPosition(new Vector2(-p.airPositionAdjustmentPower,0), transform.position, ForceMode2D.Force);
        if(m.right && m.vx < 0) rb.AddForceAtPosition(new Vector2(p.airPositionAdjustmentPower, 0), transform.position, ForceMode2D.Force);
    }
}
