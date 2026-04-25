using UnityEngine;

public class Danger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Character")
        {
            var lc = collision.GetComponent<C_LifeCycle>();
            lc.DeathByDanger();
        }
    }
}
