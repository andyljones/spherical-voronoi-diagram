using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MathUtils
{
    public const float ComparisonTolerance = 0.00001f;

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

    //TODO: Rewrite this function to not use trig.
    public static Vector3 AzimuthalMidpointBetween(Vector3 a, Vector3 b)
    {
        var azimuthOfA = AzimuthOf(a);
        var azimuthOfB = AzimuthOf(b);

        float azimuthalMidpoint;
        if (azimuthOfA <= azimuthOfB)
        {
            azimuthalMidpoint = azimuthOfA + (azimuthOfB - azimuthOfA)/2;
        }
        else
        {
            azimuthalMidpoint = azimuthOfA + (2*Mathf.PI - (azimuthOfA - azimuthOfB))/2;
        }

        var x = Mathf.Cos(azimuthalMidpoint);
        var y = -Mathf.Sin(azimuthalMidpoint);
        var z = a.z + (b.z - a.z)/2;

        return new Vector3(x, y, z).normalized;
    }

    public static int AreInCyclicOrder(Vector3 a, Vector3 b, Vector3 c)
    {
        var planarA = new Vector3(a.x, a.y, 0).normalized;
        var planarB = new Vector3(b.x, b.y, 0).normalized;
        var planarC = new Vector3(c.x, c.y, 0).normalized;

        var normalToPlane = Vector3.Cross(planarA - planarB, planarC - planarB);
        var northwardsComponent = normalToPlane.z;

        if (Mathf.Abs(northwardsComponent) < ComparisonTolerance)
        {
            return 0;
        }
        else if (northwardsComponent >= ComparisonTolerance)
        {
            return 1;
        }
        else //if (northwardsComponent <= -ComparisonTolerance)
        {
            return -1;
        }
    }
}