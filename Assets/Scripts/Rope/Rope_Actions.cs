using UnityEngine;

public class Rope_Actions : MonoBehaviour
{
    public void Up(Rope_Parameters p, DistanceJoint2D d)
    {
        if (d.distance > p.minRopeLenght) d.distance -= p.lenghtChangeSpeed * Time.deltaTime;
    }

    public void Down(Rope_Parameters p, DistanceJoint2D d)
    {
        if (d.distance < p.maxRopeLenght) d.distance += p.lenghtChangeSpeed * Time.deltaTime;
    }

    public void SwingLeft(Rope_Parameters p, DistanceJoint2D d)
    {
        var rb = d.connectedBody;
        var pos = rb.position;
        rb.AddForceAtPosition(new Vector2(-p.swingPower,0f), pos, ForceMode2D.Impulse);
    }

    public void SwingRight(Rope_Parameters p, DistanceJoint2D d)
    {
        var rb = d.connectedBody;
        var pos = rb.position;
        rb.AddForceAtPosition(new Vector2(p.swingPower, 0f), pos, ForceMode2D.Impulse);
    }

    public void Jump(Rope r)
    {
        r.DeactivateRope();
    }
}
