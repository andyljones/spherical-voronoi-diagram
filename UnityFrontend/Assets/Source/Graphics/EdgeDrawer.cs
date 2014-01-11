using System.Collections.Generic;
using System.Linq;
using Generator;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using IridiumVector3 = Generator.Vector3;

namespace Graphics
{
    public class EdgeDrawer
    {
        public const int NumberOfVerticesPerEdge = 25;

        private GameObject _parentObject;

        //private HashSet<Edge> _edgesAlreadyDrawn;
        private readonly Dictionary<IArc, Mesh> _edgeMeshes;

        private readonly Dictionary<IArc, List<IridiumVector3>> _arcToEdges;
        private readonly Dictionary<IArc, int> _edgeListCounts; 
        private Beachline _beachline;

        public EdgeDrawer(EdgeSet edgeSet, Beachline beachline)
        {
            _arcToEdges = edgeSet.CurrentEdgeDict();

            _beachline = beachline;
            _edgeMeshes = new Dictionary<IArc, Mesh>();
            _edgeListCounts = new Dictionary<IArc, int>();

            _parentObject = new GameObject("Edges");

        }

        public void Update()
        {
            foreach (var arcAndEdgeList in _arcToEdges)
            {
                var arc = arcAndEdgeList.Key;
                var edgeList = arcAndEdgeList.Value;

                if (!_edgeMeshes.ContainsKey(arc))
                {
                    var edgeListObject = DrawEdgeList(edgeList);
                    edgeListObject.transform.parent = _parentObject.transform;
                    _edgeMeshes.Add(arc, edgeListObject.GetComponent<MeshFilter>().mesh);
                    _edgeListCounts.Add(arc, edgeList.Count);
                }
                else if (_edgeListCounts[arc] != edgeList.Count)
                {
                    var edgeMesh = _edgeMeshes[arc];
                    var newVertices = VerticesInEdgeList(edgeList);
                    DrawingUtilities.UpdateLineMesh(edgeMesh, newVertices);
                    _edgeListCounts[arc] = edgeList.Count;
                }
            }
        }

        private static GameObject DrawEdgeList(List<IridiumVector3> edgeList)
        {
            var vertices = VerticesInEdgeList(edgeList);
            var gameObject = DrawingUtilities.CreateLineObject("Edge list", vertices, "EdgeMaterial");

            return gameObject;
        }

        private static Vector3[] VerticesInEdgeList(List<IridiumVector3> edgeList)
        {
            var vertices = new List<Vector3>();
            for (int i = 1; i < edgeList.Count; i++)
            {
                var verticesOfArc = VerticesOnGreatArc(edgeList[i-1].ToUnityVector3(), edgeList[i].ToUnityVector3());
                vertices.AddRange(verticesOfArc); 
            }
            return vertices.ToArray();
        }

        private static IEnumerable<Vector3> VerticesOnGreatArc(Vector3 a, Vector3 b)
        {
            var normal = Vector3.Cross(a, b);
            var perpendicularToA = Vector3.Cross(normal, a).normalized;

            var maxAngle = Mathf.Acos(Vector3.Dot(a, b));
            var angles = DrawingUtilities.AzimuthsInRange(0, maxAngle, NumberOfVerticesPerEdge);

            var vertices = angles.Select(angle => Mathf.Cos(angle) * a + Mathf.Sin(angle) * perpendicularToA);

            return vertices;
        }
    }
}