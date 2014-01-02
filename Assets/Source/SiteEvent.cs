using System;
using UnityEngine;

public class SiteEvent : IComparable<SiteEvent>
{
    public readonly Vector3 Position;
    public readonly float Priority;

    public SiteEvent(Vector3 position)
    {
        Position = position.normalized;
        Priority = CalculatePriority(position);
    }

    private float CalculatePriority(Vector3 position)
    {
        return 1 + position.z;
    }

    public int CompareTo(SiteEvent other)
    {
        return Position.z.CompareTo(other.Position.z);
    }

    public override string ToString()
    {
        return String.Format("({0,3:N0},{1,3:N0})",
            Mathf.Rad2Deg * Mathf.Acos(Position.z),
            Mathf.Rad2Deg * Azimuth());
    }

    public float Azimuth()
    {
        return MathUtils.NormalizeAngle(Mathf.Atan2(-Position.y, Position.x));
    }

    public float Colatitude()
    {
        return Mathf.Acos(Position.z);
    }
}