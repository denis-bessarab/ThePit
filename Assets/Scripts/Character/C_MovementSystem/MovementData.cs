using System;
using UnityEngine;

[Serializable]
public struct MovementData
{
    public bool left;
    public bool right;
    public bool jump;
    public bool down;
    public bool up;
    public bool sprint;
    public float vx;
    public float vy;
    public bool jumpHold;
    public Vector2 dir;

    public MovementData(bool left, bool right, bool jump, bool down, bool up, bool sprint, float vx, float vy, bool jumpHold, Vector2 dir)
    {
        this.left = left;
        this.right = right;
        this.jump = jump;
        this.down = down;
        this.up = up;
        this.sprint = sprint;
        this.vx = vx;
        this.vy = vy;
        this.jumpHold = jumpHold;
        this.dir = dir;
    }
}