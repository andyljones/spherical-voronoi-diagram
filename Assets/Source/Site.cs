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
        return Position.ToString();
    }

    public float CalculateAzimuth()
    {
        return MathMod(Mathf.Atan2(Position.y, Position.x), 2*Mathf.PI);
    }

    private static float MathMod(float x, float m)
    {
        return ((x % m) + m) % m;
    }
}