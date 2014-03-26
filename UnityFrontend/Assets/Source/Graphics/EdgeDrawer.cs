using System;
using System.Collections.Generic;
using System.Linq;
using Generator;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using IridiumVector3 = Generator.Vector3;
using Object = UnityEngine.Object;

namespace Graphics
{
    public class EdgeDrawer
    {
        public const int NumberOfVerticesPerEdge = 50;

        private readonly GameObject _parentObject;

        private readonly Dictionary<IArc, GameObject> _edgeListObjects;

        private readonly Dictionary<IArc, List<Vertex>> _arcToEdges;

        public EdgeDrawer(EdgeSet edgeSet)
        {
            _arcToEdges = edgeSet.VertexLists();

            _edgeListObjects = new Dictionary<IArc, GameObject>();

            _parentObject = new GameObject("Edges");
        }

        public void Update()
        {
            foreach (var arcAndEdgeList in _arcToEdges)
            {
                var arc = arcAndEdgeList.Key;
                var edgeList = arcAndEdgeList.Value.ToList();

                if (!_edgeListObjects.ContainsKey(arc))
                {
                    var edgeListObject = DrawEdgeList(edgeList);
                    edgeListObject.transform.parent = _parentObject.transform;
                    _edgeListObjects.Add(arc, edgeListObject);
                }
                else
                {
                    var newVertices = VerticesInEdgeList(edgeList);
                    var mesh = _edgeListObjects[arc].GetComponent<MeshFilter>().mesh;
                    DrawingUtilities.UpdateLineObject(mesh, newVertices);
                }
            }
        }

        private static GameObject DrawEdgeList(List<Vertex> vectorsInEdgeList)
        {
            var vertices = VerticesInEdgeList(vectorsInEdgeList);
            var gameObject = DrawingUtilities.CreateLineObject("Edge list", vertices, "EdgeMaterial");

            return gameObject;
        }

        private static Vector3[] VerticesInEdgeList(List<Vertex> vectorsInEdgeList)
        {
            var edgeList = vectorsInEdgeList.ToList();
            var vertices = new List<Vector3>();
            for (int i = 1; i < edgeList.Count; i++)
            {
                var verticesOfEdge = VerticesInEdge(edgeList[i-1], edgeList[i]);
                vertices.AddRange(verticesOfEdge); 
            }
            return vertices.ToArray();
        }

        private static IEnumerable<Vector3> VerticesInEdge(Vertex origin, Vertex destination)
        {
            var originVector = origin.Position.ToUnityVector3();
            var destinationVector = destination.Position.ToUnityVector3();
            if (originVector == destinationVector || Vector3.Cross(originVector, destinationVector) != new Vector3(0, 0, 0))
            {
                return DrawingUtilities.VerticesOnGeodesic(originVector, destinationVector, NumberOfVerticesPerEdge);
            }
            else
            {
                var commonSites = origin.Sites.Intersect(destination.Sites).ToList();
                var normal = -(commonSites.First().Position.ToUnityVector3() - commonSites.Last().Position.ToUnityVector3()).normalized;
                return DrawingUtilities.VerticesOnHalfOfGreatCircle(originVector, normal, NumberOfVerticesPerEdge);
            }
        }
    }
}