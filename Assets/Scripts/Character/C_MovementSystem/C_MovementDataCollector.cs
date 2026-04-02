using System;
using System.Collections;
using UnityEngine;

public class C_MovementDataCollector : MonoBehaviour
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
    [SerializeField] private C_MovementContext movementContext = C_MovementContext.Idling;
    public C_MovementContext Context
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
                C_MovementContext.RunningLeft when !m.left => C_MovementContext.Idling,
                C_MovementContext.SprintingLeft when !m.left => C_MovementContext.Idling,
                C_MovementContext.RunningRight when !m.right => C_MovementContext.Idling,
                C_MovementContext.SprintingRight when !m.right => C_MovementContext.Idling,
                C_MovementContext.Falling when g.groundBelow => C_MovementContext.Idling,
                C_MovementContext.SlidingLeft when g.groundBelow && slidingCoroutine == null => C_MovementContext.Idling,
                C_MovementContext.SlidingRight when g.groundBelow && slidingCoroutine == null => C_MovementContext.Idling,
                C_MovementContext.SlidingWallDownLeft when g.groundBelow => C_MovementContext.Idling,
                C_MovementContext.SlidingWallDownRight when g.groundBelow => C_MovementContext.Idling,
                C_MovementContext.HangingLeft when g.groundBelow && m.down => C_MovementContext.Idling,
                C_MovementContext.HangingRight when g.groundBelow && m.down => C_MovementContext.Idling,
                C_MovementContext.ClimbingUpLeft when climbingUpCoroutine == null => C_MovementContext.Idling,
                C_MovementContext.ClimbingUpRight when climbingUpCoroutine == null => C_MovementContext.Idling,

                //Sprinting Left
                C_MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft => C_MovementContext.SprintingLeft,
                C_MovementContext.Idling when m.left && g.groundBelow && !g.groundOnLeft => C_MovementContext.SprintingLeft,
                C_MovementContext.SlidingLeft when g.groundBelow && slidingCoroutine == null && m.left => C_MovementContext.SprintingLeft,
                C_MovementContext.SlidingLeft when g.groundBelow && slidingCoroutine == null && m.left => C_MovementContext.SprintingLeft,
                C_MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft => C_MovementContext.SprintingLeft,
                C_MovementContext.Falling when g.groundBelow && m.left && !g.groundOnLeft => C_MovementContext.SprintingLeft,
                C_MovementContext.HangingRight when g.groundBelow && m.left => C_MovementContext.SprintingLeft,
                C_MovementContext.WallJumpForwardLeft when g.groundBelow && m.left => C_MovementContext.SprintingLeft,

                //Sprinting Right
                C_MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight => C_MovementContext.SprintingRight,
                C_MovementContext.Idling when m.right && g.groundBelow && !g.groundOnRight => C_MovementContext.SprintingRight,
                C_MovementContext.SlidingRight when g.groundBelow && slidingCoroutine == null && m.right => C_MovementContext.SprintingRight,
                C_MovementContext.SlidingRight when g.groundBelow && slidingCoroutine == null && m.right => C_MovementContext.SprintingRight,
                C_MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight => C_MovementContext.SprintingRight,
                C_MovementContext.Falling when g.groundBelow && m.right && !g.groundOnRight => C_MovementContext.SprintingRight,
                C_MovementContext.HangingLeft when g.groundBelow && m.right => C_MovementContext.SprintingRight,
                C_MovementContext.WallJumpForwardRight when g.groundBelow && m.right => C_MovementContext.SprintingRight,

                //Jumping
                C_MovementContext.Idling when g.groundBelow && m.jump => C_MovementContext.Jumping,
                C_MovementContext.RunningLeft when m.jump => C_MovementContext.Jumping,
                C_MovementContext.RunningRight when m.jump => C_MovementContext.Jumping,
                C_MovementContext.SprintingLeft when m.jump => C_MovementContext.Jumping,
                C_MovementContext.SprintingRight when m.jump => C_MovementContext.Jumping,

                //Falling
                C_MovementContext.Idling when !g.groundBelow => C_MovementContext.Falling,
                C_MovementContext.Jumping when vy <= 0 && (vx == 0 || (!g.groundOnLeft && !g.groundOnRight)) => C_MovementContext.Falling,
                C_MovementContext.SlidingLeft when !g.groundBelow && slidingCoroutine == null => C_MovementContext.Falling,
                C_MovementContext.SlidingRight when !g.groundBelow && slidingCoroutine == null => C_MovementContext.Falling,
                C_MovementContext.WallJumpBackwardLeft when vy <= 0 => C_MovementContext.Falling,
                C_MovementContext.WallJumpBackwardRight when vy <= 0 => C_MovementContext.Falling,
                C_MovementContext.WallJumpForwardLeft when vy <= 0 && !g.groundOnLeft => C_MovementContext.Falling,
                C_MovementContext.WallJumpForwardRight when vy <= 0 && !g.groundOnRight => C_MovementContext.Falling,
                C_MovementContext.SprintingLeft when !g.groundBelow => C_MovementContext.Falling,
                C_MovementContext.SprintingRight when !g.groundBelow => C_MovementContext.Falling,
                C_MovementContext.SlidingWallDownLeft when !g.groundOnLeft => C_MovementContext.Falling,
                C_MovementContext.SlidingWallDownRight when !g.groundOnRight => C_MovementContext.Falling,

                //Sliding
                C_MovementContext.SprintingLeft when m.down => C_MovementContext.SlidingLeft,
                C_MovementContext.SprintingRight when m.down => C_MovementContext.SlidingRight,

                //Running wall up left
                C_MovementContext.Jumping when !g.groundBelow && m.left && g.groundOnLeft && g.groundAboveLeft1f => C_MovementContext.RunningWallUpLeft,
                C_MovementContext.WallJumpForwardLeft when g.groundOnLeft => C_MovementContext.RunningWallUpLeft,

                //Running wall up right
                C_MovementContext.Jumping when !g.groundBelow && m.right && g.groundOnRight && g.groundAboveRight1f => C_MovementContext.RunningWallUpRight,
                C_MovementContext.WallJumpForwardRight when g.groundOnRight => C_MovementContext.RunningWallUpRight,

                //Sliding wall down left
                C_MovementContext.HangingOnWallLeft when hangingOnWallCoroutine == null => C_MovementContext.SlidingWallDownLeft,
                C_MovementContext.RunningWallUpLeft when runningWallUpCoroutine == null => C_MovementContext.SlidingWallDownLeft,
                C_MovementContext.Falling when g.groundOnLeft => C_MovementContext.SlidingWallDownLeft,
                C_MovementContext.ForcedSlidingWallDownLeft when g.groundOnLeft && forcedSlidingWallDownCoroutine == null => C_MovementContext.SlidingWallDownLeft,

                //Sliding wall down right
                C_MovementContext.HangingOnWallRight when hangingOnWallCoroutine == null => C_MovementContext.SlidingWallDownRight,
                C_MovementContext.RunningWallUpRight when runningWallUpCoroutine == null => C_MovementContext.SlidingWallDownRight,
                C_MovementContext.Falling when g.groundOnRight => C_MovementContext.SlidingWallDownRight,
                C_MovementContext.ForcedSlidingWallDownRight when g.groundOnRight && forcedSlidingWallDownCoroutine == null => C_MovementContext.SlidingWallDownRight,

                //Wall jump backward left
                C_MovementContext.RunningWallUpRight when m.jump && !m.left => C_MovementContext.WallJumpBackwardLeft,

                //Wall jump backward right
                C_MovementContext.RunningWallUpLeft when m.jump && !m.right => C_MovementContext.WallJumpBackwardRight,

                //Wall jump forward left
                C_MovementContext.RunningWallUpRight when m.jump && m.left => C_MovementContext.WallJumpForwardLeft,
                C_MovementContext.HangingRight when m.jump && m.left => C_MovementContext.WallJumpForwardLeft,

                //Wall jump forward right
                C_MovementContext.RunningWallUpLeft when m.jump && m.right => C_MovementContext.WallJumpForwardRight,
                C_MovementContext.HangingLeft when m.jump && m.right => C_MovementContext.WallJumpForwardRight,

                //Hanging left
                C_MovementContext.Idling when g.groundOnLeft && g.groundAboveLeft && !g.groundAboveLeft0_2f && m.left => C_MovementContext.HangingLeft,
                C_MovementContext.RunningLeft when g.groundOnLeft && !g.groundAboveLeft1f && m.left => C_MovementContext.HangingLeft,
                C_MovementContext.SprintingLeft when g.groundOnLeft && !g.groundAboveLeft1f && m.left => C_MovementContext.HangingLeft,
                C_MovementContext.RunningWallUpLeft when g.groundAboveLeft && !g.groundAboveLeft0_1f => C_MovementContext.HangingLeft,

                //Hanging right
                C_MovementContext.Idling when g.groundOnRight && !g.groundAboveRight1f && m.right => C_MovementContext.HangingRight,
                C_MovementContext.RunningRight when g.groundOnRight && !g.groundAboveRight1f && m.right => C_MovementContext.HangingRight,
                C_MovementContext.SprintingRight when g.groundOnRight && !g.groundAboveRight1f && m.right => C_MovementContext.HangingRight,
                C_MovementContext.RunningWallUpRight when g.groundAboveRight && !g.groundAboveRight0_1f => C_MovementContext.HangingRight,

                //Climbing up left
                C_MovementContext.HangingLeft when m.up => C_MovementContext.ClimbingUpLeft,
                C_MovementContext.HangingLeft when m.jump => C_MovementContext.ClimbingUpLeft,

                //Climbing up right
                C_MovementContext.HangingRight when m.up => C_MovementContext.ClimbingUpRight,
                C_MovementContext.HangingRight when m.jump => C_MovementContext.ClimbingUpRight,

                //Forced sliding down left
                C_MovementContext.HangingLeft when m.down => C_MovementContext.ForcedSlidingWallDownLeft,

                //Forced sliding down right
                C_MovementContext.HangingRight when m.down => C_MovementContext.ForcedSlidingWallDownRight,

                _ => Context,
            };
        }

        if(combatSystem.CombatStateParameter == C_CombatSystem.CombatState.Clinch)
        {
            Context = Context switch
            {
                _ => C_MovementContext.Idling
            };

        }
   
    }

    private void ResolveMovementContext()
    {
        switch (Context)
        {
            case C_MovementContext.SprintingLeft:
                SprintLeft();
                break;
            case C_MovementContext.SprintingRight:
                SprintRight();
                break;
            case C_MovementContext.Jumping:
                if (jumpingCoroutine != null) return;
                jumpingCoroutine = StartCoroutine(Jump());
                break;
            case C_MovementContext.SlidingLeft:
                if (slidingCoroutine != null) return;
                slidingCoroutine = StartCoroutine(SlideLeft());
                break;
            case C_MovementContext.SlidingRight:
                if (slidingCoroutine != null) return;
                slidingCoroutine = StartCoroutine(SlideRight());
                break;
            case C_MovementContext.HangingOnWallLeft:
            case C_MovementContext.HangingOnWallRight:
                if (hangingOnWallCoroutine != null) return;
                hangingOnWallCoroutine = StartCoroutine(HangingOnWall());
                break;
            case C_MovementContext.RunningWallUpLeft:
            case C_MovementContext.RunningWallUpRight:
                if (runningWallUpCoroutine != null) return;
                runningWallUpCoroutine = StartCoroutine(RunningWallUp());
                break;
            case C_MovementContext.SlidingWallDownLeft:
                _rigidBody2D.gravityScale = 1f;
                break;
            case C_MovementContext.SlidingWallDownRight:
                _rigidBody2D.gravityScale = 1f;
                break;
            case C_MovementContext.Falling:
                if(fallingCoroutine != null) return;
                fallingCoroutine = StartCoroutine(Fall());
                break;
            case C_MovementContext.WallJumpBackwardLeft:
                if (jumpingFromWallCoroutine != null) return;
                jumpingFromWallCoroutine = StartCoroutine(WallJumpBackward(Vector2.left));
                break;
            case C_MovementContext.WallJumpBackwardRight:
                if (jumpingFromWallCoroutine != null) return;
                jumpingFromWallCoroutine = StartCoroutine(WallJumpBackward(Vector2.right));
                break;
            case C_MovementContext.WallJumpForwardLeft:
                if (jumpingFromWallCoroutine != null) return;
                jumpingFromWallCoroutine = StartCoroutine(WallJumpForward(Vector2.left));
                break;
            case C_MovementContext.WallJumpForwardRight:
                if (jumpingFromWallCoroutine != null) return;
                jumpingFromWallCoroutine = StartCoroutine(WallJumpForward(Vector2.right));
                break;
            case C_MovementContext.HangingLeft:
            case C_MovementContext.HangingRight:
                if (hangingCoroutine != null) return;
                hangingCoroutine = StartCoroutine(Hanging());
                break;
            case C_MovementContext.ClimbingUpLeft:
                if (climbingUpCoroutine != null) return;
                climbingUpCoroutine = StartCoroutine(ClimbingUp(Vector2.left));
                break;
            case C_MovementContext.ClimbingUpRight:
                if (climbingUpCoroutine != null) return;
                climbingUpCoroutine = StartCoroutine(ClimbingUp(Vector2.right));
                break;
            case C_MovementContext.ForcedSlidingWallDownLeft:
            case C_MovementContext.ForcedSlidingWallDownRight:
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
        while(Context == C_MovementContext.Jumping || Context == C_MovementContext.Falling)
        {
            _rigidBody2D.gravityScale += jumpDynamicGravity;
            yield return null;
        }
        _rigidBody2D.gravityScale = 1;
        ResetCoroutine(ref jumpingCoroutine);
    }

    private IEnumerator Fall()
    {
        while (movementContext == C_MovementContext.Falling)
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
        while (Context == C_MovementContext.WallJumpBackwardRight || Context == C_MovementContext.WallJumpBackwardLeft || Context == C_MovementContext.Falling)
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

        while (Context == C_MovementContext.WallJumpForwardRight || Context == C_MovementContext.WallJumpForwardLeft || Context == C_MovementContext.Falling)
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
        while (time < runningWallTime && (Context == C_MovementContext.RunningWallUpLeft || Context == C_MovementContext.RunningWallUpRight))
        {
            _rigidBody2D.linearVelocityY = runningWallSpeed;
            time += Time.deltaTime;
            yield return null;
        }

        while (_rigidBody2D.linearVelocityY > 0 && (Context == C_MovementContext.RunningWallUpLeft || Context == C_MovementContext.RunningWallUpRight))
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

        while(movementContext == C_MovementContext.HangingLeft || movementContext == C_MovementContext.HangingRight)
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
