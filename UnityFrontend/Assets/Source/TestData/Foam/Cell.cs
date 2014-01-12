using System.Collections.Generic;
using UnityEngine;

namespace Foam
{
    public class Cell
    {
        public List<Face> Faces = new List<Face>();
        public List<Edge> Edges = new List<Edge>();
        public List<Vertex> Vertices = new List<Vertex>();

        public float Height;
        public Vector3 Velocity = new Vector3();
    }
}
