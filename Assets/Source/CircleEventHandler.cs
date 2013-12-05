using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5;
using Events;
using UnityEngine;

public class CircleEventHandler
{
    public List<VoronoiVertex> VoronoiGraph; 

    private readonly BeachLine _beachLine;
    private readonly EventQueue _eventQueue;
    private readonly CircleEventFinder _finder;

    public CircleEventHandler(BeachLine beachLine, EventQueue queue)
    {
        VoronoiGraph = new List<VoronoiVertex>();
        _beachLine = beachLine;
        _eventQueue = queue;
        _finder = new CircleEventFinder(beachLine);
    }

    public void Handle(CircleEvent circleEvent)
    {
        var centralArc = circleEvent.CentralArc;
        var predecessorArc = _beachLine.CircularPredecessor(centralArc);
        var successorArc = _beachLine.CircularSuccessor(centralArc);

        VoronoiGraph.Add(new VoronoiVertex { centralArc.Site, predecessorArc.Site, successorArc.Site });

        if (_beachLine.TryRemove(circleEvent.CentralArc))
        {
            var pointOnSweepLine = circleEvent.Position;
            
            _eventQueue.TryDelete(predecessorArc.CircleEventHandle);
            predecessorArc.CircleEventHandle = null;
            CheckArcForCircleEvent(predecessorArc, pointOnSweepLine);
            
            _eventQueue.TryDelete(successorArc.CircleEventHandle);
            successorArc.CircleEventHandle = null;
            CheckArcForCircleEvent(successorArc, pointOnSweepLine);
        }
    }

    private void CheckArcForCircleEvent(Arc arcToCheck, Vector3 pointOnSweepline)
    {
        var circleEvent = _finder.Check(arcToCheck, pointOnSweepline);
        if (circleEvent != null)
        {
            IPriorityQueueHandle<IEvent> handle = null;
            _eventQueue.Add(ref handle, circleEvent);
            arcToCheck.CircleEventHandle = handle;
        }
    }
}