using UnityEngine;

[RequireComponent(typeof(E_MovementSystem))]
public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private E_MovementSystem ems;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private CapsuleCollider2D _collider2D;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;

    [Header("Parameters")]
    [SerializeField] private Weapon weapon;

    [Header("Data")]
    [SerializeField] private Transform target;
    [SerializeField] private bool isAttacking;
    [SerializeField] public float distanceToTarget;
    [SerializeField] public bool isTargetVisible;
    [SerializeField] public bool isOnWeaponDistance;
    [SerializeField] public bool isMoving;

    public enum EnemyState
    {
        Moving,
        Clinch
    }

    public Transform Target
    {
        get => target;
        set
        {
            target = value;

            if (ems.calculatePathCoroutine != null)
            {
                ems.EMSStop(true);
            }

            if(target != null)
            {
                ems.EMSStart();
            }
        }
    }

    private void Update()
    {
        UpdateData();
    }

    private void UpdateData()
    {
        distanceToTarget = DistanceToTarget(Target);
        isTargetVisible = IsTargetVisible();
        isOnWeaponDistance = IsOnWeaponDistance();

        if(weapon.name == "Fist" && distanceToTarget < weapon.distanceOfUse)
        {
            Clinch();
        }
    }

    private void UpdateContext()
    {

    }

    private void ResolveContext()
    {

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
            //Target = null;
        }
    }

    private bool IsTargetVisible()
    {
        if(Target  == null) return false;
        var bounds = _collider2D.bounds;
        var dir = (Target.position - transform.position).normalized;
        return !Physics2D.Raycast(bounds.center, dir, distanceToTarget, groundLayer);
    }

    private float DistanceToTarget(Transform target)
    {
        if (target != null)
        {
            return Vector2.Distance(transform.position, target.position);
        }
        else
        {
            return -1;
        }
    }

    private bool IsOnWeaponDistance()
    {
        return distanceToTarget <= weapon.distanceOfUse;
    }

    public void ResetAfterAttack()
    {
        Debug.Log("Reset after attack");
    }

    public void Clinch()
    {
        if(Target == null) return;
        Target.GetComponent<Character>().Clinch();
    }

    public void ExitClinch()
    {
        if (Target == null) return;
        Target.GetComponent<Character>().ExitClinch();
    }
}
