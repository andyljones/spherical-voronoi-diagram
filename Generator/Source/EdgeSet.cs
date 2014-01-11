using System.Collections.Generic;

namespace Generator
{
    public class EdgeSet
    {
        private readonly Dictionary<IArc, List<IVertex>> _vertexLists = new Dictionary<IArc, List<IVertex>>();
        private readonly Dictionary<IArc, LiveVertex> _liveVertices = new Dictionary<IArc, LiveVertex>();

        private readonly Sweepline _sweepline;

        public EdgeSet(Sweepline sweepline)
        {
            _sweepline = sweepline;
        }
        
        public void NewArc(IArc arc, IArc rightNeighbour)
        {
            var vertex = new LiveVertex(rightNeighbour, _sweepline);
            var vertexList = new List<IVertex> { vertex };
            _vertexLists.Add(arc, vertexList);
            _liveVertices.Add(rightNeighbour, vertex);
        }

        public void CircleEvent(CircleEvent circle)
        {
            if (_liveVertices.ContainsKey(circle.MiddleArc))
            {
                _liveVertices[circle.MiddleArc].Position = circle.Center();
                _liveVertices.Remove(circle.MiddleArc);

            }
            if (_liveVertices.ContainsKey(circle.RightArc))
            {
                _liveVertices[circle.RightArc].Position = circle.Center();
                _liveVertices.Remove(circle.RightArc);
            }

            var newVertex = new DeadVertex(circle);
            TryAdd(circle.MiddleArc, newVertex);
            TryAdd(circle.RightArc, newVertex);

            if (circle.LeftArc.LeftNeighbour == circle.RightArc.Site)
            {
                TryAdd(circle.LeftArc, newVertex);
            }
        }

        private void TryAdd(IArc arc, IVertex vertex)
        {
            if (_vertexLists.ContainsKey(arc))
            {
                _vertexLists[arc].Add(vertex);                
            }
            else
            {
                _vertexLists.Add(arc, new List<IVertex> { vertex });
            }
        }

        public Dictionary<IArc, List<IVertex>> CurrentEdgeDict()
        {
            return _vertexLists;
        }
    }
}
