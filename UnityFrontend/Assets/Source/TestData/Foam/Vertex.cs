using System.Collections.Generic;
using UnityEngine;

namespace Foam
{
    public class Vertex
    {
        public List<Cell> Cells = new List<Cell>();

        public List<Face> Faces = new List<Face>();

        public List<Edge> Edges = new List<Edge>();

        public Vector3 Position;

        public Vertex()
        {
            Position = new Vector3(0, 0, 0);
        }

        public Vertex(float x, float y, float z)
        {
            Position.x = x;
            Position.y = y;
            Position.z = z;
        }
    }
}
