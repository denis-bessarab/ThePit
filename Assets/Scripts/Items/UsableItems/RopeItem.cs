using System.Collections;
using UnityEngine;
public class RopeItem : UsableItem
{
    [Header("Parameters")]
    [SerializeField] private float ropePower = 1;
    public Coroutine ropeLoadCoroutine;

    public override void LMBHoldAction()
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

    public override void LMBReleaseAction()
    {
        if (ropeLoadCoroutine != null)
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
        var force = 10 * ropePower * dir;
        rb.AddForceAtPosition(force, ropeBall.transform.position, ForceMode2D.Impulse);
    }
}
