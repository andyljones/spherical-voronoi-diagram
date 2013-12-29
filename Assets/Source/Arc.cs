using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Arc : IComparable<Arc>
{
    public readonly Site Site;
    public Site LeftNeighbour;
    public Site RightNeighbour;

    public readonly Sweepline Sweepline;

    public Arc(Site site, Sweepline sweepline)
    {
        Site = site;
        LeftNeighbour = site;
        RightNeighbour = site;
        Sweepline = sweepline;
    }

    public float AzimuthOfLeftIntersection()
    {
        return AzimuthOfIntersectionBetween(LeftNeighbour, Site);
    }

    public float AzimuthOfRightIntersection()
    {
        return AzimuthOfIntersectionBetween(Site, RightNeighbour);
    }

    private float AzimuthOfIntersectionBetween(Site site1, Site site2)
    {
        if (Equals(site1.Position, site2.Position))
        {
            return NormalizeAngle(site1.Azimuth());
        }

        var a = site1.Position;
        var b = site2.Position;
        var z = Sweepline.Z;

        var A = a.x * (z - b.z) - b.x * (z - a.z);
        var B = -(a.y * (z - b.z) - b.y * (z - a.z));
        var c = (a.z - b.z) * Mathf.Sqrt(1 - z * z);

        var R = Mathf.Sqrt(A * A + B * B);
        var psi = Mathf.Sign(A) * Mathf.Acos(B / Mathf.Sqrt(A * A + B * B));

        var result = Mathf.Asin(c / R) - psi;

        return NormalizeAngle(result);
    }

    public override string ToString()
    {
        return String.Format(
            "({0,3:N0},{1,3:N0},{2,3:N0})",
            180 / Mathf.PI * AzimuthOfLeftIntersection(),
            180 / Mathf.PI * Site.Azimuth(),
            180 / Mathf.PI * AzimuthOfRightIntersection());
    }

    public int CompareTo(Arc otherArc)
    {
        //TODO: Test.
        var compareOnLeftIntersections = 
            AzimuthOfLeftIntersection().CompareTo(otherArc.AzimuthOfLeftIntersection());
        if (compareOnLeftIntersections != 0)
        {
            return compareOnLeftIntersections;
        }
        else
        {
            var compareOnRightIntersections =
                AzimuthOfRightIntersection().CompareTo(otherArc.AzimuthOfRightIntersection());
            return compareOnRightIntersections;
        }
    }

    private static float NormalizeAngle(float angle)
    {
        return MathMod(angle, 2*Mathf.PI);
    }

    private static float MathMod(float x, float m)
    {
        return ((x % m) + m) % m;
    }
}