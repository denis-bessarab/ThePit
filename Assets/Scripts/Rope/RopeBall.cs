using UnityEngine;

public class RopeBall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            SpawnRope(collision.GetContact(0).point);
        }
    }

    private void SpawnRope(Vector2 point)
    {
        var rope = Instantiate(Resources.Load("Prefabs/Rope/Rope") as GameObject);
        rope.transform.position = point;
        Destroy(gameObject);
    }
}
