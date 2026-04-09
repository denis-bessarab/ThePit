using UnityEngine;

public class Rope_DataCollector : MonoBehaviour
{
    public RopeData UpdateRopeData(GameObject ropeStart, GameObject ropeEnd, DistanceJoint2D d)
    {
        var ropeStartPosition = ropeStart.transform.position;
        var ropeEndPosition = ropeEnd.transform.position;
        var ropeLength = d.distance;
        var ropeCenterPosition = CalculateRopeCenterPosition(ropeStartPosition, ropeEndPosition, ropeLength);
        var ropeCenterRotation = CalculateRopeCenterRotation(ropeStartPosition, ropeEndPosition);

        return new RopeData(
            ropeStartPosition,
            ropeEndPosition,
            ropeLength,
            ropeCenterPosition,
            ropeCenterRotation);
    }

    private Vector3 CalculateRopeCenterPosition(Vector3 ropeStartPosition, Vector3 ropeEndPosition, float ropeLength)
    {
        var position = (ropeStartPosition + ropeEndPosition)/ 2;
        return position;
    }

    private Quaternion CalculateRopeCenterRotation(Vector3 ropeStartPosition, Vector3 ropeEndPosition)
    {
        Vector3 dir = ropeStartPosition - ropeEndPosition;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0, 0, angle + 90);
        return rotation;
    }
}
