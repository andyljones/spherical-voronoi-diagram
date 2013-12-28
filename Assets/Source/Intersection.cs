using System;
using UnityEngine;

public class Intersection
{
    public readonly Site LeftSite;
    public readonly Site RightSite;

    public float Azimuth { get { return CalculateAzimuth(); } }

    private readonly Sweepline _sweepline;

    public Intersection(Site leftSite, Site rightSite, Sweepline sweepline)
    {
        LeftSite = leftSite;
        RightSite = rightSite;
        _sweepline = sweepline;
    }

    private float CalculateAzimuth()
    {
        if (RightSite == null)
        {
            return LeftSite.Azimuth;
        }

        var a = LeftSite.Position;
        var b = RightSite.Position;
        var z = _sweepline.Height;

        var A = a.x*(z - b.z) - b.x*(z - a.z);
        var B = -(a.y*(z - b.z) - b.y*(z - a.z));
        var c = (a.z - b.z) * Mathf.Sqrt(1 - z*z);

        var R = Mathf.Sqrt(A*A + B*B);
        //var psi = Mathf.Atan2(A, B);
        var psi = Mathf.Sign(A) * Mathf.Acos(B/Mathf.Sqrt(A*A + B*B));

        return Mathf.Asin(c / R) - psi;
    }

    public override string ToString()
    {
        return "Left Site: " + LeftSite.ToString() + "\n Right Site: " + RightSite.ToString();
    }
}