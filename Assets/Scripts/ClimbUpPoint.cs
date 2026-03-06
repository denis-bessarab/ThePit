using UnityEngine;

public class ClimbUpPoint : MonoBehaviour
{
    [SerializeField] private Transform destinationPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Character")
        {
            MoveCharacter(collision.gameObject);
        }
    }

    private void MoveCharacter(GameObject character)
    {
        character.transform.position = destinationPoint.position;
    }
}
