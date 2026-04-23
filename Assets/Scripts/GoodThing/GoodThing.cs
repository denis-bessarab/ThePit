using UnityEngine;

public class GoodThing : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Character")
        {
            var c = collision.GetComponent<C_Collector>();
            c.CollectGoodThing();
            var gtm = c.gameObject.GetComponent<GoodThingMeter>();
            gtm.RemoveGoodThing(transform.gameObject);
            Destroy(gameObject);
        }
    }
}
