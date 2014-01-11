using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace Generator
{
    public class EdgeSet
    {
        private readonly Dictionary<IArc, List<Vector3>> _edges = new Dictionary<IArc, List<Vector3>>();
        private readonly Dictionary<IArc, SiteEvent> _currentNeighbour = new Dictionary<IArc, SiteEvent>();

        private readonly Sweepline _sweepline;

        public EdgeSet(Sweepline sweepline)
        {
            _sweepline = sweepline;
        }

        public void CheckForNewEdge(IArc arc)
        {
            if (!_currentNeighbour.ContainsKey(arc))
            {
                _currentNeighbour.Add(arc, arc.LeftNeighbour);
                _edges.Add(arc, new List<Vector3> {arc.PointOfIntersection(_sweepline)});
            }
            else if (_currentNeighbour[arc] != arc.LeftNeighbour)
            {
                _currentNeighbour[arc] = arc.LeftNeighbour;
                _edges[arc].Add(arc.PointOfIntersection(_sweepline));
            }
        }

        public void CheckForNewEdges(IEnumerable<CircleEvent> circleEvents)
        {
            foreach (var circleEvent in circleEvents)
            {
                CheckForNewEdge(circleEvent.LeftArc);
                CheckForNewEdge(circleEvent.MiddleArc);
                CheckForNewEdge(circleEvent.RightArc);
            }
        }

        //TODO: Replace with "ModifiedArcs" list
        public void TerminateArc(IArc arc)
        {
            _currentNeighbour[arc] = arc.LeftNeighbour;
            _edges[arc].Add(arc.PointOfIntersection(_sweepline));
        }

        public Dictionary<IArc, List<Vector3>> CurrentEdgeDict()
        {
            return _edges;
        }
    }
}
