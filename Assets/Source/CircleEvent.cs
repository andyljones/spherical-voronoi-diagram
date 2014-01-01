using System;
using UnityEngine;

public class CircleEvent : IComparable<CircleEvent>
{
    public Arc Arc;

    public readonly SiteEvent OriginalLeftNeighbour;
    public readonly SiteEvent OriginalSiteEvent;
    public readonly SiteEvent OriginalRightNeighbour;

    public readonly float Priority;

    //TODO: Needs more tests.
    public CircleEvent(Arc arc)
    {
        Arc = arc;
        OriginalLeftNeighbour = arc.LeftNeighbour;
        OriginalSiteEvent = arc.SiteEvent;
        OriginalRightNeighbour = arc.RightNeighbour;
        Priority = CalculatePriority(arc.LeftNeighbour.Position, arc.SiteEvent.Position, arc.RightNeighbour.Position);
    }

    public bool StillHasSameSites()
    {
        var result = 
            Arc.LeftNeighbour == OriginalLeftNeighbour &&
            Arc.SiteEvent == OriginalSiteEvent &&
            Arc.RightNeighbour == OriginalRightNeighbour;

        return result;
    }

    private static float CalculatePriority(Vector3 a, Vector3 b, Vector3 c)
    {
        var v = Vector3.Cross(a - b, c - b);
        var r = Vector3.Dot(a, v);
        var delta = v.sqrMagnitude;

        var numerator = delta + r*v.z - Mathf.Sqrt((delta - v.z*v.z)*(delta - r*r));

        return numerator != 0 ? numerator/delta : 0;
    }

    public int CompareTo(CircleEvent other)
    {
        return Priority.CompareTo(other.Priority);
    }
}