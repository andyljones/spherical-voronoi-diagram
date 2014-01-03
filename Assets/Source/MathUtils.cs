using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MathUtils
{
    public const float ComparisonTolerance = 0.0001f;

    public static float AzimuthOf(Vector3 point)
    {
        return NormalizeAngle(Mathf.Atan2(-point.y, point.x));
    }

    public static float NormalizeAngle(float angle)
    {
        return MathMod(angle, 2 * Mathf.PI);
    }

    private static float MathMod(float x, float m)
    {
        return ((x % m) + m) % m;
    }

    public static SiteEvent CreateSiteAt(float colatitude, float azimuth)
    {
        return new SiteEvent(CreateVectorAt(colatitude, azimuth));
    }

    public static Vector3 CreateVectorAt(float colatitude, float azimuth)
    {
        colatitude = colatitude * Mathf.PI / 180;
        azimuth = azimuth * Mathf.PI / 180;

        var x = Mathf.Sin(colatitude) * Mathf.Cos(azimuth);
        var y = Mathf.Sin(colatitude) * -Mathf.Sin(azimuth);
        var z = Mathf.Cos(colatitude);

        return new Vector3(x, y, z);
    }

    public static Vector3 EquatorialMidpointBetween(Vector3 a, Vector3 b)
    {
        if (a.z >= 1)
        {
            return EquatorialVectorOf(b);
        }
        if (b.z >= 1)
        {
            return EquatorialVectorOf(a);
        }
        if (a == b)
        {
            return EquatorialVectorOf(a);
        }

        var northPole = new Vector3(0, 0, 1);
        var equatorialA = EquatorialVectorOf(a);
        var equatorialB = EquatorialVectorOf(b);

        var midpoint = Vector3.Cross(equatorialB - northPole, equatorialA - northPole);
        return EquatorialVectorOf(midpoint);
    }

    public static Vector3 EquatorialVectorOf(Vector3 v)
    {
        v.z = 0;
        return v.normalized;
    }

    public static bool AreInCyclicOrder(Vector3 a, Vector3 b, Vector3 c)
    {
        var equatorialA = EquatorialVectorOf(a);
        var equatorialB = EquatorialVectorOf(b);
        var equatorialC = EquatorialVectorOf(c);

        if (equatorialA == equatorialC)
        {
            return true;
        }
        if (equatorialA == equatorialB)
        {
            return true;
        }
        if (equatorialB == equatorialC)
        {
            return false;
        }

        var normalToPlane = Vector3.Cross(equatorialA - equatorialB, equatorialC - equatorialB);
        var northwardsComponent = normalToPlane.z;

        return northwardsComponent >= 0;
    }
}