using System.Collections;
using UnityEngine;

public class C_MovementActions : MonoBehaviour
{
    public Coroutine jumpingCoroutine;
    public Coroutine fallingCoroutine;
    public Coroutine jumpingFromWallCoroutine;
    public Coroutine slidingCoroutine;
    public Coroutine hangingOnWallCoroutine;
    public Coroutine runningWallUpCoroutine;
    public Coroutine climbingUpCoroutine;
    public Coroutine hangingCoroutine;
    public Coroutine forcedSlidingWallDownCoroutine;

    public void SprintLeft(C_MovementParameters p, Rigidbody2D rb)
    {
        rb.linearVelocityX = -p.sprintingSpeed;
    }

    public void SprintRight(C_MovementParameters p, Rigidbody2D rb)
    {
        rb.linearVelocityX = p.sprintingSpeed;
    }

    public IEnumerator Jump(C_MovementParameters p, Character c, Rigidbody2D rb)
    {
        rb.AddForceAtPosition(new Vector3(0, p.jumpPower), transform.position, ForceMode2D.Impulse);
        while (c.MovementContext == C_MovementContext.Jumping || c.MovementContext == C_MovementContext.Falling)
        {
            rb.gravityScale += p.jumpDynamicGravity;
            yield return null;
        }
        rb.gravityScale = 1;
        ResetCoroutine(ref jumpingCoroutine);
    }

    public IEnumerator Fall(C_MovementParameters p, Character c, Rigidbody2D rb)
    {
        while (c.MovementContext == C_MovementContext.Falling)
        {
            rb.gravityScale += p.fallDynamicGravity;
            yield return null;
        }
        rb.gravityScale = 1;
        if (fallingCoroutine != null) ResetCoroutine(ref fallingCoroutine);
    }

    public IEnumerator WallJumpBackward(Vector2 direction, C_MovementParameters p, Character c, Rigidbody2D rb)
    {
        rb.AddForceAtPosition(new Vector3(direction.x * p.wallJumpPowerBackwardX, p.wallJumpPowerBackwardY, 0), transform.position, ForceMode2D.Impulse);
        while (c.MovementContext == C_MovementContext.WallJumpBackwardRight || c.MovementContext == C_MovementContext.WallJumpBackwardLeft || c.MovementContext == C_MovementContext.Falling)
        {
            rb.gravityScale += p.jumpDynamicGravity;
            yield return null;
        }
        rb.gravityScale = 1f;
        ResetCoroutine(ref jumpingFromWallCoroutine);
    }

    public IEnumerator WallJumpForward(Vector2 direction, C_MovementParameters p, Character c, Rigidbody2D rb)
    {
        rb.AddForceAtPosition(new Vector3(direction.x * p.wallJumpPowerForwardX, p.wallJumpPowerForwardY, 0), transform.position, ForceMode2D.Impulse);

        while (c.MovementContext == C_MovementContext.WallJumpForwardRight || c.MovementContext == C_MovementContext.WallJumpForwardLeft || c.MovementContext == C_MovementContext.Falling)
        {
            rb.gravityScale += 0.01f;
            yield return null;
        }
        rb.gravityScale = 1f;
        ResetCoroutine(ref jumpingFromWallCoroutine);
    }

    public IEnumerator SlideLeft(C_MovementParameters p, Rigidbody2D rb)
    {
        rb.AddForceAtPosition(new Vector3(-p.slidingPower, 0), transform.position, ForceMode2D.Impulse);
        yield return new WaitForSeconds(p.slidingTime);
        ResetCoroutine(ref slidingCoroutine);
    }

    public IEnumerator SlideRight(C_MovementParameters p, Rigidbody2D rb)
    {
        rb.AddForceAtPosition(new Vector3(p.slidingPower, 0), transform.position, ForceMode2D.Impulse);
        yield return new WaitForSeconds(p.slidingTime);
        ResetCoroutine(ref slidingCoroutine);
    }

    public IEnumerator HangingOnWall(C_MovementParameters p, Rigidbody2D rb)
    {
        var time = 0f;
        while (time < p.hangingOnWallTime)
        {
            rb.gravityScale = 0f;
            time += Time.deltaTime;
            yield return null;
        }
        rb.gravityScale = 1f;
        if (hangingOnWallCoroutine != null) ResetCoroutine(ref hangingOnWallCoroutine);
    }

    public IEnumerator RunningWallUp(C_MovementParameters p, Rigidbody2D rb, Character c)
    {
        var time = 0f;
        while (time < p.runningWallTime && (c.MovementContext == C_MovementContext.RunningWallUpLeft || c.MovementContext == C_MovementContext.RunningWallUpRight))
        {
            rb.linearVelocityY = p.runningWallSpeed;
            time += Time.deltaTime;
            yield return null;
        }

        while (rb.linearVelocityY > 0 && (c.MovementContext == C_MovementContext.RunningWallUpLeft || c.MovementContext == C_MovementContext.RunningWallUpRight))
        {
            yield return null;
        }
        ResetCoroutine(ref runningWallUpCoroutine);
    }

    public IEnumerator ClimbingUp(Vector2 dir, C_MovementParameters p, Rigidbody2D rb, CapsuleCollider2D col)
    {
        var b = col.bounds;

        Debug.Log(dir);

        while (Physics2D.Raycast(new Vector2(b.center.x, b.min.y), dir, 1f, p.groundLayerMask))
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = p.climbingUpSpeed;
            yield return null;
        }
        ;
        while (!Physics2D.Raycast(new Vector2(dir.x == 1 ? b.min.x : b.max.x, b.min.y), Vector2.down, 0.5f, p.groundLayerMask))
        {
            rb.linearVelocityX = dir.x == 1 ? p.climbingUpSpeed : -p.climbingUpSpeed;
            rb.linearVelocityY = 0;
            yield return null;
        }
        ;
        rb.linearVelocity = Vector2.zero;
        if (climbingUpCoroutine != null) ResetCoroutine(ref climbingUpCoroutine);
    }

    public IEnumerator Hanging(C_MovementParameters p, Rigidbody2D rb, Character c)
    {
        rb.linearVelocity = Vector2.zero;

        while (c.MovementContext == C_MovementContext.HangingLeft || c.MovementContext == C_MovementContext.HangingRight)
        {
            rb.gravityScale = 0;
            yield return null;
        }
        rb.gravityScale = 1;

        if (hangingCoroutine != null) ResetCoroutine(ref hangingCoroutine);
    }

    public IEnumerator ForcedSlidingWallDown()
    {
        float time = 0;
        while (time <= 0.5f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        ResetCoroutine(ref forcedSlidingWallDownCoroutine);
    }

    public void ResetCoroutine(ref Coroutine c)
    {
        StopCoroutine(c);
        c = null;
    }
}
