using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MathUtils
{
    public const float AngleComparisonTolerance = 0.0001f;

    public static float NormalizeAngle(float angle)
    {
        return MathMod(angle, 2 * Mathf.PI);
    }

    private static float MathMod(float x, float m)
    {
        return ((x % m) + m) % m;
    }

    public static SiteEvent SiteAt(float colatitude, float azimuth)
    {
        colatitude = colatitude * Mathf.PI / 180;
        azimuth = azimuth * Mathf.PI / 180;

        var x = Mathf.Sin(colatitude) * Mathf.Cos(azimuth);
        var y = Mathf.Sin(colatitude) * -Mathf.Sin(azimuth);
        var z = Mathf.Cos(colatitude);

        return new SiteEvent(new Vector3(x, y, z));
    }
}