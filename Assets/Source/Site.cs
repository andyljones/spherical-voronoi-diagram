using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Site
{
    public readonly Vector3 Position;

    public float Azimuth { get { return CalculateAzimuth(); } }

    public Site(Vector3 position)
    {
        Position = position.normalized;
    }

    public override string ToString()
    {
        return String.Format("({0,3:N0},{1,3:N0})",
            180 / Mathf.PI * Mathf.Acos(Position.z),
            180 / Mathf.PI * Azimuth);
    }

    private float CalculateAzimuth()
    {
        return Mathf.Atan2(Position.y, Position.x);
    }

    private static float MathMod(float x, float m)
    {
        return ((x % m) + m) % m;
    }
}