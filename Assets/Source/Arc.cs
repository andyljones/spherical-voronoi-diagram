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

    private readonly Sweepline _sweepline;

    public Arc(Site site, Sweepline sweepline)
    {
        Site = site;
        LeftNeighbour = site;
        RightNeighbour = site;
        _sweepline = sweepline;
    }

    public float AzimuthOfLeftIntersection()
    {
        return AzimuthOfIntersectionBetween(LeftNeighbour, Site);
    }

    public float AzimuthOfRightIntersection()
    {
        return AzimuthOfIntersectionBetween(Site, RightNeighbour);
    }

    private float AzimuthOfIntersectionBetween(Site siteA, Site siteB)
    {
        if (Equals(siteA.Position, siteB.Position))
        {
            return siteA.Azimuth;
        }

        var a = siteA.Position;
        var b = siteB.Position;
        var z = _sweepline.Height;

        var A = a.x * (z - b.z) - b.x * (z - a.z);
        var B = -(a.y * (z - b.z) - b.y * (z - a.z));
        var c = (a.z - b.z) * Mathf.Sqrt(1 - z * z);

        var R = Mathf.Sqrt(A * A + B * B);
        var psi = Mathf.Sign(A) * Mathf.Acos(B / Mathf.Sqrt(A * A + B * B));

        return Mathf.Asin(c / R) - psi;
    }

    public override string ToString()
    {
        return String.Format("({0,3:N0},{1,3:N0})",
            180 / Mathf.PI * EllipseDrawer.ColatitudeOf(Site.Position, _sweepline.Height, AzimuthOfLeftIntersection()),
            180 / Mathf.PI * AzimuthOfLeftIntersection());
    }

    public int CompareTo(Arc otherArc)
    {
        //TODO: Test.
        var compareOnLeft = AzimuthOfLeftIntersection().CompareTo(otherArc.AzimuthOfLeftIntersection());
        var compareOnRight = AzimuthOfRightIntersection().CompareTo(otherArc.AzimuthOfRightIntersection());

        if (compareOnLeft != 0)
        {
            return compareOnLeft;
        }
        else
        {
            return compareOnRight;
        }
    }
}