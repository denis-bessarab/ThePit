using System;

[Serializable]
public struct MovementData
{
    public bool left;
    public bool right;
    public bool jump;
    public bool down;
    public bool up;
    public bool sprint;

    public MovementData(bool left, bool right, bool jump, bool down, bool up, bool sprint)
    {
        this.left = left;
        this.right = right;
        this.jump = jump;
        this.down = down;
        this.up = up;
        this.sprint = sprint;
    }
}