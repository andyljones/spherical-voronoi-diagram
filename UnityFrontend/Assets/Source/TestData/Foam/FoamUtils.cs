using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Foam
{
    public static class FoamUtils
    {
        public static Vertex OtherEndpointFrom(this Edge edge, Vertex origin)
        {
            return edge.Vertices.Single(vertex => vertex != origin);
        }

        public static Cell NeighbourAcross(this Cell cell, Face face)
        {
            return face.Cells.Single(neighbour => neighbour != cell);
        }

        public static float HorizontalArea(this Cell cell)
        {
            var areaOfTopFace = Area(TopFace(cell));
            var areaOfBottomFace = Area(BottomFace(cell));

            return (areaOfTopFace + areaOfBottomFace)/2;
        }

        public static float VerticalWidth(this Face face)
        {
            var widthOfTopEdge = Length(TopEdge(face));
            var widthOfBottomEdge = Length(face.BottomEdge());

            return (widthOfTopEdge + widthOfBottomEdge)/2;
        }

        public static Edge BottomEdge(this Face face)
        {
            var verticesSortedByHeight = face.Vertices.OrderBy(vertex => vertex.Position.magnitude).ToList();

            var lowestVertex = verticesSortedByHeight[0];
            var secondLowestVertex = verticesSortedByHeight[1];

            var topEdge = lowestVertex.Edges.Intersect(secondLowestVertex.Edges).Single();

            return topEdge;
        }
        
        public static Edge TopEdge(this Face face)
        {
            var verticesSortedByHeight = face.Vertices.OrderByDescending(vertex => vertex.Position.magnitude).ToList();

            var highestVertex = verticesSortedByHeight[0];
            var secondHighestVertex = verticesSortedByHeight[1];

            var topEdge = highestVertex.Edges.Intersect(secondHighestVertex.Edges).Single();

            return topEdge;
        }

        public static float Length(this Edge edge)
        {
            return (edge.Vertices[0].Position - edge.Vertices[1].Position).magnitude;
        }

        public static float DistanceBetweenNeighbouringCellCenters(this Face face)
        {
            var cellA = face.Cells[0];
            var cellB = face.Cells[1];

            return cellA.DistanceTo(cellB);
        }

        public static float DistanceTo(this Cell cell, Cell neighbour)
        {
            var centerOfCell = cell.Center();
            var centerOfNeighbour = neighbour.Center();
            var angleBetweenCellAndNeighbour = Vector3.Angle(centerOfCell, centerOfNeighbour) * Mathf.PI / 180;
            var radius = (centerOfCell.magnitude + centerOfNeighbour.magnitude) / 2;
            var distanceBetweenCellAndNeighbour = radius*angleBetweenCellAndNeighbour;

            return distanceBetweenCellAndNeighbour;
        }

        public static float Area(this Face face)
        {
            var center = Center(face);
            var baseline = face.Vertices.First().Position;
            var clockwiseComparer = new CompareVectorsClockwise(center, baseline);
            var sortedVertices = face.Vertices.OrderBy(vertex => vertex.Position, clockwiseComparer).Reverse().ToList();

            var sumOfCrossProducts = new Vector3();

            for (int i = 0; i < sortedVertices.Count; i++)
            {
                var currentPosition = sortedVertices[i].Position;
                var nextPosition = sortedVertices[(i + 1) % sortedVertices.Count].Position;

                sumOfCrossProducts += Vector3.Cross(nextPosition, currentPosition);
            }

            var normalToFace = center.normalized;
            var area = Vector3.Dot(sumOfCrossProducts, normalToFace) / 2;

            return area;
        }

        public static Vector3 Center(this Cell cell)
        {
            var centerOfTopFace = Center(TopFace(cell));
            var centerOfBottomFace = Center(BottomFace(cell));

            var centerOfCell = (centerOfTopFace + centerOfBottomFace) / 2;

            return centerOfCell;
        }

        public static Vector3 Center(this Face face)
        {
            var sumOfVertexPositions = face.Vertices.Aggregate(new Vector3(), (position, vertex) => position + vertex.Position);
            var sumOfVertexMagnitudes = face.Vertices.Average(vertex => vertex.Position.magnitude);
            var centerOfFace = sumOfVertexPositions.normalized * sumOfVertexMagnitudes;

            return centerOfFace;
        }

        public static Vector3 Center(this Edge edge)
        {
            return (edge.Vertices.First().Position + edge.Vertices.Last().Position)/2;
        }

        public static List<Face> FacesWithNeighbours(this Cell cell)
        {
            return cell.Faces.Where(face => face.Cells.Count > 1).ToList();
        }

        public static List<Edge> VerticalEdges(this Cell cell)
        {
            var topFace = TopFace(cell);
            var bottomFace = BottomFace(cell);

            var verticalEdges = cell.Edges.Except(topFace.Edges).Except(bottomFace.Edges);

            return verticalEdges.ToList();
        }

        public static List<Edge> SortedClockwise(this List<Edge> edges)
        {
            var center = (edges.First().Center() + edges.Last().Center()) / 2;
            var baseline = edges.First().Center();
            var comparer = new CompareVectorsClockwise(center, baseline);
            var sortedEdges = edges.OrderBy(edge => edge.Center(), comparer);

            return sortedEdges.ToList();
        }

        public static List<Face> VerticalFaces(this Cell cell)
        {
            var faces = cell.Faces;
            faces.Remove(TopFace(cell));
            faces.Remove(BottomFace(cell));

            return faces;
        }

        public static Face TopFace(this Cell cell)
        {
            var verticesOrderedByHeight = cell.Vertices.OrderByDescending(vertex => vertex.Position.magnitude).ToList();

            var facesNeighbouringHighestVertex = verticesOrderedByHeight[0].Faces;
            var facesNeighbouringSecondHighestVertex = verticesOrderedByHeight[1].Faces;
            var facesNeighbouringThirdHighestVertex = verticesOrderedByHeight[2].Faces;

            var highestFace =
                facesNeighbouringHighestVertex
                .Intersect(facesNeighbouringSecondHighestVertex)
                .Intersect(facesNeighbouringThirdHighestVertex);

            return highestFace.Single();
        }

        public static Face BottomFace(this Cell cell)
        {
            var verticesOrderedByHeight = cell.Vertices.OrderBy(vertex => vertex.Position.magnitude).ToList();

            var facesNeighbouringLowestVertex = verticesOrderedByHeight[0].Faces;
            var facesNeighbouringSecondLowestVertex = verticesOrderedByHeight[1].Faces;
            var facesNeighbouringThirdLowestVertex = verticesOrderedByHeight[2].Faces;

            var lowestFace =
                facesNeighbouringLowestVertex
                .Intersect(facesNeighbouringSecondLowestVertex)
                .Intersect(facesNeighbouringThirdLowestVertex);

            return lowestFace.Single();
        }

        public static float Thickness(this Cell cell)
        {
            var topFace = TopFace(cell);
            var bottomFace = BottomFace(cell);
            var topFaceHeight = topFace.Vertices.Average(vertex => vertex.Position.magnitude);
            var bottomFaceHeight = bottomFace.Vertices.Average(vertex => vertex.Position.magnitude);

            return topFaceHeight - bottomFaceHeight;
        }

        public static List<Cell> Neighbours(this Cell cell)
        {
            return cell.FacesWithNeighbours().Select(face => cell.NeighbourAcross(face)).ToList();
        }
    }
}
