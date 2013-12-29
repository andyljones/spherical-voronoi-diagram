using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Site
{
    public readonly Vector3 Position;

    public Site(Vector3 position)
    {
        Position = position.normalized;
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