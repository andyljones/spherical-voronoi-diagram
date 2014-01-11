﻿using System;
using System.Collections.Generic;
using System.Linq;
using Generator;
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
        private readonly Dictionary<IArc, GameObject> _activeEdgeObjects;

        private readonly Dictionary<IArc, List<IridiumVector3>> _arcToEdges;
        private readonly Dictionary<IArc, int> _edgeListCounts; 
        private readonly Beachline _beachline;

        public EdgeDrawer(EdgeSet edgeSet, Beachline beachline)
        {
            _arcToEdges = edgeSet.CurrentEdgeDict();

            _beachline = beachline;
            _edgeListObjects = new Dictionary<IArc, GameObject>();
            _edgeListCounts = new Dictionary<IArc, int>();

            _parentObject = new GameObject("Edges");

            _activeEdgeObjects = new Dictionary<IArc, GameObject>();

        }

        public void Update()
        {
            foreach (var arcAndEdgeList in _arcToEdges)
            {
                var arc = arcAndEdgeList.Key;
                var edgeList = arcAndEdgeList.Value;

                if (!_edgeListObjects.ContainsKey(arc))
                {
                    var edgeListObject = DrawEdgeList(edgeList);
                    edgeListObject.transform.parent = _parentObject.transform;
                    _edgeListObjects.Add(arc, edgeListObject);
                    _edgeListCounts.Add(arc, edgeList.Count);
                }
                else if (_edgeListCounts[arc] != edgeList.Count)
                {
                    var newVertices = VerticesInEdgeList(edgeList);
                    var mesh = _edgeListObjects[arc].GetComponent<MeshFilter>().mesh;
                    DrawingUtilities.UpdateLineObject(mesh, newVertices);
                    _edgeListCounts[arc] = edgeList.Count;
                }
            }

            UpdateActiveEdges();
        }

        private void UpdateActiveEdges()
        {
            //TODO: Replace straight edges with arcs of great circle.
            if (_beachline.Count() <= 1)
            {
                return;
            }

            foreach (var arc in _beachline)
            {
                if (!_activeEdgeObjects.ContainsKey(arc))
                {
                    var vertices = VerticesInActiveEdge(arc);
                    var edgeObject = DrawingUtilities.CreateLineObject("Active edge", vertices, "EdgeMaterial");
                    edgeObject.transform.parent = _parentObject.transform;
                    _activeEdgeObjects.Add(arc, edgeObject);
                }
                else
                {
                    var vertices = VerticesInActiveEdge(arc);
                    var mesh = _activeEdgeObjects[arc].GetComponent<MeshFilter>().mesh;
                    DrawingUtilities.UpdateLineObject(mesh, vertices);
                }
            }

            var deadArcs = _activeEdgeObjects.Keys.Except(_beachline).ToList();

            foreach (var deadArc in deadArcs)
            {
                var deadEdgeObject = _activeEdgeObjects[deadArc];
                Object.Destroy(deadEdgeObject);
                _activeEdgeObjects.Remove(deadArc);
            }
        }

        private Vector3[] VerticesInActiveEdge(IArc arc)
        {
            var origin = _arcToEdges[arc].Last().ToUnityVector3();
            var destination = arc.PointOfIntersection(_beachline.Sweepline).ToUnityVector3();

            if (origin != destination)
            {
                var vertices = DrawingUtilities.VerticesOnGreatArc(origin, destination, NumberOfVerticesPerEdge);
                return vertices.ToArray();
            }
            else
            {
                return new Vector3[] {};
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
                var verticesOfArc = DrawingUtilities.VerticesOnGreatArc(edgeList[i-1].ToUnityVector3(), edgeList[i].ToUnityVector3(), NumberOfVerticesPerEdge);
                vertices.AddRange(verticesOfArc); 
            }
            return vertices.ToArray();
        }
    }
}