using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;
using UnityEngine;

public class CircleEventFinder
{
    private readonly BeachLine _beachLine;

    public CircleEventFinder(BeachLine beachLine)
    {
        _beachLine = beachLine;
    }

    public CircleEvent Check(Arc arc, Vector3 pointOnSweepLine)
    {
        var sites = SortedSites(arc);
        var vectorA = sites[0].Position;
        var vectorB = sites[1].Position;
        var vectorC = sites[2].Position;

        var circumcenter = Vector3.Cross(vectorA - vectorB, vectorC - vectorB).normalized;
        var radius = Mathf.Acos(Vector3.Dot(circumcenter, vectorA));

        return new CircleEvent(circumcenter, radius);
    }

    private List<SiteEvent> SortedSites(Arc arc)
    {
        var siteA = arc.Site;
        var siteB = _beachLine.CircularPredecessor(arc).Site;
        var siteC = _beachLine.CircularSuccessor(arc).Site;

        var vectorBA = (siteB.Position - siteA.Position);
        var vectorCA = (siteC.Position - siteA.Position);

        var areOrdered = Vector3.Dot(Vector3.Cross(vectorBA, vectorCA), siteA.Position) > 0;

        if (areOrdered)
        {
            return new List<SiteEvent> { siteA, siteB, siteC };
        }
        else
        {
            return new List<SiteEvent> { siteA, siteC, siteB };
        }
    }
}