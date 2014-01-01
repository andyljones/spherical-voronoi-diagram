using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5;
using UnityEngine;

public class VoronoiDiagram
{
    public readonly IEnumerable<SiteEvent> SiteEvents;

    public IPriorityQueue<SiteEvent> SiteEventQueue { get; private set; }
    public CircleEventQueue CircleEventQueue { get; private set; }

    public Beachline Beachline;

    public VoronoiDiagram(IEnumerable<Vector3> positions)
    {
        SiteEventQueue = new IntervalHeap<SiteEvent>();
        SiteEventQueue.AddAll(positions.Select(position => new SiteEvent(position)));
        SiteEvents = SiteEventQueue.ToList();
        var terminatingPriority = SiteEventQueue.FindMin().Priority;
        CircleEventQueue = new CircleEventQueue(terminatingPriority);
        Beachline = new Beachline();
    }

    public void ProcessNextEvent()
    {
        if ((!SiteEventQueue.IsEmpty && CircleEventQueue.IsEmpty()) || 
            (!SiteEventQueue.IsEmpty && SiteEventQueue.FindMax().Priority > CircleEventQueue.FindMax().Priority))
        {
            var site = SiteEventQueue.DeleteMax();
            var arcs = Beachline.Insert(site);
            CircleEventQueue.UpdateArcs(arcs);
        }
        else if ((!CircleEventQueue.IsEmpty() && SiteEventQueue.IsEmpty) || 
            (!CircleEventQueue.IsEmpty() && SiteEventQueue.FindMax().Priority < CircleEventQueue.FindMax().Priority))
        {
            var circle = CircleEventQueue.DeleteMax();
            Beachline.Sweepline.Z = circle.Priority - 1;
            var arcs = Beachline.Remove(circle.Arc);
            CircleEventQueue.UpdateArcs(arcs);
        }
    }
}