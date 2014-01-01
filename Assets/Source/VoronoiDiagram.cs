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
    public IPriorityQueue<CircleEvent> CircleEventQueue { get; private set; }

    public Beachline Beachline;

    public VoronoiDiagram(IEnumerable<Vector3> positions)
    {
        SiteEventQueue = new IntervalHeap<SiteEvent>();
        CircleEventQueue = new IntervalHeap<CircleEvent>();

        SiteEventQueue.AddAll(positions.Select(position => new SiteEvent(position)));
        SiteEvents = SiteEventQueue.ToList();
        Beachline = new Beachline();
    }

    public void ProcessNextEvent()
    {
        if ((!SiteEventQueue.IsEmpty && CircleEventQueue.IsEmpty) || 
            (!SiteEventQueue.IsEmpty && SiteEventQueue.FindMax().Priority > CircleEventQueue.FindMax().Priority))
        {
            var site = SiteEventQueue.DeleteMax();
            var circles = Beachline.Insert(site);
            CircleEventQueue.AddAll(circles);
        }
        else if ((!CircleEventQueue.IsEmpty && SiteEventQueue.IsEmpty) || 
            (!CircleEventQueue.IsEmpty && SiteEventQueue.FindMax().Priority < CircleEventQueue.FindMax().Priority))
        {
            var circle = CircleEventQueue.DeleteMax();
            Beachline.Remove(circle);
        }

        Debug.Log(String.Join(", ", CircleEventQueue.Select(circle => circle.ToString()).ToArray()));
    }
}