using UnityEngine;
[RequireComponent(typeof(C_InputController))]
[RequireComponent(typeof(C_MovementParameters))]
[RequireComponent(typeof(C_MovementDataCollector))]
[RequireComponent(typeof(C_MovementContextCreator))]
[RequireComponent(typeof(C_MovementContextResolver))]
[RequireComponent(typeof(C_MovementActions))]
[RequireComponent(typeof(C_InputResolver))]
[RequireComponent(typeof(C_Actions))]
public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public C_InputController inputController;
    [SerializeField] public C_MovementParameters movementParameters;
    [SerializeField] public C_MovementDataCollector movementDataCollector;
    [SerializeField] public C_MovementContextCreator movementContextCreator;
    [SerializeField] public C_MovementContextResolver movementContextResolver;
    [SerializeField] public C_MovementActions movementActions;
    [SerializeField] public C_InputResolver inputResolver;
    [SerializeField] public C_Actions actions;
    [SerializeField] public Rigidbody2D _rigidbody;
    [SerializeField] public CapsuleCollider2D _collider;

    [Header("Contexts")]
    [SerializeField] private C_MovementContext movementContext;
    [SerializeField] public GroundData groundData;
    [SerializeField] public MovementData movementData;
    [SerializeField] public Rope rope;

    public C_MovementContext MovementContext
    {
        get => movementContext;
        set
        {
            if (movementContext == value) return;
            Debug.Log($"Switching context from {movementContext} to {value}");
            movementContext = value;
        }
    }

    private void Reset()
    {
        inputController = GetComponent<C_InputController>();
        movementParameters = GetComponent<C_MovementParameters>();
        movementDataCollector = GetComponent<C_MovementDataCollector>();
        movementContextCreator = GetComponent<C_MovementContextCreator>();
        movementContextResolver = GetComponent<C_MovementContextResolver>();
        movementActions = GetComponent<C_MovementActions>();
        inputResolver = GetComponent<C_InputResolver>();
        actions = GetComponent<C_Actions>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        movementData = movementDataCollector.UpdateMovementData(inputController, _rigidbody);
        groundData = movementDataCollector.UpdateGroundData(_collider, movementParameters);

        MovementContext = movementContextCreator.UpdateMovementContext(
            movementData, 
            groundData, 
            movementParameters,
            movementActions,
            this,
            _rigidbody,
            rope
            );

        movementContextResolver.ResolveMovementContext(
            this,
            movementActions,
            movementParameters,
            _rigidbody,
            _collider
            );

        inputResolver.ResolveInput(inputController, actions);
    }
}
