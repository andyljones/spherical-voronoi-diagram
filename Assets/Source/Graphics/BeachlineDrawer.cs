using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public static class BeachlineDrawer
    {
        public static int NumberOfVerticesPerArc = 100;

        public static void DrawBeachline(Beachline beachline)
        {
            foreach (var arc in beachline)
            {
                DrawArc(arc);
            }
        }

        private static void DrawArc(Arc arc)
        {
            var arcObject = new GameObject("Arc" + arc);
            var arcMeshFilter = arcObject.AddComponent<MeshFilter>();
            var arcRenderer = arcObject.AddComponent<MeshRenderer>();
            arcRenderer.material = Resources.Load("WindArrows", typeof(Material)) as Material;

            IEnumerable<float> azimuths;
            if (arc.LeftNeighbour == arc.SiteEvent && arc.SiteEvent == arc.RightNeighbour)
            {
                azimuths = DrawingUtilities.AzimuthsInRange(0, 2 * Mathf.PI, NumberOfVerticesPerArc);
            }
            else
            {
                var leftLimit = MathUtils.AzimuthOf(arc.LeftIntersection());
                var rightLimit = MathUtils.AzimuthOf(arc.RightIntersection());
                azimuths =  DrawingUtilities.AzimuthsInRange(leftLimit, rightLimit, NumberOfVerticesPerArc);
            }

            var pointsOnArc = PointsOnArc(arc, azimuths).ToList();
            pointsOnArc.RemoveAll(vector => vector == new Vector3(0, 0, 0));

            arcMeshFilter.mesh.vertices = pointsOnArc.ToArray();
            arcMeshFilter.mesh.SetIndices(
                Enumerable.Range(0, pointsOnArc.Count).ToArray(),
                MeshTopology.LineStrip,
                0);

            arcMeshFilter.mesh.RecalculateNormals();
            arcMeshFilter.mesh.uv = Enumerable.Repeat(new Vector2(0, 0), pointsOnArc.Count()).ToArray();
        }

        private static IEnumerable<Vector3> PointsOnArc(Arc arc, IEnumerable<float> azimuths)
        {
            var points = azimuths.Select(azimuth => PointOnEllipse(arc, azimuth)).ToList();

            return points;
        }

        public static Vector3 PointOnEllipse(Arc arc, float azimuth)
        {
            var p = arc.SiteEvent.Position;
            var Z = arc.Sweepline.Z;

            var tx = Mathf.Cos(azimuth);
            var ty = -Mathf.Sin(azimuth);

            var tz = (Z - p.z) / (p.x * tx + p.y * ty - Mathf.Sqrt(1 - Z * Z));

            var x = 1/Mathf.Sqrt(1 + 1/(tz*tz))*tx;
            var y = 1/Mathf.Sqrt(1 + 1/(tz*tz))*ty;
            var z = Mathf.Sign(tz)*1/Mathf.Sqrt(1 + tz*tz);

            return new Vector3(x, y, z).normalized;
        }
    }
}