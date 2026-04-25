using UnityEngine;

public class C_VelocityLimiter : MonoBehaviour
{
    public void LimitVelocity(Rigidbody2D rb, C_MovementParameters p, C_MovementContext context)
    {
        if(rb.linearVelocityX < -p.horizontalVelocityMax) rb.linearVelocityX = -p.horizontalVelocityMax;
        if(rb.linearVelocityX > p.horizontalVelocityMax) rb.linearVelocityX = p.horizontalVelocityMax;
        if(context == C_MovementContext.Falling && rb.linearVelocityY < -p.fallingVelocityMax) rb.linearVelocityY = -p.fallingVelocityMax;
    }
}
