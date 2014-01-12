using System.Collections.Generic;
using System.Linq;
using Foam;
using UnityEngine;

namespace Grids.IcosahedralGridGenerator
{   
    /// <summary>
    /// Generates a unit-radius icosahedron as a list of faces, edges and vertices, all linked to eachother. 
    /// </summary>
    public class Icosahedron
    {
        public List<Vertex> Vertices;

        public List<Edge> Edges;

        public List<Face> Faces;

        public Icosahedron()
        {
            CreateVertices();
            CreateEdgesFromVertices();
            CreateFacesFromEdges();

            // Now we need to construct all the interlinking references:
            AddEachVertexToItsNeighbouringFaces();

            AddEachEdgeToItsEndpointVertices();
            AddEachFaceToItsVerticesAndEdges();
        }

        #region CreateVertices methods.

        private void CreateVertices()
        {
            Vertices = Enumerable.Repeat(default(Vertex), 12).ToList();

            // North pole
            Vertices[0] = new Vertex { Position = new Vector3(0, 0, 1)};

            // Northern latitude
            Vertices[1] = new Vertex { Position = new Vector3(xCoordOf(0), yCoordOf(0), 1 / Mathf.Sqrt(5)) };
            Vertices[2] = new Vertex { Position = new Vector3(xCoordOf(2), yCoordOf(2), 1 / Mathf.Sqrt(5)) };
            Vertices[3] = new Vertex { Position = new Vector3(xCoordOf(4), yCoordOf(4), 1 / Mathf.Sqrt(5)) };
            Vertices[4] = new Vertex { Position = new Vector3(xCoordOf(6), yCoordOf(6), 1 / Mathf.Sqrt(5)) };
            Vertices[5] = new Vertex { Position = new Vector3(xCoordOf(8), yCoordOf(8), 1 / Mathf.Sqrt(5)) };

            // SouthernLatitude
            Vertices[6] = new Vertex { Position = new Vector3(xCoordOf(1), yCoordOf(1), -1 / Mathf.Sqrt(5)) };
            Vertices[7] = new Vertex { Position = new Vector3(xCoordOf(3), yCoordOf(3), -1 / Mathf.Sqrt(5)) };
            Vertices[8] = new Vertex { Position = new Vector3(xCoordOf(5), yCoordOf(5), -1 / Mathf.Sqrt(5)) };
            Vertices[9] = new Vertex { Position = new Vector3(xCoordOf(7), yCoordOf(7), -1 / Mathf.Sqrt(5)) };
            Vertices[10] = new Vertex { Position = new Vector3(xCoordOf(9), yCoordOf(9), -1 / Mathf.Sqrt(5)) };

            // South pole
            Vertices[11] = new Vertex {Position = new Vector3(0, 0, -1)};
        }

        // There are ten distinct meridian used in an icosahedron. These two methods calculate their coordinates,
        // where the positive Y axis is taken to be the prime meridian.
        private float xCoordOf(int i)
        {
            return 2/Mathf.Sqrt(5)*Mathf.Sin(Mathf.PI/5*i);
        }

        private float yCoordOf(int i)
        {
            return 2/Mathf.Sqrt(5)*Mathf.Cos(Mathf.PI/5*i);
        }
        #endregion

        // Generates the edges in terms of the vertices. Doing it by hand is ugly as sin but doing it procedurally would be a lot worse.
        private void CreateEdgesFromVertices()
        {
            Edges = Enumerable.Repeat(default(Edge), 30).ToList();

            //Edges from north pole
            Edges[0] = new Edge { Vertices = new List<Vertex> { Vertices[0], Vertices[1] } };
            Edges[1] = new Edge { Vertices = new List<Vertex> { Vertices[0], Vertices[2] } };
            Edges[2] = new Edge { Vertices = new List<Vertex> { Vertices[0], Vertices[3] } };
            Edges[3] = new Edge { Vertices = new List<Vertex> { Vertices[0], Vertices[4] } };
            Edges[4] = new Edge { Vertices = new List<Vertex> { Vertices[0], Vertices[5] } };

            // Upper latitude
            Edges[5] = new Edge { Vertices = new List<Vertex> { Vertices[1], Vertices[2] } };
            Edges[6] = new Edge { Vertices = new List<Vertex> { Vertices[2], Vertices[3] } };
            Edges[7] = new Edge { Vertices = new List<Vertex> { Vertices[3], Vertices[4] } };
            Edges[8] = new Edge { Vertices = new List<Vertex> { Vertices[4], Vertices[5] } };
            Edges[9] = new Edge { Vertices = new List<Vertex> { Vertices[5], Vertices[1] } };

            // Middle edges, going down-up-down-up from the prime meridian
            Edges[10] = new Edge { Vertices = new List<Vertex> { Vertices[1], Vertices[6] } };
            Edges[11] = new Edge { Vertices = new List<Vertex> { Vertices[6], Vertices[2] } };
            Edges[12] = new Edge { Vertices = new List<Vertex> { Vertices[2], Vertices[7] } };
            Edges[13] = new Edge { Vertices = new List<Vertex> { Vertices[7], Vertices[3] } };
            Edges[14] = new Edge { Vertices = new List<Vertex> { Vertices[3], Vertices[8] } };
            Edges[15] = new Edge { Vertices = new List<Vertex> { Vertices[8], Vertices[4] } };
            Edges[16] = new Edge { Vertices = new List<Vertex> { Vertices[4], Vertices[9] } };
            Edges[17] = new Edge { Vertices = new List<Vertex> { Vertices[9], Vertices[5] } };
            Edges[18] = new Edge { Vertices = new List<Vertex> { Vertices[5], Vertices[10] } };
            Edges[19] = new Edge { Vertices = new List<Vertex> { Vertices[10], Vertices[1] } };

            //Lower latitude
            Edges[20] = new Edge { Vertices = new List<Vertex> { Vertices[6], Vertices[7] } };
            Edges[21] = new Edge { Vertices = new List<Vertex> { Vertices[7], Vertices[8] } };
            Edges[22] = new Edge { Vertices = new List<Vertex> { Vertices[8], Vertices[9] } };
            Edges[23] = new Edge { Vertices = new List<Vertex> { Vertices[9], Vertices[10] } };
            Edges[24] = new Edge { Vertices = new List<Vertex> { Vertices[10], Vertices[6] } };

            //Edges to south pole
            Edges[25] = new Edge { Vertices = new List<Vertex> { Vertices[6], Vertices[11] } };
            Edges[26] = new Edge { Vertices = new List<Vertex> { Vertices[7], Vertices[11] } };
            Edges[27] = new Edge { Vertices = new List<Vertex> { Vertices[8], Vertices[11] } };
            Edges[28] = new Edge { Vertices = new List<Vertex> { Vertices[9], Vertices[11] } };
            Edges[29] = new Edge { Vertices = new List<Vertex> { Vertices[10], Vertices[11] } };
        }

        // Generates the faces in terms of the edges. 
        private void CreateFacesFromEdges()
        {
            Faces = Enumerable.Repeat(default(Face), 20).ToList(); ;

            // Faces around north pole
            Faces[0] = new Face { Edges = new List<Edge> { Edges[0], Edges[1], Edges[5] }};
            Faces[1] = new Face { Edges = new List<Edge> { Edges[1], Edges[2], Edges[6] } };
            Faces[2] = new Face { Edges = new List<Edge> { Edges[2], Edges[3], Edges[7] } };
            Faces[3] = new Face { Edges = new List<Edge> { Edges[3], Edges[4], Edges[8] } };
            Faces[4] = new Face { Edges = new List<Edge> { Edges[4], Edges[0], Edges[9] } };

            // Middle faces
            Faces[5] = new Face { Edges = new List<Edge> { Edges[5], Edges[10], Edges[11] } };
            Faces[6] = new Face { Edges = new List<Edge> { Edges[20], Edges[11], Edges[12] } };
            Faces[7] = new Face { Edges = new List<Edge> { Edges[6], Edges[12], Edges[13] } };
            Faces[8] = new Face { Edges = new List<Edge> { Edges[21], Edges[13], Edges[14] } };
            Faces[9] = new Face { Edges = new List<Edge> { Edges[7], Edges[14], Edges[15] } };
            Faces[10] = new Face { Edges = new List<Edge> { Edges[22], Edges[15], Edges[16] } };
            Faces[11] = new Face { Edges = new List<Edge> { Edges[8], Edges[16], Edges[17] } };
            Faces[12] = new Face { Edges = new List<Edge> { Edges[23], Edges[17], Edges[18] } };
            Faces[13] = new Face { Edges = new List<Edge> { Edges[9], Edges[18], Edges[19] } };
            Faces[14] = new Face { Edges = new List<Edge> { Edges[24], Edges[19], Edges[10] } };

            // Lower faces
            Faces[15] = new Face { Edges = new List<Edge> { Edges[20], Edges[25], Edges[26] } };
            Faces[16] = new Face { Edges = new List<Edge> { Edges[21], Edges[26], Edges[27] } };
            Faces[17] = new Face { Edges = new List<Edge> { Edges[22], Edges[27], Edges[28] } };
            Faces[18] = new Face { Edges = new List<Edge> { Edges[23], Edges[28], Edges[29] } };
            Faces[19] = new Face { Edges = new List<Edge> { Edges[24], Edges[29], Edges[25] } };
        }

        private void AddEachEdgeToItsEndpointVertices()
        {
            foreach (var edge in Edges)
            {
                edge.Vertices[0].Edges.Add(edge);
                edge.Vertices[1].Edges.Add(edge);
            }
        }

        private void AddEachVertexToItsNeighbouringFaces()
        {
            foreach (var face in Faces)
            {
                face.Vertices = face.Edges.SelectMany(edge => edge.Vertices).Distinct().ToList();
            }
        }

        #region AddEachFaceToItsVerticesAndEdges region
        private void AddEachFaceToItsVerticesAndEdges()
        {
            foreach (var face in Faces)
            {
                AddFaceToItsEdges(face);
                AddFaceToItsVertices(face);
            }
        }

        private void AddFaceToItsEdges(Face face)
        {
            foreach (var edge in face.Edges)
            {
                edge.Faces.Add(face);
            }
        }

        private void AddFaceToItsVertices(Face face)
        {
            foreach (var vertex in face.Vertices)
            {
                vertex.Faces.Add(face);
            }
        }
        #endregion

        // Implements the (x mod m) operation. The standard (x % m) operator implements (x remainder m).
        private int MathMod(int x, int m)
        {
            return ((x%m) + m)%m;
        }
    }
}
