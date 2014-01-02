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
}