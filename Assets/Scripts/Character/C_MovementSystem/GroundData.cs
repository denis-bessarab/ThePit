using System;
using UnityEngine;

[Serializable]
public struct GroundData
{
    public bool groundOnLeft;
    public bool groundOnRight;
    public bool groundBelow;
    public bool groundAbove;
    public bool groundTopLeft;
    public bool groundTopRight;
    public bool groundAboveLeft1f;
    public bool groundAboveRight1f;
    public bool groundAboveLeft0_1f;
    public bool groundAboveRight0_1f;
    public bool groundAboveLeft0_2f;
    public bool groundAboveRight0_2f;
    public Vector2 groundNormal;
    public bool groundBelowLeft;
    public bool groundBelowCenter;
    public bool groundBelowRight;
    public bool groundBottomLeft;
    public bool groundBottomRight;
    public bool groundBeneathLeft0_2f;
    public bool groundBeneathRight0_2f;
    public bool groundBeneathLeft2f;
    public bool groundBeneathRight2f;
    public bool groundBeneathLeft3f;
    public bool groundBeneathRight3f;

    public GroundData(
        bool groundOnLeft,
        bool groundOnRight,
        bool groundBelow,
        bool groundAbove,
        bool groundTopLeft,
        bool groundTopRight,
        bool groundAboveLeft1f,
        bool groundAboveRight1f,
        bool groundAboveLeft0_1f,
        bool groundAboveRight0_1f,
        bool groundAboveLeft0_2f,
        bool groundAboveRight0_2f,
        Vector2 groundNormal,
        bool groundBelowLeft,
        bool groundBelowCenter,
        bool groundBelowRight,
        bool groundBottomLeft,
        bool groundBottomRight,
        bool groundBeneathLeft0_2f,
        bool groundBeneathRight0_2f,
        bool groundBeneathLeft2f,
        bool groundBeneathRight2f,
        bool groundBeneathLeft3f,
        bool groundBeneathRight3f

        )
    {
        this.groundOnLeft = groundOnLeft;
        this.groundOnRight = groundOnRight;
        this.groundBelow = groundBelow;
        this.groundAbove = groundAbove;
        this.groundTopLeft = groundTopLeft;
        this.groundTopRight = groundTopRight;
        this.groundAboveLeft1f = groundAboveLeft1f;
        this.groundAboveRight1f = groundAboveRight1f;
        this.groundAboveLeft0_1f = groundAboveLeft0_1f;
        this.groundAboveRight0_1f = groundAboveRight0_1f;
        this.groundAboveLeft0_2f = groundAboveLeft0_2f;
        this.groundAboveRight0_2f = groundAboveRight0_2f;
        this.groundNormal = groundNormal;
        this.groundBelowLeft = groundBelowLeft;
        this.groundBelowCenter = groundBelowCenter;
        this.groundBelowRight = groundBelowRight;
        this.groundBottomLeft = groundBottomLeft;
        this.groundBottomRight = groundBottomRight;
        this.groundBeneathLeft0_2f = groundBeneathLeft0_2f;
        this.groundBeneathRight0_2f = groundBeneathRight0_2f;
        this.groundBeneathLeft2f = groundBeneathLeft2f;
        this.groundBeneathRight2f = groundBeneathRight2f;
        this.groundBeneathLeft3f = groundBeneathLeft3f;
        this.groundBeneathRight3f = groundBeneathRight3f;
    }
}