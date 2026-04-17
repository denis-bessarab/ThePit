using UnityEngine;

public class GoodThing : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var c = collision.GetComponent<C_Collector>();
        c.CollectGoodThing();
        Destroy(gameObject);
    }
}
