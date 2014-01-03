﻿using System;
using UnityEngine;
using SDebug = System.Diagnostics.Debug;

public class Arc : IArc
{
    public SiteEvent SiteEvent;
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

    public Vector3 DirectionOfLeftIntersection
    {
        get { return EllipseCalculator.EquatorialVectorOfIntersection(LeftNeighbour, SiteEvent, Sweepline); }
    }

    public Vector3 DirectionOfRightIntersection
    {
        get { return EllipseCalculator.EquatorialVectorOfIntersection(SiteEvent, RightNeighbour, Sweepline); }
    }

    public void UpdateLeftEdge()
    {
        var oldEdge = LeftEdge;
        var endpoint = EllipseCalculator.PointOnEllipseAboveVector(DirectionOfLeftIntersection, SiteEvent.Position, Sweepline);

        var newEdge = new Edge(endpoint, oldEdge);
        LeftEdge = newEdge;
    }

    public void UpdateRightEdge()
    {
        var oldEdge = RightEdge;
        var endpoint = EllipseCalculator.PointOnEllipseAboveVector(DirectionOfLeftIntersection, SiteEvent.Position, Sweepline);

        var newEdge = new Edge(endpoint, oldEdge);
        RightEdge = newEdge;
    }

    public override string ToString()
    {
        return String.Format(
            "({0,3:N0},{1,3:N0},{2,3:N0})",
            Mathf.Rad2Deg * MathUtils.AzimuthOf(DirectionOfLeftIntersection),
            Mathf.Rad2Deg * MathUtils.AzimuthOf(SiteEvent.Position),
            Mathf.Rad2Deg * MathUtils.AzimuthOf(DirectionOfRightIntersection));
    }
}