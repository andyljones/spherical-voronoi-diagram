using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public class EdgeDrawer
    {
        public const int NumberOfVerticesPerEdge = 100;

        private HashSet<Edge> _edgesAlreadyDrawn; 
        private IEnumerable<Edge> _edges; 

        public EdgeDrawer(IEnumerable<Edge> edges)
        {
            _edges = edges;
            _edgesAlreadyDrawn = new HashSet<Edge>();
        }

        public void Update()
        {
            //var undrawnEdges = _edges.Except(_edgesAlreadyDrawn).ToList();

            //foreach (var edge in undrawnEdges)
            //{
            //    DrawEdgeList(edge);
            //    _edgesAlreadyDrawn.Add(edge);
            //}
        }

        private static void DrawEdgeList(Edge edge)
        {
            var vertices = new List<Vector3>();
            while (edge.PreviousEdge != null)
            {
                var verticesOfArc = VerticesOnGreatArc(edge.Endpoint, edge.PreviousEdge.Endpoint);
                vertices.AddRange(verticesOfArc);
                edge = edge.PreviousEdge;
            }

            var gameObject = DrawingUtilities.CreateLineObject("Edge list", vertices.ToArray(), "");
        }


        private static IEnumerable<Vector3> VerticesOnGreatArc(Vector3 a, Vector3 b)
        {
            var normal = Vector3.Cross(a, b);
            var perpendicularToA = Vector3.Cross(normal, a).normalized;

            var maxAngle = Mathf.Acos(Vector3.Dot(a, b));
            var angles = DrawingUtilities.AzimuthsInRange(0, maxAngle, NumberOfVerticesPerEdge);

            var vertices = angles.Select(angle => Mathf.Cos(angle)*a + Mathf.Sin(angle)*perpendicularToA);

            return vertices;
        }
    }
}
