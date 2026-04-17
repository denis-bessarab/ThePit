using UnityEngine;

public class Danger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Character")
        {
            var c = collision.GetComponent<Character>();
            var lc = c.lifeCycle;
            lc.Death(c);
        }
    }
}
