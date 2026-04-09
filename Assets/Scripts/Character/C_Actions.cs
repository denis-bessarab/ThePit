using System.Collections;
using UnityEngine;

public class C_Actions : MonoBehaviour
{
    public Coroutine ropeLoadCoroutine;

    private float ropePower;
    public void RopeLoad()
    {
        ropeLoadCoroutine ??= StartCoroutine(RopeLoadCoroutine());
    }

    private IEnumerator RopeLoadCoroutine()
    {
        ropePower = 0;

        while (ropePower < 3)
        {
            Debug.Log($"Loading {ropePower}");
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
        }
        Debug.Log($"Release {ropePower}");
    }
}
