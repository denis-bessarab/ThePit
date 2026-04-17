using System.Collections;
using UnityEngine;

public class C_LifeCycle : MonoBehaviour
{
    public Coroutine deathCoroutine;

    public void Death(Character c)
    {
        deathCoroutine = StartCoroutine(DeathCoroutine(c));
    }

    private IEnumerator DeathCoroutine(Character c)
    {
        c.enabled = false;
        yield return new WaitForSeconds(0.5f);
        c.gameObject.transform.position = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.5f);
        c.enabled = true;
    }
}
