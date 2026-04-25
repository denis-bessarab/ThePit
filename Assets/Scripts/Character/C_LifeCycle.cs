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
        var sm = GetComponent<C_StaminaManager>();
        var rb = c._rigidbody;
        deathCoroutine = StartCoroutine(DeathCoroutine(c, rb, sm));
    }

    public void DeathByFalling()
    {
        if (deathCoroutine != null) return;
        var c = GetComponent<Character>();
        var sm = GetComponent<C_StaminaManager>();
        var rb = c._rigidbody;
        deathCoroutine = StartCoroutine(DeathCoroutine(c, rb, sm));
    }

    private IEnumerator DeathCoroutine(Character c, Rigidbody2D rb, C_StaminaManager sm)
    {
        c.enabled = false;
        c.rope = null;
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.5f);
        c.gameObject.transform.position = resurrectionPosition;
        yield return new WaitForSeconds(0.5f);
        c.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        sm.staminaSpent = 0;
        sm.TriggerStaminaRegeneration();
        deathCoroutine = null;
    }

}
