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
        private readonly Dictionary<IArc, GameObject> _edgeObjects;

        private readonly Dictionary<IArc, List<IridiumVector3>> _arcToEdges;
        private Beachline _beachline;

        public EdgeDrawer(EdgeSet edgeSet, Beachline beachline)
        {
            _arcToEdges = edgeSet.CurrentEdgeDict();

            _beachline = beachline;
            _edgeObjects = new Dictionary<IArc, GameObject>();

            _parentObject = new GameObject("Edges");

        }

        //TODO: Refactor.
        public void Update()
        {
            foreach (var arcAndEdgeList in _arcToEdges)
            {
                var arc = arcAndEdgeList.Key;
                var edgeList = arcAndEdgeList.Value;

                if (!_edgeObjects.ContainsKey(arc))
                {
                    var edgeListObject = DrawEdgeList(edgeList);
                    edgeListObject.transform.parent = _parentObject.transform;
                    _edgeObjects.Add(arc, edgeListObject);

                }
                else
                {
                    var edgeListObject = _edgeObjects[arc];
                    var edgeListMesh = edgeListObject.GetComponent<MeshFilter>().mesh;
                    var newVertices = VerticesInEdgeList(edgeList);
                    DrawingUtilities.UpdateLineMesh(edgeListMesh, newVertices);
                }
            }
        }

        private static GameObject DrawEdgeList(List<IridiumVector3> edgeList)
        {
            var vertices = VerticesInEdgeList(edgeList);
            var gameObject = DrawingUtilities.CreateLineObject("Edge list", vertices, "");

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