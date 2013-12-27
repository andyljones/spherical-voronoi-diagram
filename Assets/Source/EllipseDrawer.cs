using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public static class EllipseDrawer
{
    public static IEnumerable<Vector3> PointsOnEllipse(Vector3 focus, float height, int count)
    {
        var longitudes = Enumerable.Range(0, count).Select(i => 2*Mathf.PI*i/count);
        
        var points = longitudes.Select(longitude => PointOnEllipse(focus, height, longitude)).ToList();
        points.Add(points.First());

        return points;
    }

    public static Vector3 PointOnEllipse(Vector3 focus, float height, float longitude)
    {
        var Y = focus.z - height;
        var X = Mathf.Sqrt(1-height*height) - (focus.x * Mathf.Cos(longitude) - focus.y * Mathf.Sin(longitude));

        var x = Y/Mathf.Sqrt(X*X+Y*Y) * Mathf.Cos(longitude);
        var y = Y/Mathf.Sqrt(X*X+Y*Y) * -Mathf.Sin(longitude);
        var z = X/Mathf.Sqrt(X*X+Y*Y);

        return new Vector3(x, y, z);
    }
}