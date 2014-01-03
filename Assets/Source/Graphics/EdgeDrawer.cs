using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public class EdgeDrawer
    {
        public const int NumberOfVerticesPerEdge = 100;

        private GameObject _parentObject;

        private HashSet<Edge> _edgesAlreadyDrawn;
        private Dictionary<Arc, GameObject> _activeEdges; 
        
        private IEnumerable<Edge> _edges;
        private Beachline _beachline;

        public EdgeDrawer(IEnumerable<Edge> edges, Beachline beachline)
        {
            _edges = edges;
            _edgesAlreadyDrawn = new HashSet<Edge>();

            _beachline = beachline;
            _activeEdges = new Dictionary<Arc, GameObject>();

            _parentObject = new GameObject("Edges");

        }

        //TODO: Refactor.
        public void Update()
        {
            var undrawnEdges = _edges.Except(_edgesAlreadyDrawn).ToList();
            foreach (var edge in undrawnEdges)
            {
                var edgeObject = DrawEdgeList(edge);
                edgeObject.transform.parent = _parentObject.transform;
                _edgesAlreadyDrawn.Add(edge);
            }

            foreach (var arc in _beachline)
            {
                Vector3 focus;
                if (Mathf.Abs(arc.SiteEvent.Position.z - arc.Sweepline.Z) >= Mathf.Abs(arc.LeftNeighbour.Position.z - arc.Sweepline.Z))
                {
                    focus = arc.SiteEvent.Position;
                }
                else
                {
                    focus = arc.LeftNeighbour.Position;
                }

                var endpoint = EllipseCalculator.PointOnEllipseAboveVector(arc.DirectionOfLeftIntersection, focus, arc.Sweepline);
                var newEdge = new Edge(endpoint, arc.LeftEdge);

                if (_activeEdges.ContainsKey(arc))
                {
                    var edgeObject = _activeEdges[arc];
                    var mesh = edgeObject.GetComponent<MeshFilter>().mesh;
                    var vertices = VerticesInEdgeList(newEdge);
                    DrawingUtilities.UpdateLineMesh(mesh, vertices);
                }
                else
                {
                    var edgeObject = DrawEdgeList(newEdge);
                    edgeObject.transform.parent = _parentObject.transform;
                    _activeEdges.Add(arc, edgeObject);
                }
            }

            var deadArcs = _activeEdges.Keys.Except(_beachline).ToList();

            foreach (var arc in deadArcs)
            {
                var edgeObject = _activeEdges[arc];
                Object.Destroy(edgeObject);
                _activeEdges.Remove(arc);
            }
        }

        private static GameObject DrawEdgeList(Edge edge)
        {
            var vertices = VerticesInEdgeList(edge);
            var gameObject = DrawingUtilities.CreateLineObject("Edge list", vertices, "");

            return gameObject;
        }

        private static Vector3[] VerticesInEdgeList(Edge edge)
        {
            var vertices = new List<Vector3>();
            while (edge.PreviousEdge != null)
            {
                var verticesOfArc = VerticesOnGreatArc(edge.Endpoint, edge.PreviousEdge.Endpoint);
                vertices.AddRange(verticesOfArc);
                edge = edge.PreviousEdge;
            }
            return vertices.ToArray();
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
