using UnityEngine;

public class CircleEvent
{
    public Arc Arc;

    private readonly SiteEvent _originalLeftNeighbour;
    private readonly SiteEvent _originalSiteEvent;
    private readonly SiteEvent _originalRightNeighbour;

    public readonly float Priority;

    //TODO: Needs more tests.
    public CircleEvent(Arc arc)
    {
        Arc = arc;
        _originalLeftNeighbour = arc.LeftNeighbour;
        _originalSiteEvent = arc.SiteEvent;
        _originalRightNeighbour = arc.RightNeighbour;
        Priority = CalculatePriority(arc.LeftNeighbour.Position, arc.SiteEvent.Position, arc.RightNeighbour.Position);
    }

    public bool StillHasSameSites()
    {
        var result = 
            Arc.LeftNeighbour == _originalLeftNeighbour &&
            Arc.SiteEvent == _originalSiteEvent &&
            Arc.RightNeighbour == _originalRightNeighbour;

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
}