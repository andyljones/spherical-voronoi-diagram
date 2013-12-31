using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public static class VoronoiGeneratorDrawer
    {
        public static int NumberOfSweeplineVertices = 100;

        public static void DrawVoronoiGenerator(VoronoiGenerator generator)
        {
            DrawSites(generator.SiteEvents);
            DrawSweepline(generator.Beachline.Sweepline);
            BeachlineDrawer.DrawBeachline(generator.Beachline);
        }

        private static void DrawSweepline(Sweepline sweepline)
        {
            var sweeplineObject = new GameObject("Sweepline");
            var sweeplineMeshFilter = sweeplineObject.AddComponent<MeshFilter>();
            sweeplineObject.AddComponent<MeshRenderer>();

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
        }

        private static void DrawSites(IEnumerable<SiteEvent> sites)
        {
            var siteObject = new GameObject("Sites");
            var siteMeshFilter = siteObject.AddComponent<MeshFilter>();
            siteObject.AddComponent<MeshRenderer>();

            var points = sites.Select(site => site.Position).ToArray();

            Debug.Log(points.Count());
            siteMeshFilter.mesh.vertices = points;
            siteMeshFilter.mesh.SetIndices(
                Enumerable.Range(0, points.Count()).ToArray(),
                MeshTopology.Points,
                0);
        }

    }
}
