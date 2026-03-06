using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementSystem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputActionAsset _inputActions;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [Header("Parameters")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float verticalRaycastDistance;
    [SerializeField] private float horizontalRaycastDistance;
    [SerializeField] private float jumpPower;
    [SerializeField] private float slidingPower;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float sprintingSpeed;
    [SerializeField] private float hangingOnWallTime;
    [SerializeField] private float runningWallSpeed;
    [SerializeField] private float runningWallTime;

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
    private Coroutine hangingOnWallCoroutine;
    private Coroutine runningWallUpCoroutine;

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
        HangingOnWallLeft,
        HangingOnWallRight,
        RunningWallUpLeft,
        RunningWallUpRight,
        SlidingWallDownLeft,
        SlidingWallDownRight,
        JumpingFromWallFaceToWall,
        JumpingFromWallBackToWall,
        Falling
    }

    private void OnEnable()
    {
        _inputActions.FindActionMap("Character").Enable();
    }

    private void OnDisable()
    {
        _inputActions.FindActionMap("Character").Disable();
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
        var left = Physics2D.Raycast(_collider.bounds.center, Vector2.left, horizontalRaycastDistance, groundLayerMask);
        var right = Physics2D.Raycast(_collider.bounds.center, Vector2.right, horizontalRaycastDistance, groundLayerMask);
        var below = Physics2D.Raycast(_collider.bounds.center, Vector2.down, verticalRaycastDistance, groundLayerMask);
        var above = Physics2D.Raycast(_collider.bounds.center, Vector2.up, verticalRaycastDistance, groundLayerMask);

        groundData =  new GroundData(left, right, below, above);
    }

    private void UpdateMovementContext()
    {
        var m = movementData;
        var g = groundData;
        var vx = _rigidBody2D.linearVelocityX;
        var vy = _rigidBody2D.linearVelocityY;

        movementContext = movementContext switch
        {
            //Idling
            MovementContext.RunningLeft when !m.left => MovementContext.Idling,
            MovementContext.SprintingLeft when !m.left => MovementContext.Idling,
            MovementContext.RunningRight when !m.right => MovementContext.Idling,
            MovementContext.SprintingRight when !m.right => MovementContext.Idling,
            MovementContext.Falling when g.groundBelow => MovementContext.Idling,
            MovementContext.SlidingLeft when !isSliding && g.groundBelow => MovementContext.Idling,
            MovementContext.SlidingRight when !isSliding && g.groundBelow => MovementContext.Idling,
            MovementContext.SlidingWallDownLeft when g.groundBelow => MovementContext.Idling,
            MovementContext.SlidingWallDownRight when g.groundBelow => MovementContext.Idling,


            //Running Left
            MovementContext.SprintingLeft when !m.sprint => MovementContext.RunningLeft,
            MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft => MovementContext.RunningLeft,
            MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft => MovementContext.RunningLeft,

            //Running Right
            MovementContext.SprintingRight when !m.sprint => MovementContext.RunningRight,
            MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight => MovementContext.RunningRight,
            MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight => MovementContext.RunningRight,

            //Sprinting Left
            MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft && m.sprint => MovementContext.SprintingLeft,
            MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft && m.sprint => MovementContext.SprintingLeft,
            MovementContext.RunningLeft when m.sprint => MovementContext.SprintingLeft,

            //Sprinting Right
            MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight && m.sprint => MovementContext.SprintingRight,
            MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight && m.sprint => MovementContext.SprintingRight,
            MovementContext.RunningRight when m.sprint => MovementContext.SprintingRight,

            //Jumping
            MovementContext.Idling when g.groundBelow && m.jump => MovementContext.Jumping,
            MovementContext.RunningLeft when m.jump => MovementContext.Jumping,
            MovementContext.RunningRight when m.jump => MovementContext.Jumping,
            MovementContext.SprintingLeft when m.jump => MovementContext.Jumping,
            MovementContext.SprintingRight when m.jump => MovementContext.Jumping,
            
            //Falling
            MovementContext.Jumping when vy <= 0 => MovementContext.Falling,
            //MovementContext.RunningWallUpLeft when runningWallUpCoroutine == null => MovementContext.Falling,
            //MovementContext.RunningWallUpRight when runningWallUpCoroutine == null => MovementContext.Falling,

            //Sliding
            MovementContext.SprintingLeft when m.down => MovementContext.SlidingLeft,
            MovementContext.SprintingRight when m.down => MovementContext.SlidingRight,

            //Hanging on wall
            MovementContext.Jumping when !g.groundBelow && m.left && g.groundOnLeft && !m.sprint => MovementContext.HangingOnWallLeft,
            MovementContext.Jumping when !g.groundBelow && m.right && g.groundOnRight && !m.sprint=> MovementContext.HangingOnWallRight,

            //Running wall up
            MovementContext.HangingOnWallLeft when m.sprint && g.groundOnLeft => MovementContext.RunningWallUpLeft,
            MovementContext.HangingOnWallRight when m.sprint && g.groundOnRight => MovementContext.RunningWallUpRight,
            MovementContext.Jumping when !g.groundBelow && m.left && g.groundOnLeft && m.sprint => MovementContext.RunningWallUpLeft,
            MovementContext.Jumping when !g.groundBelow && m.right && g.groundOnRight && m.sprint => MovementContext.RunningWallUpRight,

            //Sliding wall down
            MovementContext.HangingOnWallLeft when hangingOnWallCoroutine == null => MovementContext.SlidingWallDownLeft,
            MovementContext.HangingOnWallRight when hangingOnWallCoroutine == null => MovementContext.SlidingWallDownRight,
            MovementContext.RunningWallUpLeft when runningWallUpCoroutine == null => MovementContext.SlidingWallDownLeft,
            MovementContext.RunningWallUpRight when runningWallUpCoroutine == null => MovementContext.SlidingWallDownRight,

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
            case MovementContext.HangingOnWallLeft:
                if (hangingOnWallCoroutine != null) return;
                _rigidBody2D.gravityScale = 0;
                hangingOnWallCoroutine = StartCoroutine(HangingOnWall());
                break;
            case MovementContext.HangingOnWallRight:
                if (hangingOnWallCoroutine != null) return;
                _rigidBody2D.gravityScale = 0;
                hangingOnWallCoroutine = StartCoroutine(HangingOnWall());
                break;
            case MovementContext.RunningWallUpLeft:
                if (runningWallUpCoroutine != null) return;
                runningWallUpCoroutine = StartCoroutine(RunningWallUp());
                break;
            case MovementContext.RunningWallUpRight:
                if (runningWallUpCoroutine != null) return;
                runningWallUpCoroutine = StartCoroutine(RunningWallUp());
                break;
            case MovementContext.SlidingWallDownLeft:
                _rigidBody2D.gravityScale = 1f;
                break;
            case MovementContext.SlidingWallDownRight:
                _rigidBody2D.gravityScale = 1f;
                break;
            case MovementContext.Falling:
                _rigidBody2D.gravityScale = 1f;
                break;
        }
    }
    private void RunLeft()
    {
        _rigidBody2D.linearVelocityX = -runningSpeed;
    }

    private void RunRight()
    {
        _rigidBody2D.linearVelocityX = runningSpeed;
    }

    private void SprintLeft()
    {
        _rigidBody2D.linearVelocityX = -sprintingSpeed;
    }

    private void SprintRight()
    {
        _rigidBody2D.linearVelocityX = sprintingSpeed;
    }

    private void Jump()
    {
        _rigidBody2D.AddForceAtPosition(new Vector3(0, jumpPower), transform.position, ForceMode2D.Impulse);
    }



    private void Sliding()
    {
        if (movementContext == MovementContext.SprintingLeft) StartCoroutine(SlideLeft());
        if (movementContext == MovementContext.SprintingRight) StartCoroutine(SlideRight());
    }

    private IEnumerator SlideLeft()
    {
        isSliding = true;
        _rigidBody2D.AddForceAtPosition(new Vector3(-slidingPower, 0), transform.position, ForceMode2D.Impulse);
        //REMOVE THIS WHEN GET ANIMATIONS AND MAKE THIS ANIMATION DRIVEN
        yield return new WaitForSeconds(1f);
        isSliding = false;
        //END OF REMOVE
    }

    private IEnumerator SlideRight()
    {
        isSliding = true;
        _rigidBody2D.AddForceAtPosition(new Vector3(slidingPower, 0), transform.position, ForceMode2D.Impulse);
        //REMOVE THIS WHEN GET ANIMATIONS AND MAKE THIS ANIMATION DRIVEN
        yield return new WaitForSeconds(1f);
        isSliding = false;
        //END OF REMOVE
    }

    private IEnumerator HangingOnWall()
    {
        var time = 0f;
        while(time < hangingOnWallTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        ResetCoroutine(ref hangingOnWallCoroutine);
    }

    private IEnumerator RunningWallUp()
    {
        var time = 0f;
        while ((movementContext == MovementContext.RunningWallUpLeft || movementContext == MovementContext.RunningWallUpRight) && time < runningWallTime)
        {
            _rigidBody2D.linearVelocityY = runningWallSpeed;
            time += Time.deltaTime;
            yield return null;
        }
        ResetCoroutine(ref runningWallUpCoroutine);
    }

    private void ResetCoroutine(ref Coroutine c)
    {
        Debug.Log($"Reset Coroutine");
        StopCoroutine(c);
        c = null;
    }
}
