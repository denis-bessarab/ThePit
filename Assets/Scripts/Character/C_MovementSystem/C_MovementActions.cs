using System.Collections;
using UnityEngine;

public class C_MovementActions : MonoBehaviour
{
    public Coroutine jumpingCoroutine;
    public Coroutine fallingCoroutine;
    public Coroutine wallJumpCoroutine;
    public Coroutine slidingCoroutine;
    public Coroutine hangingOnWallCoroutine;
    public Coroutine runningWallUpCoroutine;
    public Coroutine climbingUpCoroutine;
    public Coroutine hangingCoroutine;
    public Coroutine forcedSlidingWallDownCoroutine;
    public Coroutine stoppingCoroutine;
    public Coroutine stepCoroutine;
    public Coroutine stepDownCoroutine;
    public Coroutine climbingDownCoroutine;
    public Coroutine cliffHangCoroutine;

    public void RunningLeft(C_MovementParameters p, Rigidbody2D rb, Character c)
    {
        rb.linearVelocityX = -p.runningSpeed;
    }

    public void RunningRight(C_MovementParameters p, Rigidbody2D rb, Character c)
    {
        rb.linearVelocityX = p.runningSpeed;
    }

    public void SprintLeft(C_MovementParameters p, Rigidbody2D rb, Character c)
    {
        rb.linearVelocityX = -p.sprintingSpeed;
    }

    public void SprintRight(C_MovementParameters p, Rigidbody2D rb, Character c)
    {
        rb.linearVelocityX = p.sprintingSpeed;
    }

    public IEnumerator Jump(C_MovementParameters p, Character c, Rigidbody2D rb)
    {
        rb.AddForceAtPosition(new Vector3(p.horizontalJumpPower * c.movementData.dir.x, p.jumpPower, 0), transform.position, ForceMode2D.Impulse);
        int framesHold = 0;

        while (c.MovementContext == C_MovementContext.Jumping)
        {
            if (framesHold != 20)
            {
                framesHold++;
                yield return null;
            }
            else if (c.movementData.jumpHold && framesHold < p.jumpPowerAddTimesLimit + 20)
            {
                framesHold++;
                rb.AddForceAtPosition(new Vector3(0, p.additionalJumpPower), transform.position, ForceMode2D.Impulse);
                yield return null;
            }
            else
            {
                rb.gravityScale += p.jumpDynamicGravity * Time.deltaTime;
                yield return null;
            }
        }
        rb.gravityScale = 1;
        ResetCoroutine(ref jumpingCoroutine);
    }

    public IEnumerator Fall(C_MovementParameters p, Character c, Rigidbody2D rb, C_LifeCycle lc)
    {
        var fallDistance = 0f;
        var initialFallPosY = transform.position.y;
        while (c.MovementContext == C_MovementContext.Falling)
        {
            rb.gravityScale += p.fallDynamicGravity * Time.deltaTime;
            fallDistance = Mathf.Abs(initialFallPosY - transform.position.y);
            yield return null;
        }
        rb.gravityScale = 1;
        if (fallDistance > 12f) lc.DeathByFalling();
        if (fallingCoroutine != null) ResetCoroutine(ref fallingCoroutine);
    }

    public IEnumerator WallJumpSoft(Vector2 direction, C_MovementParameters p, Character c, Rigidbody2D rb)
    {
        rb.AddForceAtPosition(new Vector3(direction.x * p.wallJumpPowerBackwardX, p.wallJumpPowerBackwardY, 0), transform.position, ForceMode2D.Impulse);
        while (c.MovementContext == C_MovementContext.WallJumpSoftRight || c.MovementContext == C_MovementContext.WallJumpSoftLeft || c.MovementContext == C_MovementContext.Falling)
        {
            rb.gravityScale += p.jumpDynamicGravity * Time.deltaTime;
            yield return null;
        }
        rb.gravityScale = 1f;
        ResetCoroutine(ref wallJumpCoroutine);
    }

    public IEnumerator WallJumpHard(Vector2 direction, C_MovementParameters p, Character c, Rigidbody2D rb)
    {
        rb.AddForceAtPosition(new Vector3(direction.x * p.wallJumpPowerForwardX, p.wallJumpPowerForwardY, 0), transform.position, ForceMode2D.Impulse);

        while (c.MovementContext == C_MovementContext.WallJumpHardRight || c.MovementContext == C_MovementContext.WallJumpHardLeft || c.MovementContext == C_MovementContext.Falling)
        {
            rb.gravityScale += p.fallDynamicGravity * Time.deltaTime;
            yield return null;
        }
        rb.gravityScale = 1f;
        ResetCoroutine(ref wallJumpCoroutine);
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

    public IEnumerator ClimbingUp(Vector2 dir, C_MovementParameters p, Rigidbody2D rb, CapsuleCollider2D col, Character c)
    {

        if(dir.x < 0)
        {
            while (c.groundData.groundOnLeft)
            {
                rb.linearVelocityX = 0;
                rb.linearVelocityY = p.climbingUpSpeed;
                yield return null;
            };

            while (!c.groundData.groundBelowRight0_3f)
            {
                rb.linearVelocityX = -p.climbingUpSpeed;
                rb.linearVelocityY = 0;
                yield return null;
            }
            ;
        }

        if (dir.x > 0)
        {
            while (c.groundData.groundOnRight)
            {
                rb.linearVelocityX = 0;
                rb.linearVelocityY = p.climbingUpSpeed;
                yield return null;
            };

            while (!c.groundData.groundBelowLeft0_3f)
            {
                rb.linearVelocityX = p.climbingUpSpeed;
                rb.linearVelocityY = 0;
                yield return null;
            }
            ;
        }

        rb.linearVelocity = Vector2.zero;
        if (climbingUpCoroutine != null) ResetCoroutine(ref climbingUpCoroutine);
    }

    public IEnumerator Hanging(C_MovementParameters p, Rigidbody2D rb, Character c)
    {

        //ADJUSTING POSITION
        if (c.MovementContext == C_MovementContext.HangingLeft)
        {
            while (c.groundData.groundAboveLeft0_1f)
            {
                rb.linearVelocityY = 1;
                yield return null;
            }
        }

        if (c.MovementContext == C_MovementContext.HangingRight)
        {
            while (c.groundData.groundAboveRight0_1f)
            {
                rb.linearVelocityY = 1;
                yield return null;
            }
        }

        rb.linearVelocityY = 0;

        while (c.MovementContext == C_MovementContext.HangingLeft || c.MovementContext == C_MovementContext.HangingRight)
        {
            rb.linearVelocity = Vector2.zero;
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

    public IEnumerator Step(Vector2 dir, Rigidbody2D rb, Character c, C_MovementParameters p)
    {
        if (dir.x < 0)
        {
            while (c.groundData.groundOnLeft)
            {
                rb.linearVelocityX = 0;
                rb.linearVelocityY = p.climbingUpSpeed;
                yield return null;
            }
            ;
        }

        if (dir.x > 0)
        {
            while (c.groundData.groundOnRight)
            {
                rb.linearVelocityX = 0;
                rb.linearVelocityY = p.climbingUpSpeed;
                yield return null;
            }
            ;
        }


        while (!c.groundData.groundBelowLeft || !c.groundData.groundBelowCenter || !c.groundData.groundBelowRight)
        {
            rb.linearVelocityX = dir.x == 1 ? p.climbingUpSpeed : -p.climbingUpSpeed;
            rb.linearVelocityY = 0;
            yield return null;
        }
        ;

        rb.linearVelocity = Vector2.zero;
        ResetCoroutine(ref stepCoroutine);
    }

    public IEnumerator StepDown(Vector2 dir, Rigidbody2D rb, Character c, C_MovementParameters p)
    {

        if (dir.x < 0)
        {
            while (c.groundData.groundBelowRight)
            {
                rb.linearVelocityX = -p.climbingUpSpeed;
                rb.linearVelocityY = 0;
                yield return null;
            }
            ;
        }

        if (dir.x > 0)
        {
            while (c.groundData.groundBelowLeft)
            {
                rb.linearVelocityX = p.climbingUpSpeed;
                rb.linearVelocityY = 0;
                yield return null;
            }
            ;
        }


        while (!c.groundData.groundBelow)
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = -p.climbingUpSpeed;
            yield return null;
        }
    ;

        rb.linearVelocity = Vector2.zero;
        ResetCoroutine(ref stepDownCoroutine);
    }

    public IEnumerator ClimbDown(Vector2 dir, Rigidbody2D rb, Character c, C_MovementParameters p)
    {

        if (dir.x < 0)
        {
            while (c.groundData.groundBelowRight)
            {
                rb.linearVelocityX = -p.climbingUpSpeed;
                rb.linearVelocityY = 0;
                yield return null;
            }
            ;
        }

        if (dir.x > 0)
        {
            while (c.groundData.groundBelowLeft)
            {
                rb.linearVelocityX = p.climbingUpSpeed;
                rb.linearVelocityY = 0;
                yield return null;
            }
            ;
        }


        while (!c.groundData.groundBelow)
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = -p.climbingUpSpeed;
            yield return null;
        }
;

        rb.linearVelocity = Vector2.zero;
        ResetCoroutine(ref climbingDownCoroutine);
    }

    public IEnumerator CliffHang(Vector2 dir, Rigidbody2D rb, Character c, C_MovementParameters p)
    {
        if (dir.x < 0)
        {
            while (c.groundData.groundBelowRight)
            {
                rb.linearVelocityX = -p.climbingUpSpeed;
                rb.linearVelocityY = 0;
                yield return null;
            }

            while (!c.groundData.groundOnRight)
            {
                rb.linearVelocityX = 0;
                rb.linearVelocityY = -p.climbingUpSpeed;
                yield return null;
            }

            while (!c.groundData.groundAboveRight0_1f)
            {
                rb.linearVelocityX = 0;
                rb.linearVelocityY = -p.climbingUpSpeed;
                yield return null;
            }
        }

        if (dir.x > 0)
        {
            while (c.groundData.groundBelowLeft)
            {
                rb.linearVelocityX = p.climbingUpSpeed;
                rb.linearVelocityY = 0;
                yield return null;
            }

            while (!c.groundData.groundOnLeft)
            {
                rb.linearVelocityX = 0;
                rb.linearVelocityY = -p.climbingUpSpeed;
                yield return null;
            }

            while (!c.groundData.groundAboveLeft0_1f)
            {
                rb.linearVelocityX = 0;
                rb.linearVelocityY = -p.climbingUpSpeed;
                yield return null;
            }
        }

        rb.linearVelocity = Vector2.zero;
        ResetCoroutine(ref cliffHangCoroutine);
    }


    public void ResetCoroutine(ref Coroutine c)
    {
        if (c == null) return;
        StopCoroutine(c);
        c = null;
    }
}
