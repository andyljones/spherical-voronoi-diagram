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
            rightVertex.Sites.AddRange(new List<SiteEvent> { arc.Site, rightNeighbour.Site });

            var leftVertex = new Vertex(arc, _sweepline);
            _vertexLists.Add(arc, new List<Vertex> {rightVertex, leftVertex});
            leftVertex.Sites.AddRange(new List<SiteEvent> { arc.LeftNeighbour, arc.Site });
        }

        public void CircleEvent(CircleEvent circle)
        {
            FixPosition(circle.MiddleArc, circle);
            FixPosition(circle.RightArc, circle);

            if (circle.LeftArc.LeftNeighbour == circle.RightArc.Site)
            {
                FixPosition(circle.LeftArc, circle);
            }
            else
            {
                _vertexLists[circle.RightArc].Add(new Vertex(circle.RightArc, _sweepline));
            }
        }

        private void FixPosition(IArc arc, CircleEvent circle)
        {
            var vertex = _vertexLists[arc].Last();
            vertex.Position = circle.Center();
            vertex.Sites = new List<SiteEvent> { circle.LeftArc.Site, circle.MiddleArc.Site, circle.RightArc.Site };
        }

        public Dictionary<IArc, List<Vertex>> VertexLists()
        {
            return _vertexLists;
        }
    }
}
