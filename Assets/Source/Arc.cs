using System;
using UnityEngine;
using SDebug = System.Diagnostics.Debug;

public class Arc
{
    public readonly SiteEvent SiteEvent;
    public SiteEvent LeftNeighbour;
    public SiteEvent RightNeighbour;

    public Edge LeftEdge;
    public Edge RightEdge;

    public readonly Sweepline Sweepline;

    public Arc(SiteEvent siteEvent, Sweepline sweepline)
    {
        SiteEvent = siteEvent;
        LeftNeighbour = siteEvent;
        RightNeighbour = siteEvent;
        Sweepline = sweepline;
    }

    public Vector3 XYOfLeftIntersection()
    {
        return EllipseCalculator.EquatorialVectorOfIntersection(LeftNeighbour, SiteEvent, Sweepline);        
    }

    public Vector3 XYOfRightIntersection()
    {
        return EllipseCalculator.EquatorialVectorOfIntersection(SiteEvent, RightNeighbour, Sweepline);
    }

    public Vector3 LeftIntersection()
    {
        return EllipseCalculator.IntersectionBetween(LeftNeighbour, SiteEvent, Sweepline);
    }

    public Vector3 RightIntersection()
    {
        return EllipseCalculator.IntersectionBetween(SiteEvent, RightNeighbour, Sweepline);
    }

    public override string ToString()
    {
        var leftIntersection = LeftIntersection();
        var rightIntersection = RightIntersection();

        return String.Format(
            "({0,3:N0},{1,3:N0},{2,3:N0})",
            Mathf.Rad2Deg * MathUtils.AzimuthOf(leftIntersection),
            Mathf.Rad2Deg * MathUtils.AzimuthOf(SiteEvent.Position),
            Mathf.Rad2Deg * MathUtils.AzimuthOf(rightIntersection));
    }

    public static bool AreInOrder(Arc a, Arc b, Arc c)
    {
        //TODO: Optimize this.
        var aLeft = a.XYOfLeftIntersection();
        var bLeft = b.XYOfLeftIntersection();
        var cLeft = c.XYOfLeftIntersection();

        var orderingOnLeftIntersections =
            MathUtils.AreInCyclicOrder(aLeft, bLeft, cLeft);

        if (orderingOnLeftIntersections != 0)
        {
            return orderingOnLeftIntersections > 0;
        }
        else
        {
            var aRight = a.XYOfRightIntersection();
            var aMid = MathUtils.AzimuthalMidpointBetween(aLeft, aRight);

            var bRight = b.XYOfRightIntersection();
            var bMid = MathUtils.AzimuthalMidpointBetween(bLeft, bRight);

            var cRight = c.XYOfRightIntersection();
            var cMid = MathUtils.AzimuthalMidpointBetween(cLeft, cRight);

            var orderingOnMidpoints =
                MathUtils.AreInCyclicOrder(aMid, bMid, cMid);

            return orderingOnMidpoints >= 0;
        }
    }
}