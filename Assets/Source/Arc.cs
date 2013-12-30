using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Arc : IComparable<Arc>
{
    public readonly SiteEvent SiteEvent;
    public SiteEvent LeftNeighbour;
    public SiteEvent RightNeighbour;

    public readonly Sweepline Sweepline;

    public Arc(SiteEvent siteEvent, Sweepline sweepline)
    {
        SiteEvent = siteEvent;
        LeftNeighbour = siteEvent;
        RightNeighbour = siteEvent;
        Sweepline = sweepline;
    }

    public float AzimuthOfLeftIntersection()
    {
        return AzimuthOfIntersectionBetween(LeftNeighbour, SiteEvent);
    }

    public float AzimuthOfRightIntersection()
    {
        return AzimuthOfIntersectionBetween(SiteEvent, RightNeighbour);
    }

    private float AzimuthOfIntersectionBetween(SiteEvent site1, SiteEvent site2)
    {
        if (Equals(site1.Position, site2.Position))
        {
            return MathUtils.NormalizeAngle(site1.Azimuth());
        }

        var a = site1.Position;
        var b = site2.Position;
        var z = Sweepline.Z;

        var A = a.x * (z - b.z) - b.x * (z - a.z);
        var B = -(a.y * (z - b.z) - b.y * (z - a.z));
        var c = (a.z - b.z) * Mathf.Sqrt(1 - z * z);

        var R = Mathf.Sqrt(A * A + B * B);
        var psi = Mathf.Sign(A) * Mathf.Acos(B / Mathf.Sqrt(A * A + B * B));

        var ratioOfCtoR = c != 0 ? c/R : 0;

        var result = Mathf.Asin(ratioOfCtoR) - psi;

        return MathUtils.NormalizeAngle(result);
    }

    public override string ToString()
    {
        return String.Format(
            "({0,3:N0},{1,3:N0},{2,3:N0})",
            180 / Mathf.PI * AzimuthOfLeftIntersection(),
            180 / Mathf.PI * SiteEvent.Azimuth(),
            180 / Mathf.PI * AzimuthOfRightIntersection());
    }

    public int CompareTo(Arc otherArc)
    {
        //TODO: Test.
        //TODO: Compare arcs rather than intersections!
        var thisLeftAzimuth = AzimuthOfLeftIntersection();
        var otherLeftAzimuth = otherArc.AzimuthOfLeftIntersection();       
        if (Mathf.Abs(thisLeftAzimuth - otherLeftAzimuth) > MathUtils.AngleComparisonTolerance)
        {
            //Debug.Log("Compared on left");
            return thisLeftAzimuth.CompareTo(otherLeftAzimuth);
        }
        else
        {
            var thisRightAzimuth = AzimuthOfRightIntersection();
            var otherRightAzimuth = otherArc.AzimuthOfRightIntersection();

            if (Mathf.Abs(thisRightAzimuth - otherRightAzimuth) <= MathUtils.AngleComparisonTolerance)
            {
                return 0;
            }
            if (thisLeftAzimuth > thisRightAzimuth && otherLeftAzimuth > otherRightAzimuth)
            {
                // Case where both arcs cross the origin.
                return thisRightAzimuth.CompareTo(otherRightAzimuth);
            }
            else if (thisLeftAzimuth > thisRightAzimuth && otherLeftAzimuth <= otherRightAzimuth)
            {
                // Case where this arc crosses the origin, but the other doesn't.
                return 1;
            }
            else if (thisLeftAzimuth <= thisRightAzimuth && otherLeftAzimuth > otherRightAzimuth)
            {
                // Case where the other arc crosses the origin, but this one doesn't.
                return -1;
            }
            else//if (leftAzimuth <= thisRightAzimuth && otherLeftAzimuth <= otherRightAzimuth)
            {
                // Case where neither arc cross the origin.
                return thisRightAzimuth.CompareTo(otherRightAzimuth);
            }
        }
    }
}