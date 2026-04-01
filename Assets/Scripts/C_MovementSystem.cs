using System;
using System.Collections;
using UnityEngine;

public class C_MovementSystem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private C_InputController inputController;
    [SerializeField] private C_CombatSystem combatSystem;
    [Header("Parameters")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float verticalRaycastDistance;
    [SerializeField] private float horizontalRaycastDistance;
    [SerializeField] private float jumpPower;
    [SerializeField] private float wallJumpPowerForwardX;
    [SerializeField] private float wallJumpPowerForwardY;
    [SerializeField] private float wallJumpPowerBackwardX;
    [SerializeField] private float wallJumpPowerBackwardY;
    [SerializeField] private float slidingPower;
    [SerializeField] private float slidingTime;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float sprintingSpeed;
    [SerializeField] private float hangingOnWallTime;
    [SerializeField] private float runningWallSpeed;
    [SerializeField] private float runningWallTime;
    [SerializeField] private float climbingUpSpeed;
    [SerializeField] private float fallDynamicGravity;
    [SerializeField] private float jumpDynamicGravity;
    [SerializeField] private MovementData movementData;
    [SerializeField] private GroundData groundData;
    [SerializeField] private MovementContext movementContext = MovementContext.Idling;
    public MovementContext Context
    {
        get => movementContext;
        set
        {
            if (movementContext == value) return;
            //Debug.Log($"Switching context from {movementContext} to {value}");
            movementContext = value;
        }
    }

    private Coroutine jumpingCoroutine;
    private Coroutine fallingCoroutine;
    private Coroutine jumpingFromWallCoroutine;
    private Coroutine slidingCoroutine;
    private Coroutine hangingOnWallCoroutine;
    private Coroutine runningWallUpCoroutine;
    private Coroutine climbingUpCoroutine;
    private Coroutine hangingCoroutine;
    private Coroutine forcedSlidingWallDownCoroutine;

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
        public bool groundAboveLeft;
        public bool groundAboveRight;
        public bool groundAboveLeft1f;
        public bool groundAboveRight1f;
        public bool groundAboveLeft0_1f;
        public bool groundAboveRight0_1f;
        public bool groundAboveLeft0_2f;
        public bool groundAboveRight0_2f;
        public Vector2 groundNormal;
        public bool groundBelowLeft;
        public bool groundBelowCenter;
        public bool groundBelowRight;

        public GroundData(
            bool groundOnLeft, 
            bool groundOnRight,  
            bool groundBelow, 
            bool groundAbove, 
            bool groundAboveLeft, 
            bool groundAboveRight, 
            bool groundAboveLeft1f, 
            bool groundAboveRight1f, 
            bool groundAboveLeft0_1f, 
            bool groundAboveRight0_1f,
            bool groundAboveLeft0_2f, 
            bool groundAboveRight0_2f,
            Vector2 groundNormal,
            bool groundBelowLeft,
            bool groundBelowCenter,
            bool groundBelowRight
            )
        {
            this.groundOnLeft = groundOnLeft;
            this.groundOnRight = groundOnRight;
            this.groundBelow = groundBelow;
            this.groundAbove = groundAbove;
            this.groundAboveLeft = groundAboveLeft;
            this.groundAboveRight = groundAboveRight;
            this.groundAboveLeft1f = groundAboveLeft1f;
            this.groundAboveRight1f = groundAboveRight1f;
            this.groundAboveLeft0_1f = groundAboveLeft0_1f;
            this.groundAboveRight0_1f = groundAboveRight0_1f;
            this.groundAboveLeft0_2f = groundAboveLeft0_2f;
            this.groundAboveRight0_2f = groundAboveRight0_2f;
            this.groundNormal = groundNormal;
            this.groundBelowLeft = groundBelowLeft;
            this.groundBelowCenter = groundBelowCenter;
            this.groundBelowRight = groundBelowRight;
        }
    }

    public enum MovementContext
    {
        Idling,
        RunningLeft,
        RunningRight,
        Jumping,
        SlidingLeft,
        SlidingRight,
        SprintingLeft,
        SprintingRight,
        HangingLeft,
        HangingRight,
        ClimbingUpLeft,
        ClimbingDownLeft,
        ClimbingUpRight,
        ClimbingDownRight,
        HangingOnWallLeft,
        HangingOnWallRight,
        RunningWallUpLeft,
        RunningWallUpRight,
        SlidingWallDownLeft,
        SlidingWallDownRight,
        WallJumpBackwardLeft,
        WallJumpBackwardRight,
        WallJumpForwardLeft,
        WallJumpForwardRight,
        Falling,
        ForcedSlidingWallDownLeft,
        ForcedSlidingWallDownRight
    }

    private void Update()
    {
        UpdateMovementData();
        UpdateGroundData();
        UpdateMovementContext();
        ResolveMovementContext();
    }

    private void UpdateMovementData()
    {
        var left = inputController.m_left.IsPressed();
        var right = inputController.m_right.IsPressed();
        var jump = inputController.m_jump.WasPressedThisFrame();
        var down = inputController.m_down.WasPressedThisFrame();
        var up = inputController.m_up.IsPressed();
        var sprint = inputController.m_sprint.IsPressed();

        movementData = new MovementData(left, right, jump, down, up, sprint);
    }

    private void UpdateGroundData()
    {
        var b = _collider.bounds;

        var belowLeft = Physics2D.Raycast(new Vector2(b.min.x + 0.05f, b.center.y), Vector2.down, verticalRaycastDistance, groundLayerMask);
        var belowCenter = Physics2D.Raycast(b.center, Vector2.down, verticalRaycastDistance, groundLayerMask);
        var belowRight = Physics2D.Raycast(new Vector2(b.max.x - 0.05f, b.center.y), Vector2.down, verticalRaycastDistance, groundLayerMask);

        var below = belowLeft || belowCenter || belowRight;
        var groundNormal = belowCenter.normal;

        var left = Physics2D.Raycast(b.center, Vector2.left, horizontalRaycastDistance, groundLayerMask);
        var right = Physics2D.Raycast(b.center, Vector2.right, horizontalRaycastDistance, groundLayerMask);
        var above = Physics2D.Raycast(b.center, Vector2.up, verticalRaycastDistance, groundLayerMask);
        var aboveLeft = Physics2D.Raycast(new Vector2(b.center.x, b.max.y), Vector2.left, horizontalRaycastDistance, groundLayerMask);
        var aboveRight = Physics2D.Raycast(new Vector2(b.center.x, b.max.y), Vector2.right, horizontalRaycastDistance, groundLayerMask);
        var aboveLeft1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + 1f), Vector2.left, horizontalRaycastDistance + 1f, groundLayerMask);
        var aboveRight1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + 1f), Vector2.right, horizontalRaycastDistance + 1f, groundLayerMask);
        var aboveLeft0_1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .1f), Vector2.left, horizontalRaycastDistance + 1f, groundLayerMask);
        var aboveRight0_1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .1f), Vector2.right, horizontalRaycastDistance + 1f, groundLayerMask);
        var aboveLeft0_2f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .2f), Vector2.left, horizontalRaycastDistance + 1f, groundLayerMask);
        var aboveRight0_2f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .2f), Vector2.right, horizontalRaycastDistance + 1f, groundLayerMask);

        groundData = new GroundData(
            left,
            right,
            below,
            above,
            aboveLeft,
            aboveRight,
            aboveLeft1f,
            aboveRight1f,
            aboveLeft0_1f,
            aboveRight0_1f,
            aboveLeft0_2f,
            aboveRight0_2f,
            groundNormal,
            belowLeft,
            belowCenter,
            belowRight
            );
    }

    private void UpdateMovementContext()
    {
        var m = movementData;
        var g = groundData;
        var vx = _rigidBody2D.linearVelocityX;
        var vy = _rigidBody2D.linearVelocityY;
        if(combatSystem.CombatStateParameter == C_CombatSystem.CombatState.Free)
        {
            Context = Context switch
            {
                //Idling
                MovementContext.RunningLeft when !m.left => MovementContext.Idling,
                MovementContext.SprintingLeft when !m.left => MovementContext.Idling,
                MovementContext.RunningRight when !m.right => MovementContext.Idling,
                MovementContext.SprintingRight when !m.right => MovementContext.Idling,
                MovementContext.Falling when g.groundBelow => MovementContext.Idling,
                MovementContext.SlidingLeft when g.groundBelow && slidingCoroutine == null => MovementContext.Idling,
                MovementContext.SlidingRight when g.groundBelow && slidingCoroutine == null => MovementContext.Idling,
                MovementContext.SlidingWallDownLeft when g.groundBelow => MovementContext.Idling,
                MovementContext.SlidingWallDownRight when g.groundBelow => MovementContext.Idling,
                MovementContext.HangingLeft when g.groundBelow && m.down => MovementContext.Idling,
                MovementContext.HangingRight when g.groundBelow && m.down => MovementContext.Idling,
                MovementContext.ClimbingUpLeft when climbingUpCoroutine == null => MovementContext.Idling,
                MovementContext.ClimbingUpRight when climbingUpCoroutine == null => MovementContext.Idling,

                //Sprinting Left
                MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft => MovementContext.SprintingLeft,
                MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft => MovementContext.SprintingLeft,
                MovementContext.SlidingLeft when g.groundBelow && slidingCoroutine == null && m.left => MovementContext.SprintingLeft,
                MovementContext.SlidingLeft when g.groundBelow && slidingCoroutine == null && m.left => MovementContext.SprintingLeft,
                MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft => MovementContext.SprintingLeft,
                MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft => MovementContext.SprintingLeft,
                MovementContext.HangingRight when g.groundBelow && m.left => MovementContext.SprintingLeft,
                MovementContext.WallJumpForwardLeft when g.groundBelow && m.left => MovementContext.SprintingLeft,

                //Sprinting Right
                MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight => MovementContext.SprintingRight,
                MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight => MovementContext.SprintingRight,
                MovementContext.SlidingRight when g.groundBelow && slidingCoroutine == null && m.right => MovementContext.SprintingRight,
                MovementContext.SlidingRight when g.groundBelow && slidingCoroutine == null && m.right => MovementContext.SprintingRight,
                MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight => MovementContext.SprintingRight,
                MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight => MovementContext.SprintingRight,
                MovementContext.HangingLeft when g.groundBelow && m.right => MovementContext.SprintingRight,
                MovementContext.WallJumpForwardRight when g.groundBelow && m.right => MovementContext.SprintingRight,

                //Jumping
                MovementContext.Idling when g.groundBelow && m.jump => MovementContext.Jumping,
                MovementContext.RunningLeft when m.jump => MovementContext.Jumping,
                MovementContext.RunningRight when m.jump => MovementContext.Jumping,
                MovementContext.SprintingLeft when m.jump => MovementContext.Jumping,
                MovementContext.SprintingRight when m.jump => MovementContext.Jumping,

                //Falling
                MovementContext.Idling when !g.groundBelow => MovementContext.Falling,
                MovementContext.Jumping when vy <= 0 && (vx == 0 || (!g.groundOnLeft && !g.groundOnRight)) => MovementContext.Falling,
                MovementContext.SlidingLeft when !g.groundBelow && slidingCoroutine == null => MovementContext.Falling,
                MovementContext.SlidingRight when !g.groundBelow && slidingCoroutine == null => MovementContext.Falling,
                MovementContext.WallJumpBackwardLeft when vy <= 0 => MovementContext.Falling,
                MovementContext.WallJumpBackwardRight when vy <= 0 => MovementContext.Falling,
                MovementContext.WallJumpForwardLeft when vy <= 0 && !g.groundOnLeft => MovementContext.Falling,
                MovementContext.WallJumpForwardRight when vy <= 0 && !g.groundOnRight => MovementContext.Falling,
                MovementContext.SprintingLeft when !g.groundBelow => MovementContext.Falling,
                MovementContext.SprintingRight when !g.groundBelow => MovementContext.Falling,
                MovementContext.SlidingWallDownLeft when !g.groundOnLeft => MovementContext.Falling,
                MovementContext.SlidingWallDownRight when !g.groundOnRight => MovementContext.Falling,

                //Sliding
                MovementContext.SprintingLeft when m.down => MovementContext.SlidingLeft,
                MovementContext.SprintingRight when m.down => MovementContext.SlidingRight,

                //Running wall up left
                MovementContext.Jumping when !g.groundBelow && m.left && g.groundOnLeft && g.groundAboveLeft1f => MovementContext.RunningWallUpLeft,
                MovementContext.WallJumpForwardLeft when g.groundOnLeft => MovementContext.RunningWallUpLeft,

                //Running wall up right
                MovementContext.Jumping when !g.groundBelow && m.right && g.groundOnRight && g.groundAboveRight1f => MovementContext.RunningWallUpRight,
                MovementContext.WallJumpForwardRight when g.groundOnRight => MovementContext.RunningWallUpRight,

                //Sliding wall down left
                MovementContext.HangingOnWallLeft when hangingOnWallCoroutine == null => MovementContext.SlidingWallDownLeft,
                MovementContext.RunningWallUpLeft when runningWallUpCoroutine == null => MovementContext.SlidingWallDownLeft,
                MovementContext.Falling when g.groundOnLeft => MovementContext.SlidingWallDownLeft,
                MovementContext.ForcedSlidingWallDownLeft when g.groundOnLeft && forcedSlidingWallDownCoroutine == null => MovementContext.SlidingWallDownLeft,

                //Sliding wall down right
                MovementContext.HangingOnWallRight when hangingOnWallCoroutine == null => MovementContext.SlidingWallDownRight,
                MovementContext.RunningWallUpRight when runningWallUpCoroutine == null => MovementContext.SlidingWallDownRight,
                MovementContext.Falling when g.groundOnRight => MovementContext.SlidingWallDownRight,
                MovementContext.ForcedSlidingWallDownRight when g.groundOnRight && forcedSlidingWallDownCoroutine == null => MovementContext.SlidingWallDownRight,

                //Wall jump backward left
                MovementContext.RunningWallUpRight when m.jump && !m.left => MovementContext.WallJumpBackwardLeft,

                //Wall jump backward right
                MovementContext.RunningWallUpLeft when m.jump && !m.right => MovementContext.WallJumpBackwardRight,

                //Wall jump forward left
                MovementContext.RunningWallUpRight when m.jump && m.left => MovementContext.WallJumpForwardLeft,
                MovementContext.HangingRight when m.jump && m.left => MovementContext.WallJumpForwardLeft,

                //Wall jump forward right
                MovementContext.RunningWallUpLeft when m.jump && m.right => MovementContext.WallJumpForwardRight,
                MovementContext.HangingLeft when m.jump && m.right => MovementContext.WallJumpForwardRight,

                //Hanging left
                MovementContext.Idling when g.groundOnLeft && g.groundAboveLeft && !g.groundAboveLeft0_2f && m.left => MovementContext.HangingLeft,
                MovementContext.RunningLeft when g.groundOnLeft && !g.groundAboveLeft1f && m.left => MovementContext.HangingLeft,
                MovementContext.SprintingLeft when g.groundOnLeft && !g.groundAboveLeft1f && m.left => MovementContext.HangingLeft,
                MovementContext.RunningWallUpLeft when g.groundAboveLeft && !g.groundAboveLeft0_1f => MovementContext.HangingLeft,

                //Hanging right
                MovementContext.Idling when g.groundOnRight && !g.groundAboveRight1f && m.right => MovementContext.HangingRight,
                MovementContext.RunningRight when g.groundOnRight && !g.groundAboveRight1f && m.right => MovementContext.HangingRight,
                MovementContext.SprintingRight when g.groundOnRight && !g.groundAboveRight1f && m.right => MovementContext.HangingRight,
                MovementContext.RunningWallUpRight when g.groundAboveRight && !g.groundAboveRight0_1f => MovementContext.HangingRight,

                //Climbing up left
                MovementContext.HangingLeft when m.up => MovementContext.ClimbingUpLeft,
                MovementContext.HangingLeft when m.jump => MovementContext.ClimbingUpLeft,

                //Climbing up right
                MovementContext.HangingRight when m.up => MovementContext.ClimbingUpRight,
                MovementContext.HangingRight when m.jump => MovementContext.ClimbingUpRight,

                //Forced sliding down left
                MovementContext.HangingLeft when m.down => MovementContext.ForcedSlidingWallDownLeft,

                //Forced sliding down right
                MovementContext.HangingRight when m.down => MovementContext.ForcedSlidingWallDownRight,

                _ => Context,
            };
        }

        if(combatSystem.CombatStateParameter == C_CombatSystem.CombatState.Clinch)
        {
            Context = Context switch
            {
                _ => MovementContext.Idling
            };

        }
   
    }

    private void ResolveMovementContext()
    {
        switch (Context)
        {
            case MovementContext.SprintingLeft:
                SprintLeft();
                break;
            case MovementContext.SprintingRight:
                SprintRight();
                break;
            case MovementContext.Jumping:
                if (jumpingCoroutine != null) return;
                jumpingCoroutine = StartCoroutine(Jump());
                break;
            case MovementContext.SlidingLeft:
                if (slidingCoroutine != null) return;
                slidingCoroutine = StartCoroutine(SlideLeft());
                break;
            case MovementContext.SlidingRight:
                if (slidingCoroutine != null) return;
                slidingCoroutine = StartCoroutine(SlideRight());
                break;
            case MovementContext.HangingOnWallLeft:
            case MovementContext.HangingOnWallRight:
                if (hangingOnWallCoroutine != null) return;
                hangingOnWallCoroutine = StartCoroutine(HangingOnWall());
                break;
            case MovementContext.RunningWallUpLeft:
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
                if(fallingCoroutine != null) return;
                fallingCoroutine = StartCoroutine(Fall());
                break;
            case MovementContext.WallJumpBackwardLeft:
                if (jumpingFromWallCoroutine != null) return;
                jumpingFromWallCoroutine = StartCoroutine(WallJumpBackward(Vector2.left));
                break;
            case MovementContext.WallJumpBackwardRight:
                if (jumpingFromWallCoroutine != null) return;
                jumpingFromWallCoroutine = StartCoroutine(WallJumpBackward(Vector2.right));
                break;
            case MovementContext.WallJumpForwardLeft:
                if (jumpingFromWallCoroutine != null) return;
                jumpingFromWallCoroutine = StartCoroutine(WallJumpForward(Vector2.left));
                break;
            case MovementContext.WallJumpForwardRight:
                if (jumpingFromWallCoroutine != null) return;
                jumpingFromWallCoroutine = StartCoroutine(WallJumpForward(Vector2.right));
                break;
            case MovementContext.HangingLeft:
            case MovementContext.HangingRight:
                if (hangingCoroutine != null) return;
                hangingCoroutine = StartCoroutine(Hanging());
                break;
            case MovementContext.ClimbingUpLeft:
                if (climbingUpCoroutine != null) return;
                climbingUpCoroutine = StartCoroutine(ClimbingUp(Vector2.left));
                break;
            case MovementContext.ClimbingUpRight:
                if (climbingUpCoroutine != null) return;
                climbingUpCoroutine = StartCoroutine(ClimbingUp(Vector2.right));
                break;
            case MovementContext.ForcedSlidingWallDownLeft:
            case MovementContext.ForcedSlidingWallDownRight:
                _rigidBody2D.gravityScale = 1f;
                if (forcedSlidingWallDownCoroutine != null) return;
                forcedSlidingWallDownCoroutine = StartCoroutine(ForcedSlidingWallDown());
                break;
        }
    }

    private void SprintLeft()
    {
        _rigidBody2D.linearVelocityX = -sprintingSpeed;
    }

    private void SprintRight()
    {
        _rigidBody2D.linearVelocityX = sprintingSpeed;
    }

    private IEnumerator Jump()
    {
        _rigidBody2D.AddForceAtPosition(new Vector3(0, jumpPower), transform.position, ForceMode2D.Impulse);
        while(Context == MovementContext.Jumping || Context == MovementContext.Falling)
        {
            _rigidBody2D.gravityScale += jumpDynamicGravity;
            yield return null;
        }
        _rigidBody2D.gravityScale = 1;
        ResetCoroutine(ref jumpingCoroutine);
    }

    private IEnumerator Fall()
    {
        while (movementContext == MovementContext.Falling)
        {
            _rigidBody2D.gravityScale += fallDynamicGravity;
            yield return null;
        }
        _rigidBody2D.gravityScale = 1;
        if(fallingCoroutine != null) ResetCoroutine(ref fallingCoroutine);
    }

    private IEnumerator WallJumpBackward(Vector2 direction)
    {
        _rigidBody2D.AddForceAtPosition(new Vector3(direction.x * wallJumpPowerBackwardX, wallJumpPowerBackwardY, 0), transform.position, ForceMode2D.Impulse);
        while (Context == MovementContext.WallJumpBackwardRight || Context == MovementContext.WallJumpBackwardLeft || Context == MovementContext.Falling)
        {
            _rigidBody2D.gravityScale += jumpDynamicGravity;
            yield return null;
        }
        _rigidBody2D.gravityScale = 1f;
        ResetCoroutine(ref jumpingFromWallCoroutine);
    }

    private IEnumerator WallJumpForward(Vector2 direction)
    {
        _rigidBody2D.AddForceAtPosition(new Vector3(direction.x * wallJumpPowerForwardX, wallJumpPowerForwardY, 0), transform.position, ForceMode2D.Impulse);

        while (Context == MovementContext.WallJumpForwardRight || Context == MovementContext.WallJumpForwardLeft || Context == MovementContext.Falling)
        {
            _rigidBody2D.gravityScale += 0.01f;
            yield return null;
        }
        _rigidBody2D.gravityScale = 1f;
        ResetCoroutine(ref jumpingFromWallCoroutine);
    }

    private IEnumerator SlideLeft()
    {
        _rigidBody2D.AddForceAtPosition(new Vector3(-slidingPower, 0), transform.position, ForceMode2D.Impulse);
        yield return new WaitForSeconds(slidingTime);
        ResetCoroutine(ref slidingCoroutine);
    }

    private IEnumerator SlideRight()
    {
        _rigidBody2D.AddForceAtPosition(new Vector3(slidingPower, 0), transform.position, ForceMode2D.Impulse);
        yield return new WaitForSeconds(slidingTime);
        ResetCoroutine(ref slidingCoroutine);
    }

    private IEnumerator HangingOnWall()
    {
        var time = 0f;
        while(time < hangingOnWallTime)
        {
            _rigidBody2D.gravityScale = 0f;
            time += Time.deltaTime;
            yield return null;
        }
        _rigidBody2D.gravityScale = 1f;
        if(hangingOnWallCoroutine != null) ResetCoroutine(ref hangingOnWallCoroutine);
    }

    private IEnumerator RunningWallUp()
    {
        var time = 0f;
        while (time < runningWallTime && (Context == MovementContext.RunningWallUpLeft || Context == MovementContext.RunningWallUpRight))
        {
            _rigidBody2D.linearVelocityY = runningWallSpeed;
            time += Time.deltaTime;
            yield return null;
        }

        while (_rigidBody2D.linearVelocityY > 0 && (Context == MovementContext.RunningWallUpLeft || Context == MovementContext.RunningWallUpRight))
        {
            yield return null;
        }
        ResetCoroutine(ref runningWallUpCoroutine);
    }

    private IEnumerator ClimbingUp(Vector2 dir)
    {
        while (Physics2D.Raycast(new Vector2(_collider.bounds.center.x, _collider.bounds.min.y), dir, 1f, groundLayerMask))
        {
            _rigidBody2D.linearVelocityX = 0;
            _rigidBody2D.linearVelocityY = climbingUpSpeed;
            yield return null;
        };
        while (!Physics2D.Raycast(new Vector2(dir.x == 1 ? _collider.bounds.min.x : _collider.bounds.max.x, _collider.bounds.min.y), Vector2.down, 0.5f, groundLayerMask))
        {
            _rigidBody2D.linearVelocityX = dir.x == 1 ? climbingUpSpeed : -climbingUpSpeed;
            _rigidBody2D.linearVelocityY = 0;
            yield return null;
        };
        _rigidBody2D.linearVelocity = Vector2.zero;
        if (climbingUpCoroutine != null) ResetCoroutine(ref climbingUpCoroutine);
    }

    private IEnumerator Hanging()
    {
        _rigidBody2D.linearVelocity = Vector2.zero;

        while(movementContext == MovementContext.HangingLeft || movementContext == MovementContext.HangingRight)
        {
            _rigidBody2D.gravityScale = 0;
            yield return null;
        }
        _rigidBody2D.gravityScale = 1;
        
        if(hangingCoroutine != null) ResetCoroutine(ref hangingCoroutine);
    }

    private IEnumerator ForcedSlidingWallDown()
    {
        float time = 0;
        while (time <= 0.5f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        ResetCoroutine(ref forcedSlidingWallDownCoroutine);
    } 

    private void ResetCoroutine(ref Coroutine c)
    {
        StopCoroutine(c);
        c = null;
    }
}
