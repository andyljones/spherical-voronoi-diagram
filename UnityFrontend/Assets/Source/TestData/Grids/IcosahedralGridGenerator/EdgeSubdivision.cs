using System.Collections.Generic;
using Foam;

namespace Grids.IcosahedralGridGenerator
{
    /// <summary>
    /// This is a service class for the IcosahedralGridGenerator. It takes a list of edges and divides each into two
    /// new edges with a new vertex in the middle.
    /// </summary>
    public class EdgeSubdivision
    {
        public List<Vertex> Vertices { get; private set; }
        public List<Edge> Edges { get; private set; }

        public EdgeSubdivision(List<Edge> oldEdges, List<Vertex> vertices)
        {
            Vertices = vertices;
            Edges = new List<Edge>();

            SubdivideEdges(oldEdges);
        }

        private void SubdivideEdges(List<Edge> oldEdges)
        {
            foreach (var oldEdge in oldEdges)
            {
                SubdivideEdge(oldEdge);
            }
        }

        private void SubdivideEdge(Edge oldEdge)
        {
            CreateSubEdgesFrom(oldEdge);
            RemoveReferencesTo(oldEdge);
        }

        // Divides an edge in two at its midpoint and creates the neccessary references to the new objects in  
        // neighbouring vertices and faces
        private void CreateSubEdgesFrom(Edge oldEdge)
        {
            var vertexA = oldEdge.Vertices[0];
            var vertexB = oldEdge.Vertices[1];

            var midpoint = new Vertex { Position = (vertexA.Position + vertexB.Position) / 2 };
            var newEdgeX = new Edge { Vertices = new List<Vertex> { vertexA, midpoint } };
            var newEdgeY = new Edge { Vertices = new List<Vertex> { vertexB, midpoint } };

            CreateReferencesForNewVertex(midpoint, oldEdge);
            CreateReferencesForNewEdge(newEdgeX, oldEdge);
            CreateReferencesForNewEdge(newEdgeY, oldEdge);
        }
        // Creates the necessary references in the icosahedral data structure for the new vertex.
        private void CreateReferencesForNewVertex(Vertex midpoint, Edge oldEdge)
        {
            Vertices.Add(midpoint);

            oldEdge.Faces[0].Vertices.Add(midpoint);
            oldEdge.Faces[1].Vertices.Add(midpoint);
        }

        // Creates the necessary references in the icosahedral data structure for the new vertex.
        private void CreateReferencesForNewEdge(Edge newEdge, Edge oldEdge)
        {
            Edges.Add(newEdge);

            newEdge.Vertices[0].Edges.Add(newEdge);
            newEdge.Vertices[1].Edges.Add(newEdge);

            oldEdge.Faces[0].Edges.Add(newEdge);
            oldEdge.Faces[1].Edges.Add(newEdge);
        }

        // Removes the references to the edge from each adjacent vertex and face.
        private void RemoveReferencesTo(Edge edge)
        {
            edge.Vertices[0].Edges.Remove(edge);
            edge.Vertices[1].Edges.Remove(edge);

            edge.Faces[0].Edges.Remove(edge);
            edge.Faces[1].Edges.Remove(edge);
        }
    }
}
