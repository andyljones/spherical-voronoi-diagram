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
        
        var points = longitudes.Select(longitude => PointOnEllipse(focus, height, longitude)).ToList();
        points.Add(points.First());

        return points;
    }

    public static Vector3 PointOnEllipse(Vector3 focus, float height, float longitude)
    {
        var longitudeOfFocus = Mathf.Atan2(-focus.y, focus.x);

        var Y = focus.z - height;
        var X = Mathf.Sqrt(1-height*height) - Mathf.Sqrt(1-focus.z*focus.z)*Mathf.Cos(longitudeOfFocus - longitude);

        var colatitude = Mathf.Atan2(Y, X);

        var x = Mathf.Sin(colatitude)*Mathf.Cos(longitude);
        var y = -Mathf.Sin(colatitude)*Mathf.Sin(longitude);
        var z = Mathf.Cos(colatitude);

        return new Vector3(x, y, z);
    }
}