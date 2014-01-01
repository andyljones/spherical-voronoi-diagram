using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public class VoronoiDiagramDrawer
    {
        public static int NumberOfSweeplineVertices = 100;
        public static int NumberOfVerticesPerCircle = 100;

        private BeachlineDrawer _beachlineDrawer;
        private SweeplineDrawer _sweeplineDrawer;
        private CircleEventsDrawer _circleEventDrawer;

        public VoronoiDiagramDrawer(VoronoiDiagram diagram)
        {
            LatLongGridDrawer.DrawGrid();
            DrawSites(diagram.SiteEvents);

            _sweeplineDrawer = new SweeplineDrawer(diagram.Beachline.Sweepline);
            _beachlineDrawer = new BeachlineDrawer(diagram.Beachline);
            _circleEventDrawer = new CircleEventsDrawer(diagram.CircleEventQueue);
        }

        public void UpdateVoronoiDiagram()
        {
            _sweeplineDrawer.Update();
            _beachlineDrawer.Update();
            _circleEventDrawer.Update();
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
    }
}
