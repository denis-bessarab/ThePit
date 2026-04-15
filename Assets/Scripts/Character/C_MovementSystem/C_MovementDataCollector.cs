using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class C_MovementDataCollector : MonoBehaviour
{

    public MovementData UpdateMovementData(C_InputController ic, Rigidbody2D rb)
    {
        var left = ic.m_left.IsPressed();
        var right = ic.m_right.IsPressed();
        var jump = ic.m_jump.WasPressedThisFrame();
        var down = ic.m_down.IsPressed();
        var up = ic.m_up.IsPressed();
        var sprint = ic.m_sprint.IsPressed();
        var vx = rb.linearVelocityX;
        var vy = rb.linearVelocityY;

        return new MovementData(left, right, jump, down, up, sprint, vx, vy);
    }

    public GroundData UpdateGroundData(CapsuleCollider2D col, C_MovementParameters p)
    {
        var b = col.bounds;

        var groundBelowLeft = Physics2D.Raycast(new Vector2(b.min.x + 0.05f, b.center.y), Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        var groundBelowCenter = Physics2D.Raycast(b.center, Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        var groundBelowRight = Physics2D.Raycast(new Vector2(b.max.x - 0.05f, b.center.y), Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        
        var groundBelow = groundBelowLeft || groundBelowCenter || groundBelowRight;

        var left1 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y + .6f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var left2 = Physics2D.Raycast(b.center, Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var left3 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y - .6f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundBottomLeft = Physics2D.Raycast(new Vector2(b.center.x, b.min.y + 0.05f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundTopLeft = Physics2D.Raycast(new Vector2(b.center.x, b.max.y - 0.05f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        
        var groundOnLeft = left1 || left2 || left3 || groundBottomLeft || groundTopLeft;

        var right1 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y + .6f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var right2 = Physics2D.Raycast(b.center, Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var right3 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y - .6f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundBottomRight = Physics2D.Raycast(new Vector2(b.center.x, b.min.y + 0.05f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundTopRight = Physics2D.Raycast(new Vector2(b.center.x, b.max.y - 0.05f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);

        var groundOnRight = right1 || right2 || right3 || groundBottomRight || groundTopRight;
        
        var groundNormal = groundBelowCenter.normal;

        var groundAbove = Physics2D.Raycast(b.center, Vector2.up, p.verticalRaycastDistance, p.groundLayerMask);
        var groundAboveLeft1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + 1f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundAboveRight1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + 1f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundAboveLeft0_1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .1f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundAboveRight0_1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .1f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundAboveLeft0_2f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .2f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundAboveRight0_2f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .2f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);

        var boxCastSize = 0.8f;

        var groundBeneathLeft1f = Physics2D.OverlapBox(new Vector2(b.center.x - 1f, b.min.y - .5f), new Vector2(boxCastSize, boxCastSize), 0f, p.groundLayerMask);
        var groundBeneathRight1f = Physics2D.OverlapBox(new Vector2(b.center.x + 1f, b.min.y - .5f), new Vector2(boxCastSize, boxCastSize), 0f, p.groundLayerMask);
        var groundBeneathLeft2f = Physics2D.OverlapBox(new Vector2(b.center.x - 1f, b.min.y - 1.5f), new Vector2(boxCastSize, boxCastSize), 0f, p.groundLayerMask);
        var groundBeneathRight2f = Physics2D.OverlapBox(new Vector2(b.center.x + 1f, b.min.y - 1.5f), new Vector2(boxCastSize, boxCastSize), 0f, p.groundLayerMask);
        var groundBeneathLeft3f = Physics2D.OverlapBox(new Vector2(b.center.x - 1f, b.min.y - 2.5f), new Vector2(boxCastSize, boxCastSize), 0f,  p.groundLayerMask);
        var groundBeneathRight3f = Physics2D.OverlapBox(new Vector2(b.center.x + 1f, b.min.y - 2.5f), new Vector2(boxCastSize, boxCastSize), 0f,  p.groundLayerMask);

        return new GroundData(
            groundOnLeft,
            groundOnRight,
            groundBelow,
            groundAbove,
            groundTopLeft,
            groundTopRight,
            groundAboveLeft1f,
            groundAboveRight1f,
            groundAboveLeft0_1f,
            groundAboveRight0_1f,
            groundAboveLeft0_2f,
            groundAboveRight0_2f,
            groundNormal,
            groundBelowLeft,
            groundBelowCenter,
            groundBelowRight,
            groundBottomLeft,
            groundBottomRight,
            groundBeneathLeft1f,
            groundBeneathRight1f,
            groundBeneathLeft2f,
            groundBeneathRight2f,
            groundBeneathLeft3f,
            groundBeneathRight3f
            );
    }
}
