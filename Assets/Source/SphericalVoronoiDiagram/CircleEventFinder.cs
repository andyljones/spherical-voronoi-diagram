using System.Collections.Generic;
using Assets.Source.SphericalVoronoiDiagram.Events;
using Events;
using UnityEngine;

public class CircleEventFinder
{
    private readonly BeachLine _beachLine;

    public CircleEventFinder(BeachLine beachLine)
    {
        _beachLine = beachLine;
    }

    public CircleEvent Check(IArc newArc, Vector3 pointOnSweepLine)
    {
        var arcs = SortedArcs(newArc);
        var vectorA = arcs[0].Site.Position;
        var vectorB = arcs[1].Site.Position;
        var vectorC = arcs[2].Site.Position;

        var circumcenter = Vector3.Cross(vectorA - vectorB, vectorC - vectorB).normalized;
        var radius = Mathf.Acos(Vector3.Dot(circumcenter, vectorA));

        var northPole = new Vector3(0, 0, 1);
        var polarAngleOfCenter = Mathf.Acos(Vector3.Dot(circumcenter.normalized, northPole));
        var polarAngleOfSweepline = Mathf.Acos(Vector3.Dot(pointOnSweepLine.normalized, northPole));

        if (polarAngleOfCenter + radius > polarAngleOfSweepline)
        {
            var newCircleEvent = new CircleEvent(newArc, circumcenter, radius);
            return newCircleEvent;
        }
        else
        {
            return null;
        }
    }

    private List<IArc> SortedArcs(IArc arc)
    {
        var arcA = arc;
        var arcB = _beachLine.CircularPredecessor(arc);
        var arcC = _beachLine.CircularSuccessor(arc);

        var vectorBA = (arcB.Site.Position - arcA.Site.Position);
        var vectorCA = (arcC.Site.Position - arcA.Site.Position);

        var areOrdered = Vector3.Dot(Vector3.Cross(vectorBA, vectorCA), arcA.Site.Position) > 0;

        if (areOrdered)
        {
            return new List<IArc> { arcA, arcB, arcC };
        }
        else
        {
            return new List<IArc> { arcA, arcC, arcB };
        }
    }
}