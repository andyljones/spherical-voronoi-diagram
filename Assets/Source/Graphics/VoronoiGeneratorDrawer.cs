using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public static class VoronoiGeneratorDrawer
    {
        public static int NumberOfSweeplineVertices = 100;
        public static int NumberOfVerticesPerCircle = 100;

        public static void DrawVoronoiGenerator(VoronoiGenerator generator)
        {
            LatLongGridDrawer.DrawGrid();
            DrawSites(generator.SiteEvents);
            DrawSweepline(generator.Beachline.Sweepline);
            BeachlineDrawer.DrawBeachline(generator.Beachline);

            var circleObject = new GameObject("Circles");
            foreach (var circleEvent in generator.CircleEvents)
            {
                var circle = DrawCircle(circleEvent);
                circle.transform.parent = circleObject.transform;
            }
        }

        private static void DrawSweepline(Sweepline sweepline)
        {
            sweepline.Z = sweepline.Z - 0.0001f;

            var sweeplineObject = new GameObject("Sweepline");
            var sweeplineMeshFilter = sweeplineObject.AddComponent<MeshFilter>();
            var sweeplineRenderer = sweeplineObject.AddComponent<MeshRenderer>();
            sweeplineRenderer.material = Resources.Load("WindArrows", typeof(Material)) as Material;

            var azimuths = DrawingUtilities.AzimuthsInRange(0, 2 * Mathf.PI, NumberOfSweeplineVertices);

            var scaleFactor = Mathf.Sqrt(1 - sweepline.Z * sweepline.Z);
            Func<float, Vector3> pointAtAzimuth =
                azimuth => new Vector3(scaleFactor * Mathf.Cos(azimuth), -scaleFactor * Mathf.Sin(azimuth), sweepline.Z);

            var points = azimuths.Select(azimuth => pointAtAzimuth(azimuth)).ToArray();

            sweeplineMeshFilter.mesh.vertices = points;
            sweeplineMeshFilter.mesh.SetIndices(
                Enumerable.Range(0, points.Count()).ToArray(),
                MeshTopology.LineStrip,
                0);

            sweeplineMeshFilter.mesh.RecalculateNormals();
            sweeplineMeshFilter.mesh.uv = Enumerable.Repeat(new Vector2(0, 0), points.Count()).ToArray();
        }

        private static void DrawSites(IEnumerable<SiteEvent> sites)
        {
            var siteObject = new GameObject("Sites");
            var siteMeshFilter = siteObject.AddComponent<MeshFilter>();
            var siteRenderer = siteObject.AddComponent<MeshRenderer>();
            siteRenderer.material = Resources.Load("WindArrows", typeof(Material)) as Material;

            var points = sites.Select(site => site.Position).ToArray();

            siteMeshFilter.mesh.vertices = points;
            siteMeshFilter.mesh.SetIndices(
                Enumerable.Range(0, points.Count()).ToArray(),
                MeshTopology.Points,
                0);

            siteMeshFilter.mesh.RecalculateNormals();
            siteMeshFilter.mesh.uv = Enumerable.Repeat(new Vector2(0, 0), points.Count()).ToArray();
        }


        private static GameObject DrawCircle(CircleEvent circle)
        {
            var circleObject = new GameObject("Circle" + circle);
            var circleMeshFilter = circleObject.AddComponent<MeshFilter>();
            var circleRenderer = circleObject.AddComponent<MeshRenderer>();
            circleRenderer.material = Resources.Load("CircleEvents", typeof(Material)) as Material;

            var a = circle.OriginalLeftNeighbour.Position;
            var b = circle.OriginalSiteEvent.Position;
            var c = circle.OriginalRightNeighbour.Position;

            var n = Vector3.Cross(a - b, c - b).normalized;

            var s = a - Vector3.Dot(a, n) * n;
            var t = Vector3.Cross(s, n);

            var angles = DrawingUtilities.AzimuthsInRange(0, 2 * Mathf.PI, NumberOfVerticesPerCircle);

            var points = angles.Select(angle => Vector3.Dot(a,n)*n + Mathf.Cos(angle) * s + Mathf.Sin(angle) * t).ToArray();

            circleMeshFilter.mesh.vertices = points;
            circleMeshFilter.mesh.SetIndices(
                Enumerable.Range(0, points.Count()).ToArray(),
                MeshTopology.LineStrip,
                0);

            circleMeshFilter.mesh.RecalculateNormals();
            circleMeshFilter.mesh.uv = Enumerable.Repeat(new Vector2(0, 0), points.Count()).ToArray();

            return circleObject;
        }
    }
}
