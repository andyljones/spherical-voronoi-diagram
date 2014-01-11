using System.Collections.Generic;
using System.Linq;

namespace Generator
{
    public class EdgeSet
    {
        private readonly Dictionary<IArc, List<Vertex>> _vertexLists = new Dictionary<IArc, List<Vertex>>();

        private readonly Sweepline _sweepline;

        public EdgeSet(Sweepline sweepline)
        {
            _sweepline = sweepline;
        }
        
        public void NewArc(IArc arc, IArc rightNeighbour)
        {
            var rightVertex = new Vertex(rightNeighbour, _sweepline);
            _vertexLists.Add(rightNeighbour, new List<Vertex> {rightVertex});

            var leftVertex = new Vertex(arc, _sweepline);
            _vertexLists.Add(arc, new List<Vertex> {rightVertex, leftVertex});
        }

        public void CircleEvent(CircleEvent circle)
        {
            var intersection = circle.Center();
            FixPosition(circle.MiddleArc, intersection);
            FixPosition(circle.RightArc, intersection);

            if (circle.LeftArc.LeftNeighbour == circle.RightArc.Site)
            {
                FixPosition(circle.LeftArc, intersection);
            }
            else
            {
                _vertexLists[circle.RightArc].Add(new Vertex(circle.RightArc, _sweepline));
            }
        }

        private void FixPosition(IArc arc, Vector3 position)
        {
            _vertexLists[arc].Last().Position = position;
        }

        public Dictionary<IArc, List<Vertex>> VertexLists()
        {
            return _vertexLists;
        }
    }
}
