using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementSystem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputActionAsset InputActions;
    [SerializeField] private CapsuleCollider2D Collider2D;
    [SerializeField] private Rigidbody2D RigidBody2D;
    [Header("Parameters")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float verticalRaycastDistance;
    [SerializeField] private float horizontalRaycastDistance;
    [SerializeField] private float jumpPower;
    [SerializeField] private float slidingPower;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float sprintingSpeed;

    private InputAction m_left;
    private InputAction m_right;
    private InputAction m_jump;
    private InputAction m_down;
    private InputAction m_up;
    private InputAction m_sprint;

    [SerializeField] private MovementData movementData;
    [SerializeField] private GroundData groundData;
    [SerializeField] private MovementContext movementContext = MovementContext.Idling;

    private bool isSliding;

    [Serializable]
    private struct MovementData
    {
        public bool left;
        public bool right;
        public bool jump;
        public bool down;
        public bool up;
        public bool sprint;

        public MovementData(bool left, bool right, bool  jump, bool down, bool up, bool sprint)
        {
            this.left = left;
            this.right = right;
            this.jump = jump;
            this.down = down;
            this.up = up;
            this.sprint = sprint;
        }
    }

    [Serializable]
    private struct GroundData
    {
        public bool groundOnLeft;
        public bool groundOnRight;
        public bool groundBelow;
        public bool groundAbove;

        public GroundData(bool groundOnLeft, bool groundOnRight,  bool groundBelow, bool groundAbove)
        {
            this.groundOnLeft = groundOnLeft;
            this.groundOnRight = groundOnRight;
            this.groundBelow = groundBelow;
            this.groundAbove = groundAbove;
        }
    }

    private enum MovementContext
    {
        Idling,
        RunningLeft,
        RunningRight,
        Jumping,
        SlidingLeft,
        SlidingRight,
        SprintingLeft,
        SprintingRight,
        Hanging,
        ClimbingUp,
        ClimbingDown,
        RunningWallUp,
        SlidingWallDown,
        JumpingFromWallFaceToWall,
        JumpingFromWallBackToWall,
        Falling
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Character").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Character").Disable();
    }

    private void Awake()
    {
        m_left = InputSystem.actions.FindAction("Left");
        m_right = InputSystem.actions.FindAction("Right");
        m_jump = InputSystem.actions.FindAction("Jump");
        m_down = InputSystem.actions.FindAction("Down");
        m_up = InputSystem.actions.FindAction("Up");
        m_sprint = InputSystem.actions.FindAction("Sprint");
    }

    private void Update()
    {
        UpdateMovementData();
        UpdateGroundData();
        HandleDirectInput();
        UpdateMovementContext();
        ResolveMovementContext();
    }

    private void UpdateMovementData()
    {
        var left = m_left.IsPressed();
        var right = m_right.IsPressed();
        var jump = m_jump.WasPressedThisFrame();
        var down = m_down.WasPressedThisFrame();
        var up = m_up.IsPressed();
        var sprint = m_sprint.IsPressed();

        movementData = new MovementData(left, right, jump, down, up, sprint);
    }

    private void UpdateGroundData()
    {
        var left = Physics2D.Raycast(Collider2D.bounds.center, Vector2.left, horizontalRaycastDistance, groundLayerMask);
        var right = Physics2D.Raycast(Collider2D.bounds.center, Vector2.right, horizontalRaycastDistance, groundLayerMask);
        var below = Physics2D.Raycast(Collider2D.bounds.center, Vector2.down, verticalRaycastDistance, groundLayerMask);
        var above = Physics2D.Raycast(Collider2D.bounds.center, Vector2.up, verticalRaycastDistance, groundLayerMask);

        groundData =  new GroundData(left, right, below, above);
    }

    private void UpdateMovementContext()
    {
        var m = movementData;
        var g = groundData;
        var vx = RigidBody2D.linearVelocityX;
        var vy = RigidBody2D.linearVelocityY;

        movementContext = movementContext switch
        {
            //Movement Left
            MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft => MovementContext.RunningLeft,
            MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft && m.sprint => MovementContext.SprintingLeft,
            MovementContext.RunningLeft when !m.left => MovementContext.Idling,
            MovementContext.RunningLeft when m.sprint => MovementContext.SprintingLeft,
            MovementContext.SprintingLeft when !m.sprint => MovementContext.RunningLeft,
            MovementContext.SprintingLeft when !m.left => MovementContext.Idling,
            //Movement Right
            MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight => MovementContext.RunningRight,
            MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight && m.sprint => MovementContext.SprintingRight,
            MovementContext.RunningRight when !m.right => MovementContext.Idling,
            MovementContext.RunningRight when m.sprint => MovementContext.SprintingRight,
            MovementContext.SprintingRight when !m.sprint => MovementContext.RunningRight,
            MovementContext.SprintingRight when !m.right => MovementContext.Idling,
            //Jumping and Falling
            MovementContext.Idling when g.groundBelow && m.jump => MovementContext.Jumping,
            MovementContext.RunningLeft when m.jump => MovementContext.Jumping,
            MovementContext.RunningRight when m.jump => MovementContext.Jumping,
            MovementContext.SprintingLeft when m.jump => MovementContext.Jumping,
            MovementContext.SprintingRight when m.jump => MovementContext.Jumping,
            MovementContext.Jumping when vy <= 0 => MovementContext.Falling,
            MovementContext.Falling when g.groundBelow => MovementContext.Idling,
            MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft => MovementContext.RunningLeft,
            MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight => MovementContext.RunningRight,
            MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft && m.sprint => MovementContext.SprintingLeft,
            MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight && m.sprint => MovementContext.SprintingRight,
            //Sliding
            MovementContext.SprintingLeft when m.down => MovementContext.SlidingLeft,
            MovementContext.SprintingRight when m.down => MovementContext.SlidingRight,
            MovementContext.SlidingLeft when !isSliding && g.groundBelow => MovementContext.Idling,
            MovementContext.SlidingRight when !isSliding && g.groundBelow => MovementContext.Idling,
            _ => movementContext,
        };
    }

    private void HandleDirectInput()
    {
        var m = movementData;
        var g = groundData;
        if (m.jump && g.groundBelow) Jump();
        if (m.down) Sliding();
    }

    private void ResolveMovementContext()
    {
        switch (movementContext)
        {
            case MovementContext.RunningLeft:
                RunLeft();
                break;
            case MovementContext.RunningRight:
                RunRight();
                break;
            case MovementContext.SprintingLeft:
                SprintLeft();
                break;
            case MovementContext.SprintingRight:
                SprintRight();
                break;
        }
    }
    private void RunLeft()
    {
        RigidBody2D.linearVelocityX = -runningSpeed;
        //RigidBody2D.AddForceAtPosition(new Vector3(-runningSpeed, 0), transform.position, ForceMode2D.Impulse);
    }

    private void RunRight()
    {
        RigidBody2D.linearVelocityX = runningSpeed;
        //RigidBody2D.AddForceAtPosition(new Vector3(runningSpeed, 0), transform.position, ForceMode2D.Impulse);
    }

    private void SprintLeft()
    {
        RigidBody2D.linearVelocityX = -sprintingSpeed;
        //RigidBody2D.AddForceAtPosition(new Vector3(-sprintingSpeed, 0), transform.position, ForceMode2D.Impulse);
    }

    private void SprintRight()
    {
        RigidBody2D.linearVelocityX = sprintingSpeed;
        //RigidBody2D.AddForceAtPosition(new Vector3(sprintingSpeed, 0), transform.position, ForceMode2D.Impulse);
    }

    private void Jump()
    {
        RigidBody2D.AddForceAtPosition(new Vector3(0, jumpPower), transform.position, ForceMode2D.Impulse);
    }

    private void Sliding()
    {
        if (movementContext == MovementContext.SprintingLeft) StartCoroutine(SlideLeft());
        if (movementContext == MovementContext.SprintingRight) StartCoroutine(SlideRight());
    }

    private IEnumerator SlideLeft()
    {
        isSliding = true;
        RigidBody2D.AddForceAtPosition(new Vector3(-slidingPower, 0), transform.position, ForceMode2D.Impulse);
        //REMOVE THIS WHEN GET ANIMATIONS AND MAKE THIS ANIMATION DRIVEN
        yield return new WaitForSeconds(1f);
        isSliding = false;
        //END OF REMOVE
    }

    private IEnumerator SlideRight()
    {
        isSliding = true;
        RigidBody2D.AddForceAtPosition(new Vector3(slidingPower, 0), transform.position, ForceMode2D.Impulse);
        //REMOVE THIS WHEN GET ANIMATIONS AND MAKE THIS ANIMATION DRIVEN
        yield return new WaitForSeconds(1f);
        isSliding = false;
        //END OF REMOVE
    }
}
