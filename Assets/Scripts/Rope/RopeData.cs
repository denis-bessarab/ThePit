using System;
using UnityEngine;

[Serializable]
public struct RopeData
{
    public Vector3 ropeStartPosition;
    public Vector3 ropeEndPosition;
    public float ropeLength;
    public Vector3 ropeCenterPosition;
    public Quaternion ropeCenterRotation;

    public RopeData (
        Vector3 ropeStartPosition,
        Vector3 ropeEndPosition,
        float ropeLength,
        Vector3 ropeCenterPosition,
        Quaternion ropeCenterRotation
        )
    {
        this.ropeStartPosition = ropeStartPosition;
        this.ropeEndPosition = ropeEndPosition;
        this.ropeLength = ropeLength;
        this.ropeCenterPosition = ropeCenterPosition;
        this.ropeCenterRotation = ropeCenterRotation;
    }
}
