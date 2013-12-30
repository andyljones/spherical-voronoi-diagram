using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class BeachlineDrawer
{
    public static int PointsPerArc = 100;

    public static void DrawBeachline(Beachline beachline)
    {
        foreach (var arc in beachline)
        {
            var arcObject = new GameObject("Arc" + arc);
            var arcMeshFilter = arcObject.AddComponent<MeshFilter>();
            arcObject.AddComponent<MeshRenderer>();

            var arcMesh = arcMeshFilter.mesh;

            var azimuths = AzimuthsInRange(arc.AzimuthOfLeftIntersection(), arc.AzimuthOfRightIntersection());
            var pointsOnArc = PointsOnArc(arc, azimuths).ToList();

            arcMesh.vertices = pointsOnArc.ToArray();
            arcMesh.SetIndices(
                Enumerable.Range(0, pointsOnArc.Count).ToArray(), 
                MeshTopology.LineStrip, 
                0);
        }
    }

    private static IEnumerable<Vector3> PointsOnArc(Arc arc, IEnumerable<float> azimuths)
    {
        var points = azimuths.Select(azimuth => PointOnEllipse(arc, azimuth)).ToList();

        return points;
    }

    private static IEnumerable<float> AzimuthsInRange(float leftAzimuth, float rightAzimuth)
    {
        if (leftAzimuth <= rightAzimuth)
        {
            var distance = rightAzimuth - leftAzimuth;
            return Enumerable.Range(0, PointsPerArc+1).Select(i => leftAzimuth + i*distance/PointsPerArc);
        }
        else
        {
            var distanceFromLeftToZero = Mathf.Abs(2*Mathf.PI - leftAzimuth);
            var distanceFromRightTo2Pi = Mathf.Abs(rightAzimuth);
            var distance = distanceFromLeftToZero + distanceFromRightTo2Pi;

            var numberOfpointsFromLeft = Mathf.FloorToInt(PointsPerArc * distanceFromLeftToZero / distance);
            var numberOfpointsFromRight = Mathf.CeilToInt(PointsPerArc * distanceFromRightTo2Pi / distance);

            var pointsFromLeft =
                Enumerable.Range(0, numberOfpointsFromLeft+1)
                    .Select(i => leftAzimuth + i*distanceFromLeftToZero/numberOfpointsFromLeft);

            var pointsFromRight =
                Enumerable.Range(0, numberOfpointsFromRight+1)
                    .Select(i => i * distanceFromRightTo2Pi / numberOfpointsFromRight);

            return pointsFromLeft.Concat(pointsFromRight);
        }
    }

    public static Vector3 PointOnEllipse(Arc arc, float azimuth)
    {
        var focus = arc.SiteEvent.Position;
        var height = arc.Sweepline.Z;

        var Y = focus.z - height;
        var X = Mathf.Sqrt(1 - height * height) - (focus.x * Mathf.Cos(azimuth) - focus.y * Mathf.Sin(azimuth));

        var x = Y/Mathf.Sqrt(X*X+Y*Y) * Mathf.Cos(azimuth);
        var y = Y/Mathf.Sqrt(X*X+Y*Y) * -Mathf.Sin(azimuth);
        var z = X/Mathf.Sqrt(X*X+Y*Y);

        return new Vector3(x, y, z);
    }
}