using System.Collections;
using UnityEngine;

public class C_LifeCycle : MonoBehaviour
{
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
        yield return new WaitForSeconds(0.5f);
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
        c.gameObject.transform.position = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.5f);
        c.enabled = true;
        deathCoroutine = null;
    }

}
