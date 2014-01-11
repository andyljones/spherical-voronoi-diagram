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
        public const int NumberOfVerticesPerEdge = 50;

        private readonly GameObject _parentObject;

        private readonly Dictionary<IArc, Mesh> _edgeMeshes;
        private readonly Mesh _activeEdges;

        private readonly Dictionary<IArc, List<IridiumVector3>> _arcToEdges;
        private readonly Dictionary<IArc, int> _edgeListCounts; 
        private readonly Beachline _beachline;

        public EdgeDrawer(EdgeSet edgeSet, Beachline beachline)
        {
            _arcToEdges = edgeSet.CurrentEdgeDict();

            _beachline = beachline;
            _edgeMeshes = new Dictionary<IArc, Mesh>();
            _edgeListCounts = new Dictionary<IArc, int>();

            _parentObject = new GameObject("Edges");
            _activeEdges = CreateActiveEdgesObject();

        }

        private Mesh CreateActiveEdgesObject()
        {
            var gameObject = new GameObject("Active Edges");
            var meshFilter = gameObject.AddComponent<MeshFilter>();
            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material = Resources.Load("EdgeMaterial", typeof(Material)) as Material;

            return meshFilter.mesh;
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

            UpdateActiveEdges();
        }

        private void UpdateActiveEdges()
        {
            //TODO: Replace straight edges with arcs of great circle.
            if (_beachline.Count() < 2)
            {
                return;
            }

            var vertices = new List<Vector3>();
            foreach (var arc in _beachline)
            {
                var origin = _arcToEdges[arc].Last().ToUnityVector3();
                var destination = arc.PointOfIntersection(_beachline.Sweepline).ToUnityVector3();

                vertices.Add(origin);
                vertices.Add(destination);
            }

            _activeEdges.SetIndices(new int[] {}, MeshTopology.Lines, 0);
            _activeEdges.vertices = vertices.ToArray();
            _activeEdges.SetIndices(Enumerable.Range(0, _activeEdges.vertexCount).ToArray(), MeshTopology.Lines, 0);
            _activeEdges.RecalculateNormals();
            _activeEdges.uv = Enumerable.Repeat(new Vector2(0, 0), _activeEdges.vertexCount).ToArray();
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
                var verticesOfArc = DrawingUtilities.VerticesOnGreatArc(edgeList[i-1].ToUnityVector3(), edgeList[i].ToUnityVector3(), NumberOfVerticesPerEdge);
                vertices.AddRange(verticesOfArc); 
            }
            return vertices.ToArray();
        }
    }
}