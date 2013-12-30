using System;
using UnityEngine;

public class SiteEvent
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

    public override string ToString()
    {
        return String.Format("({0,3:N0},{1,3:N0})",
            180 / Mathf.PI * Mathf.Acos(Position.z),
            180 / Mathf.PI * Azimuth());
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