using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Grids.IcosahedralGridGenerator
{

    /// <summary>
    /// This is a service class for the IcosahedralGridGenerator. It subdivides each triangular face of the
    /// icosphere into four new triangular faces.
    /// </summary>
    public class FaceSubdivision
    {
        public List<Vertex> Vertices { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Face> Faces { get; private set; }  

        public FaceSubdivision(List<Face> oldFaces, List<Edge> edges, List<Vertex> vertices)
        {
            Faces = new List<Face>();
            Edges = edges;
            Vertices = vertices;

            SubdivideFaces(oldFaces);
        }

        private void SubdivideFaces(List<Face> oldFaces)
        {
            foreach (var face in oldFaces)
            {
                SubdivideFace(face);
            }
        }

        // This divides a face into four new faces. The tricky bit is that to get the indexing right, it has to 
        // distinguish between north-pointing and south-pointing faces.
        private void SubdivideFace(Face face)
        {
            var zSortedVertices = face.Vertices.OrderBy(vertex => vertex.Position.z);
            var baselineVertex = IsNorthPointing(face) ? zSortedVertices.Last() : zSortedVertices.First();

            var sortedVertices = SortBoundaryVertices(face, baselineVertex);

            // Diagrams of the possible layouts of sortedVertices and the subfaces they form.
            //
            //     0
            //    / \    <-- Subface A
            //   5---1
            //  / \ / \  <-- Subfaces C, D, B
            // 4---3---2
            //
            // 2---3---4
            //  \ / \ /     <-- Subfaces B, D, C
            //   1---5
            //    \ /       <-- Subface A
            //     0

            var subfaceA = CreateSubface(sortedVertices[5], sortedVertices[0], sortedVertices[1]);
            var subfaceB = CreateSubface(sortedVertices[1], sortedVertices[2], sortedVertices[3]);
            var subfaceC = CreateSubface(sortedVertices[3], sortedVertices[4], sortedVertices[5]);

            var subfaceD = CreateCentralSubface(sortedVertices[1], sortedVertices[3], sortedVertices[5]);

            RemoveReferencesToFace(face);

            Faces.AddRange(new List<Face> { subfaceA, subfaceB, subfaceC, subfaceD });
        }

        // This sorts the vertices of a face clockwise from a given "baseline" vertex.
        private Vertex[] SortBoundaryVertices(Face face, Vertex baseline)
        {
            var center = CenterOfFace(face);

            // So we sort the vertices clockwise from 12 o'clock (global north)...
            var clockwiseFromBaseline = new CompareVectorsClockwise(center, new Vector3(0, 0, 1));
            var clockwiseSortedVertices = face.Vertices.OrderBy(vertex => vertex.Position, clockwiseFromBaseline).ToList();
            
            // ...and then cycle the elements of the array so that the first vertex in the array is the baseline.
            var indexOfBaseline = clockwiseSortedVertices.IndexOf(baseline);
            var sortedVertices = new Vertex[clockwiseSortedVertices.Count];

            for (int i = 0; i < sortedVertices.Length; i++)
            {
                sortedVertices[i] = clockwiseSortedVertices[MathMod(indexOfBaseline + i, clockwiseSortedVertices.Count)];
            }

            return sortedVertices;
        }

        private void RemoveReferencesToFace(Face oldFace)
        {
            foreach (var vertex in oldFace.Vertices)
            {
                vertex.Faces.Remove(oldFace);
            }

            foreach (var edge in oldFace.Edges)
            {
                edge.Faces.Remove(oldFace);
            }
        }

        private Face CreateCentralSubface(Vertex u, Vertex v, Vertex w)
        {
            var vertices = new List<Vertex> { u, v, w };

            var newFace = new Face
            {
                Vertices = vertices,
                Edges =  FindEdgesBetween(vertices)
            };

            AddFaceToEdgesAndVertices(newFace);

            return newFace;
        }

        private Face CreateSubface(Vertex u, Vertex v, Vertex w)
        {
            var vertices = new List<Vertex> { u, v, w };

            var edges = FindEdgesBetween(vertices);
            var newEdge = CreateEdge(u, w);

            edges.Add(newEdge);

            var newFace = new Face
            {
                Vertices = vertices,
                Edges = edges
            };

            AddFaceToEdgesAndVertices(newFace);

            return newFace;
        }

        private Edge CreateEdge(Vertex u, Vertex v)
        {
            var newEdge = new Edge { Vertices = new List<Vertex> { u, v } };
            u.Edges.Add(newEdge);
            v.Edges.Add(newEdge);

            Edges.Add(newEdge);

            return newEdge;
        }

        private void AddFaceToEdgesAndVertices(Face face)
        {
            foreach (var edge in face.Edges)
            {
                edge.Faces.Add(face);
            }

            foreach (var vertex in face.Vertices)
            {
                vertex.Faces.Add(face);
            }
        }

        private List<Edge> FindEdgesBetween(List<Vertex> vertices)
        {
            var allNeighbouringEdges = vertices.SelectMany(vertex => vertex.Edges).ToList();
            var distinctNeighbouringEdges = allNeighbouringEdges.Distinct();
            // If an edge appears in the neighbour list twice, it must be a neighbour of two vertices, and hence connects them.
            var connectingEdges = distinctNeighbouringEdges.Where(e => allNeighbouringEdges.Count(f => (e == f)) > 1).ToList();

            return connectingEdges;
        }

        private bool IsNorthPointing(Face face)
        {
            var zCoords = face.Vertices.Select(vertex => vertex.Position.z);
            var middle = (zCoords.Max() + zCoords.Min()) / 2;
            var mean = zCoords.Average();

            // If the mean z-coordinate is less than half-way up the face, the face must have more vertices on the bottom 
            // than the top, and as such must be pointing north.
            return mean < middle;
        }

        private Vector3 CenterOfFace(Face face)
        {
            var vertexPositions = face.Vertices.Select(vertex => vertex.Position).ToList();
            var averageVertexPosition = vertexPositions.Aggregate((u, v) => u + v) / vertexPositions.Count();

            return averageVertexPosition;
        }

        // This implements the mathematical (x modulo m) operator. The built in (x % m) operator implements (x remainder m).
        private int MathMod(int x, int m)
        {
            return ((x % m) + m) % m;
        }
    }
}
