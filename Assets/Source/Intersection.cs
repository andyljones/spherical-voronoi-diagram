using System;
using UnityEngine;

public class Intersection
{
    public readonly Site LeftSite;
    public readonly Site RightSite;

    private readonly Sweepline _sweepline;

    public Intersection(Site leftSite, Site rightSite, Sweepline sweepline)
    {
        LeftSite = leftSite;
        RightSite = rightSite;
        _sweepline = sweepline;
    }

    public float Longitude()
    {
        var a = LeftSite.Position;
        var b = RightSite.Position;

        var aDeltaZ = a.z - _sweepline.Height;
        var bDeltaZ = b.z - _sweepline.Height;

        var B = aDeltaZ * b.x - bDeltaZ * a.x;
        var A = -(aDeltaZ * b.y - bDeltaZ * a.y);
        var c = (a.z - b.z) * Mathf.Sqrt(1 - _sweepline.Height*_sweepline.Height);

        var R = Mathf.Sqrt(B * B + A * A);
        var psi = Mathf.Atan2(B, A);

        Debug.Log("R:" + R);
        Debug.Log("psi:" + psi * 180 / Math.PI);

        return Mathf.Asin(c / R) - psi;
    }
}