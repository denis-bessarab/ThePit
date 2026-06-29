using UnityEngine;

public class C_MovementDataCollector : MonoBehaviour
{
    public bool left;
    public bool right;
    public bool jump;
    public bool down;
    public bool up;
    public bool sprint;
    public bool jumpHold;

    public void LeftHold() { left = true; }
    public void LeftRelease() { left = false; }
    public void RightHold() { right = true; }
    public void RightRelease() { right = false; }
    public void Jump() { jump = true; }
    public void JumpHold() { jumpHold = true; }
    public void JumpRelease() { jump = false; jumpHold = false; }
    public void DownHold() { down = true; }
    public void DownRelease() { down = false; }
    public void UpHold() { up = true; }
    public void UpRelease() { up = false; }
    public void SprintHold() { sprint = true; }
    public void SprintRelease() { sprint = false; }


    public MovementData UpdateMovementData(Rigidbody2D rb)
    {
        var vx = rb.linearVelocityX;
        var vy = rb.linearVelocityY;
        var dir = new Vector2(vx, vy).normalized;

        return new MovementData(left, right, jump, down, up, sprint, vx, vy, jumpHold, dir);
    }

    public GroundData UpdateGroundData(CapsuleCollider2D col, C_MovementParameters p)
    {
        var b = col.bounds;
        var boxCastSize = 0.8f;

        var groundBelowLeft = Physics2D.Raycast(new Vector2(b.min.x + 0.05f, b.center.y), Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        var groundBelowCenter = Physics2D.Raycast(b.center, Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        var groundBelowRight = Physics2D.Raycast(new Vector2(b.max.x - 0.05f, b.center.y), Vector2.down, p.verticalRaycastDistance, p.groundLayerMask);
        
        var groundBelowLeft0_3f = Physics2D.Raycast(new Vector2(b.min.x + 0.05f, b.center.y), Vector2.down, p.verticalRaycastDistance + .2f, p.groundLayerMask);
        var groundBelowRight0_3f = Physics2D.Raycast(new Vector2(b.max.x - 0.05f, b.center.y), Vector2.down, p.verticalRaycastDistance + .2f, p.groundLayerMask);

        var groundBelow = groundBelowLeft || groundBelowCenter || groundBelowRight;

        var left1 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y + .6f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundMiddleLeft = Physics2D.Raycast(new Vector2(b.center.x , b.center.y - 0.1f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var left3 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y - .6f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundBottomLeft = Physics2D.Raycast(new Vector2(b.center.x, b.min.y + 0.05f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundTopLeft = Physics2D.Raycast(new Vector2(b.center.x, b.max.y - 0.05f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        
        var groundOnLeft = left1 || groundMiddleLeft || left3 || groundBottomLeft || groundTopLeft;

        var right1 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y + .6f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundMiddleRight = Physics2D.Raycast(new Vector2(b.center.x, b.center.y - 0.1f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var right3 = Physics2D.Raycast(new Vector2(b.center.x, b.center.y - .6f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundBottomRight = Physics2D.Raycast(new Vector2(b.center.x, b.min.y + 0.05f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundTopRight = Physics2D.Raycast(new Vector2(b.center.x, b.max.y - 0.05f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);

        var groundOnRight = right1 || groundMiddleRight || right3 || groundBottomRight || groundTopRight;
        
        var groundNormal = groundBelowCenter.normal;

        var groundAbove = Physics2D.Raycast(b.center, Vector2.up, p.verticalRaycastDistance, p.groundLayerMask);
        var groundAboveLeft1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + 1f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundAboveRight1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + 1f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundAboveLeft0_1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .1f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundAboveRight0_1f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .1f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundAboveLeft0_2f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .2f), Vector2.left, p.horizontalRaycastDistance, p.groundLayerMask);
        var groundAboveRight0_2f = Physics2D.Raycast(new Vector2(b.center.x, b.max.y + .2f), Vector2.right, p.horizontalRaycastDistance, p.groundLayerMask);


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
            groundBeneathRight3f,
            groundMiddleLeft,
            groundMiddleRight,
            groundBelowLeft0_3f,
            groundBelowRight0_3f
            );
    }

    public void ResetMovementDataFlags()
    {
        jump = false;
    }
}
