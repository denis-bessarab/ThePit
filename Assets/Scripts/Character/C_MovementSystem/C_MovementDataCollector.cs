using UnityEngine;

public class C_MovementDataCollector : MonoBehaviour
{
    public MovementData UpdateMovementData(C_InputController ic, Rigidbody2D rb)
    {
        var left = ic.m_left.IsPressed();
        var right = ic.m_right.IsPressed();
        var jump = ic.m_jump.WasPressedThisFrame();
        var down = ic.m_down.WasPressedThisFrame();
        var up = ic.m_up.IsPressed();
        var sprint = ic.m_sprint.IsPressed();
        var vx = rb.linearVelocityX;
        var vy = rb.linearVelocityY;

        return new MovementData(left, right, jump, down, up, sprint, vx, vy);
    }

    public GroundData UpdateGroundData(CapsuleCollider2D col, C_MovementParameters p)
    {
        var b = col.bounds;

        var belowLeft = Physics2D.Raycast(new Vector2(b.min.x + 0.05f, b.center.y), Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        var belowCenter = Physics2D.Raycast(b.center, Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        var belowRight = Physics2D.Raycast(new Vector2(b.max.x - 0.05f, b.center.y), Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        
        var below = belowLeft || belowCenter || belowRight;

        var left1 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y + .6f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var left2 = Physics2D.Raycast(b.center, Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var left3 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y - .6f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var leftBottom = Physics2D.Raycast(new Vector2(b.center.x, b.min.y + 0.05f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var leftTop = Physics2D.Raycast(new Vector2(b.center.x, b.max.y - 0.05f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        
        var groundOnLeft = left1 || left2 || left3 || leftBottom || leftTop;

        var right1 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y + .6f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var right2 = Physics2D.Raycast(b.center, Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var right3 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y - .6f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var rightBottom = Physics2D.Raycast(new Vector2(b.center.x, b.min.y + 0.05f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var rightTop = Physics2D.Raycast(new Vector2(b.center.x, b.max.y - 0.05f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);

        var groundOnRight = right1 || right2 || right3 || rightBottom || rightTop;
        
        var groundNormal = belowCenter.normal;

        var above = Physics2D.Raycast(b.center, Vector2.up, p.verticalRaycastDistance, p.groundLayerMask);
        var aboveLeft1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + 1f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var aboveRight1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + 1f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var aboveLeft0_1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .1f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var aboveRight0_1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .1f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var aboveLeft0_2f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .2f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var aboveRight0_2f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .2f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);

        return new GroundData(
            groundOnLeft,
            groundOnRight,
            below,
            above,
            leftTop,
            rightTop,
            aboveLeft1f,
            aboveRight1f,
            aboveLeft0_1f,
            aboveRight0_1f,
            aboveLeft0_2f,
            aboveRight0_2f,
            groundNormal,
            belowLeft,
            belowCenter,
            belowRight,
            leftBottom,
            rightBottom
            );
    }
}
