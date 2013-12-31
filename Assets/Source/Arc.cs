using System;
using UnityEngine;
using SDebug = System.Diagnostics.Debug;

public class Arc
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

    public Vector3 LeftIntersection()
    {
        return EllipseIntersectionCalculator.IntersectionBetween(LeftNeighbour, SiteEvent, Sweepline);
    }

    public Vector3 RightIntersection()
    {
        return EllipseIntersectionCalculator.IntersectionBetween(SiteEvent, RightNeighbour, Sweepline);
    }

    public override string ToString()
    {
        var leftIntersection = LeftIntersection();
        var rightIntersection = RightIntersection();

        return String.Format(
            "({0,3:N0},{1,3:N0},{2,3:N0})",
            180 / Mathf.PI * MathUtils.AzimuthOf(leftIntersection),
            180 / Mathf.PI * MathUtils.AzimuthOf(SiteEvent.Position),
            180 / Mathf.PI * MathUtils.AzimuthOf(rightIntersection));
    }

    public static bool AreInOrder(Arc a, Arc b, Arc c)
    {
        //TODO: Optimize this.
        var aLeft = a.LeftIntersection();
        var bLeft = b.LeftIntersection();
        var cLeft = c.LeftIntersection();

        var orderingOnLeftIntersections =
            MathUtils.AreInCyclicOrder(aLeft, bLeft, cLeft);

        if (orderingOnLeftIntersections != 0)
        {
            return orderingOnLeftIntersections > 0;
        }
        else
        {
            var aRight = a.RightIntersection();
            var aMid = MathUtils.AzimuthalMidpointBetween(aLeft, aRight);

            var bRight = b.RightIntersection();
            var bMid = MathUtils.AzimuthalMidpointBetween(bLeft, bRight);

            var cRight = c.RightIntersection();
            var cMid = MathUtils.AzimuthalMidpointBetween(cLeft, cRight);

            var orderingOnMidpoints =
                MathUtils.AreInCyclicOrder(aMid, bMid, cMid);

            return orderingOnMidpoints >= 0;
        }

        //Debug.Log(aLength + ", " + bLength + ", " + cLength);

        ////if (aLength <= MathUtils.ComparisonTolerance)
        ////{
        ////    return MathUtils.AreInCyclicOrder(aLeft, bRight, cRight) > 0;
        ////}

        ////if (bLength <= MathUtils.ComparisonTolerance)
        ////{
        ////    return MathUtils.AreInCyclicOrder(aLeft, bLeft, cRight) > 0;
        ////}

        ////if (cLength <= MathUtils.ComparisonTolerance)
        ////{
        ////    return MathUtils.AreInCyclicOrder(aLeft, bLeft, cLeft) > 0;
        ////}

        //var orderingOnLeftIntersections =
        //    MathUtils.AreInCyclicOrder(aLeft, bLeft, cLeft);

        //if (orderingOnLeftIntersections != 0)
        //{
        //    return orderingOnLeftIntersections > 0;
        //}
        //else
        //{
        //    var orderingOnRightIntersections =
        //        MathUtils.AreInCyclicOrder(aRight, bRight, cRight);

        //    return orderingOnRightIntersections >= 0;
        //}
    }
}