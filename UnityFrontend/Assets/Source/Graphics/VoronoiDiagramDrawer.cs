using System;
using System.Collections.Generic;
using System.Linq;
using Generator;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Graphics
{
    public class VoronoiDiagramDrawer
    {
        public static int NumberOfSweeplineVertices = 100;
        public static int NumberOfVerticesPerCircle = 100;

        private BeachlineDrawer _beachlineDrawer;
        private SweeplineDrawer _sweeplineDrawer;
        private CircleEventsDrawer _circleEventDrawer;
        //private EdgeDrawer _edgeDrawer;

        public VoronoiDiagramDrawer(VoronoiDiagram diagram)
        {
            LatLongGridDrawer.DrawGrid();
            DrawSites(diagram.SiteEvents);

            _sweeplineDrawer = new SweeplineDrawer(diagram.Beachline.Sweepline);
            _beachlineDrawer = new BeachlineDrawer(diagram.Beachline);
            _circleEventDrawer = new CircleEventsDrawer(diagram.CircleEventQueue);
            //_edgeDrawer = new EdgeDrawer(diagram.FinishedEdges, diagram.Beachline);
        }

        public void UpdateVoronoiDiagram()
        {
            _sweeplineDrawer.Update();
            _beachlineDrawer.Update();
            _circleEventDrawer.Update();
            //_edgeDrawer.Update();
        }

        private static void DrawSites(IEnumerable<SiteEvent> sites)
        {
            var siteObject = new GameObject("Sites");
            var siteMeshFilter = siteObject.AddComponent<MeshFilter>();
            var mesh = siteMeshFilter.mesh;
            var siteRenderer = siteObject.AddComponent<MeshRenderer>();
            siteRenderer.material = Resources.Load("SiteMaterial", typeof(Material)) as Material;

            var vertices = sites.SelectMany(site => new List<Vector3> {site.Position.ToUnityVector3(), 1.01f*site.Position.ToUnityVector3()}).ToArray();

            mesh.vertices = vertices;
            mesh.SetIndices(
                Enumerable.Range(0, mesh.vertexCount).ToArray(),
                MeshTopology.Lines,
                0);

            mesh.RecalculateNormals();
            mesh.uv = Enumerable.Repeat(new Vector2(0, 0), mesh.vertexCount).ToArray();
        }
    }
}
