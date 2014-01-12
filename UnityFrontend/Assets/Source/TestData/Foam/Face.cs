using System.Collections.Generic;

namespace Foam
{
    public class Face
    {
        public List<Cell> Cells = new List<Cell>();

        public List<Edge> Edges = new List<Edge>();

        public List<Vertex> Vertices = new List<Vertex>();
    }
}
