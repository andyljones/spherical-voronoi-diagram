using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class EllipseDrawer
{
    public static IEnumerable<Vector3> PointsOnEllipse(Vector3 focus, float height, int count)
    {
        var longitudes = Enumerable.Range(0, count).Select(i => 2*Mathf.PI*i/count);
        
        var points = longitudes.Select(azimuth => PointOnEllipse(focus, height, azimuth)).ToList();
        points.Add(points.First());

        return points;
    }

    public static Vector3 PointOnEllipse(Vector3 focus, float height, float azimuth)
    {
        var Y = focus.z - height;
        var X = Mathf.Sqrt(1-height*height) - (focus.x * Mathf.Cos(azimuth) - focus.y * Mathf.Sin(azimuth));

        var x = Y/Mathf.Sqrt(X*X+Y*Y) * Mathf.Cos(azimuth);
        var y = Y/Mathf.Sqrt(X*X+Y*Y) * -Mathf.Sin(azimuth);
        var z = X/Mathf.Sqrt(X*X+Y*Y);

        return new Vector3(x, y, z);
    }

    public static float ColatitudeOf(Vector3 focus, float height, float azimuth)
    {
        return Mathf.Acos(PointOnEllipse(focus, height, azimuth).z);
    }
}