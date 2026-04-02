using UnityEngine;

public class C_MovementDataCollector : MonoBehaviour
{
    public MovementData UpdateMovementData(C_InputController ic)
    {
        var left = ic.m_left.IsPressed();
        var right = ic.m_right.IsPressed();
        var jump = ic.m_jump.WasPressedThisFrame();
        var down = ic.m_down.WasPressedThisFrame();
        var up = ic.m_up.IsPressed();
        var sprint = ic.m_sprint.IsPressed();

        return new MovementData(left, right, jump, down, up, sprint);
    }

    public GroundData UpdateGroundData(CapsuleCollider2D col, C_MovementParameters p)
    {
        var b = col.bounds;

        var belowLeft = Physics2D.Raycast(new Vector2(b.min.x + 0.05f, b.center.y), Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        var belowCenter = Physics2D.Raycast(b.center, Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        var belowRight = Physics2D.Raycast(new Vector2(b.max.x - 0.05f, b.center.y), Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);

        var below = belowLeft || belowCenter || belowRight;
        var groundNormal = belowCenter.normal;

        var left = Physics2D.Raycast(b.center, Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var right = Physics2D.Raycast(b.center, Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var above = Physics2D.Raycast(b.center, Vector2.up, p.verticalRaycastDistance, p.groundLayerMask);
        var aboveLeft = Physics2D.Raycast(new Vector2(b.center.x, b.max.y), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var aboveRight = Physics2D.Raycast(new Vector2(b.center.x, b.max.y), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var aboveLeft1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + 1f), Vector2.left, p.horizontalRaycastDistance + 1f, p.groundLayerMask);
        var aboveRight1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + 1f), Vector2.right, p.horizontalRaycastDistance + 1f, p.groundLayerMask);
        var aboveLeft0_1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .1f), Vector2.left, p.horizontalRaycastDistance + 1f, p.groundLayerMask);
        var aboveRight0_1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .1f), Vector2.right, p.horizontalRaycastDistance + 1f, p.groundLayerMask);
        var aboveLeft0_2f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .2f), Vector2.left, p.horizontalRaycastDistance + 1f, p.groundLayerMask);
        var aboveRight0_2f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .2f), Vector2.right, p.horizontalRaycastDistance + 1f, p.groundLayerMask);

        return new GroundData(
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
}
