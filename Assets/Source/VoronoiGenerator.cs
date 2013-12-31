using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5;
using UnityEngine;

public class VoronoiGenerator
{
    public readonly IEnumerable<SiteEvent> SiteEvents;
    public IEnumerable<CircleEvent> CircleEvents { get { return _circleEventQueue.ToList(); } } 

    private IPriorityQueue<SiteEvent> _siteEventQueue = new IntervalHeap<SiteEvent>();
    private IPriorityQueue<CircleEvent> _circleEventQueue = new IntervalHeap<CircleEvent>();

    public Beachline Beachline;

    public VoronoiGenerator(IEnumerable<Vector3> positions)
    {
        _siteEventQueue.AddAll(positions.Select(position => new SiteEvent(position)));
        SiteEvents = _siteEventQueue.ToList();
        Beachline = new Beachline();
    }

    public void ProcessNextEvent()
    {
        if (_circleEventQueue.IsEmpty || _siteEventQueue.FindMax().Priority > _circleEventQueue.FindMax().Priority)
        {
            var site = _siteEventQueue.FindMax();
            _siteEventQueue.DeleteMax();
            var circles = Beachline.Insert(site);
            _circleEventQueue.AddAll(circles);
        }
        else
        {
            var circle = _circleEventQueue.FindMax();
            _circleEventQueue.DeleteMax();
            Beachline.Remove(circle);
        }
    }
}