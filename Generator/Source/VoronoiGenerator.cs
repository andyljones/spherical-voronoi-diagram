using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using C5;

namespace Generator
{
    public class VoronoiDiagram
    {
        public readonly IEnumerable<SiteEvent> SiteEvents;

        public IPriorityQueue<SiteEvent> SiteEventQueue { get; private set; }
        public CircleEventQueue CircleEventQueue { get; private set; }
        public EdgeSet Edges { get; private set; }

        public Beachline Beachline;

        public VoronoiDiagram(IEnumerable<double[]> positions)
        {
            SiteEventQueue = new IntervalHeap<SiteEvent>();
            SiteEventQueue.AddAll(positions.Select(position => new SiteEvent(new Vector3(position[0], position[1], position[2]))));
            SiteEvents = SiteEventQueue.ToList();

            var terminatingPriority = -SiteEventQueue.FindMin().Priority;
            CircleEventQueue = new CircleEventQueue(terminatingPriority);

            Beachline = new Beachline();

            Edges = new EdgeSet(Beachline.Sweepline);
        }

        public void ProcessNextEvent()
        {
            if (ACircleEventIsNext())
            {
                var circleEvent = CircleEventQueue.PopHighestPriorityEvent();
                Beachline.Remove(circleEvent);
                CircleEventQueue.TryInsertAll(Beachline.PotentialCircleEvents);
                Edges.CircleEvent(circleEvent);
                Beachline.ClearPotentialCircleEventList();
                Beachline.ClearNewArcs();
            }
            else if (ASiteEventIsNext())
            {
                Beachline.Insert(SiteEventQueue.DeleteMax());
                CircleEventQueue.TryInsertAll(Beachline.PotentialCircleEvents);
                if (Beachline.NewArcs.Any())
                {
                    Edges.NewArc(Beachline.NewArcs[0][0], Beachline.NewArcs[0][1]);
                }
                Beachline.ClearPotentialCircleEventList();
                Beachline.ClearNewArcs();
            }
            
            if (CircleEventQueue.IsEmpty() && SiteEventQueue.IsEmpty)
            {
                Beachline.Clear();
            }
        }

        private bool ASiteEventIsNext()
        {
            return (!SiteEventQueue.IsEmpty && CircleEventQueue.IsEmpty()) ||
                   (!SiteEventQueue.IsEmpty && SiteEventQueue.FindMax().Priority > CircleEventQueue.HighestPriority());
        }

        private bool ACircleEventIsNext()
        {
            return (!CircleEventQueue.IsEmpty() && SiteEventQueue.IsEmpty) ||
                   (!CircleEventQueue.IsEmpty() && SiteEventQueue.FindMax().Priority <= CircleEventQueue.HighestPriority());
        }
    }
}