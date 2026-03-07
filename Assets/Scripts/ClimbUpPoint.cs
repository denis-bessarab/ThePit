using UnityEngine;

public class ClimbUpPoint : MonoBehaviour
{
    [SerializeField] private Transform destinationPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Character")
        {
            var ms = collision.GetComponent<MovementSystem>();
            var rb = collision.GetComponent<Rigidbody2D>();
            ms.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            //MoveCharacter(collision.gameObject);
        }
    }

    private void MoveCharacter(GameObject character)
    {
        character.transform.position = destinationPoint.position;
    }
}
