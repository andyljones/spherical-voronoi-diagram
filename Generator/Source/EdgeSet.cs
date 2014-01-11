using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace Generator
{
    public class EdgeSet
    {
        private readonly Dictionary<IArc, List<Vector3>> _edges = new Dictionary<IArc, List<Vector3>>();

        private readonly Sweepline _sweepline;

        public EdgeSet(Sweepline sweepline)
        {
            _sweepline = sweepline;
        }

        public void UpdateArcs(List<IArc> arcs)
        {
            arcs.ForEach(arc => UpdateArc(arc));
        }

        public void UpdateArc(IArc arc)
        {
            if (!_edges.ContainsKey(arc))
            {
                _edges.Add(arc, new List<Vector3> { arc.PointOfIntersection(_sweepline) });
            }
            else
            {
                _edges[arc].Add(arc.PointOfIntersection(_sweepline));
            }
        }

        public Dictionary<IArc, List<Vector3>> CurrentEdgeDict()
        {
            return _edges;
        }
    }
}
