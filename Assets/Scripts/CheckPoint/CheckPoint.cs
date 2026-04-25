using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Character")
        {
            var lc = collision.GetComponent<C_LifeCycle>();
            if (lc == null) return;

            lc.resurrectionPosition = transform.position;
        }
    }
}
