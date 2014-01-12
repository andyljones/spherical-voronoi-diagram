using System.Collections.Generic;
using System.Linq;
using Foam;
using Grids.IcosahedralGridGenerator;
using UnityEngine;

//TODO: ADD (more) TESTS, needs refactoring
namespace Grids.GeodesicGridGenerator
{
    public class GeodesicGrid : IGrid
    {
        public List<Face> Faces { get; private set; }
            
        public GeodesicGrid(IIcosahedralGridOptions options)
        {
            var icosahedralGrid = new IcosahedralGrid(options);

            var icosahedralFaces = icosahedralGrid.Faces;
            var icosahedralEdges = icosahedralFaces.SelectMany(face => face.Edges).Distinct().ToList();
            var icosahedralVertices = icosahedralFaces.SelectMany(face => face.Vertices).Distinct().ToList();

            var icosahedralFaceToGeodesicVertexMap = CreateGeodesicVertices(icosahedralFaces);
            var icosahedralEdgeToGeodesicEdgeMap = CreateGeodesicEdges(icosahedralEdges, icosahedralFaceToGeodesicVertexMap);
            var icosahedralVertexToGeodesicFaceMap = CreateGeodesicFaces(icosahedralVertices, icosahedralEdgeToGeodesicEdgeMap);

            Faces = icosahedralVertexToGeodesicFaceMap.Values.ToList();
        }

        private Dictionary<Vertex, Face> CreateGeodesicFaces(List<Vertex> icosahedralVertices, Dictionary<Edge, Edge> icosahedralEdgeToGeodesicEdgeMap)
        {
            var geodesicFaces = new Dictionary<Vertex, Face>();

            foreach (var vertex in icosahedralVertices)
            {
                var geodesicEdges = vertex.Edges.Select(edge => icosahedralEdgeToGeodesicEdgeMap[edge]).ToList();
                var geodesicVertices = geodesicEdges.SelectMany(edge => edge.Vertices).Distinct().ToList();

                var geodesicFace = new Face {Vertices = geodesicVertices, Edges = geodesicEdges};

                foreach (var edge in geodesicEdges)
                {
                    edge.Faces.Add(geodesicFace);
                }

                foreach (var geodesicVertex in geodesicVertices)
                {
                    geodesicVertex.Faces.Add(geodesicFace);
                }

                geodesicFaces.Add(vertex, geodesicFace);
            }

            return geodesicFaces;
        }

        private Dictionary<Edge,Edge> CreateGeodesicEdges(List<Edge> icosahedralEdges, Dictionary<Face, Vertex> icosahedralFaceToGeodesicVertexMap)
        {
            var geodesicEdges = new Dictionary<Edge, Edge>();

            foreach (var edge in icosahedralEdges)
            {
                var geodesicEndpoints = edge.Faces.Select(face => icosahedralFaceToGeodesicVertexMap[face]).ToList();

                var geodesicEdge = new Edge {Vertices = geodesicEndpoints};
                
                foreach (var vertex in geodesicEndpoints)
                {
                    vertex.Edges.Add(geodesicEdge);
                }

                geodesicEdges.Add(edge, geodesicEdge);
            }

            return geodesicEdges;
        }

        private Dictionary<Face, Vertex> CreateGeodesicVertices(List<Face> icosahedralFaces)
        {
            var geodesicVertices = new Dictionary<Face, Vertex>();

            foreach (var face in icosahedralFaces)
            {
                var centerOfFace = FoamUtils.Center(face);

                var corners = face.Vertices.Select(vertex => vertex.Position).ToList();
                var clockwiseComparer = new CompareVectorsClockwise(centerOfFace, corners.First());
                var orderedCorners = corners.OrderBy(corner => corner, clockwiseComparer).ToList();

                var edgeA = orderedCorners[1].normalized - orderedCorners[0].normalized;
                var edgeB = orderedCorners[2].normalized - orderedCorners[0].normalized;
                var voronoiPoint = Vector3.Cross(edgeA, edgeB).normalized;

                var geodesicVertex = new Vertex {Position = voronoiPoint};
                geodesicVertices.Add(face, geodesicVertex);
            }

            return geodesicVertices;
        }
    }
}