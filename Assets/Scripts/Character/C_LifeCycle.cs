using System.Collections;
using UnityEngine;

public class C_LifeCycle : MonoBehaviour
{
    public Vector3 resurrectionPosition = Vector3.zero;
    public Coroutine deathCoroutine;
    public void DeathByDanger()
    {
        if (deathCoroutine != null) return;
        var c = GetComponent<Character>();
        var rb = c._rigidbody;
        deathCoroutine = StartCoroutine(DeathCoroutine(c, rb));
    }

    public void DeathByFalling()
    {
        if (deathCoroutine != null) return;
        var c = GetComponent<Character>();
        var rb = c._rigidbody;
        deathCoroutine = StartCoroutine(DeathCoroutine(c, rb));
    }

    private IEnumerator DeathCoroutine(Character c, Rigidbody2D rb)
    {
        c.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.5f);
        c.gameObject.transform.position = resurrectionPosition;
        yield return new WaitForSeconds(0.5f);
        c.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        deathCoroutine = null;
    }

}
