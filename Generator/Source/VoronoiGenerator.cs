using System.Collections.Generic;
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
            if (ASiteEventIsNext())
            {
                Beachline.Insert(SiteEventQueue.DeleteMax());
                CircleEventQueue.TryInsertAll(Beachline.PotentialCircleEvents);
                Edges.UpdateArcs(Beachline.EdgeUpdates);
                //Edges.CheckForNewEdges(Beachline.PotentialCircleEvents);
                Beachline.ClearPotentialCircleEventList();
                Beachline.ClearEdgeUpdates();
            }
            else if (ACircleEventIsNext())
            {
                var circleEvent = CircleEventQueue.PopHighestPriorityEvent();
                Beachline.Remove(circleEvent);
                CircleEventQueue.TryInsertAll(Beachline.PotentialCircleEvents);
                Edges.UpdateArcs(Beachline.EdgeUpdates);
                //Edges.CheckForNewEdges(Beachline.PotentialCircleEvents);
                //Edges.TerminateArc(circleEvent.MiddleArc);
                Beachline.ClearPotentialCircleEventList();
                Beachline.ClearEdgeUpdates();
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