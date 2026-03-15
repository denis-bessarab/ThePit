using UnityEngine;

[RequireComponent(typeof(EnemyMovementSystem))]
public class Enemy : MonoBehaviour
{
    private Transform target;

    [SerializeField] private EnemyMovementSystem ems;

    public Transform Target
    {
        get => target;
        set
        {
            target = value;
            if (target == null) ems.Stop();
            ems.Path = ems.CalculatePath();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            Target = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            Target = null;
        }
    }
}
