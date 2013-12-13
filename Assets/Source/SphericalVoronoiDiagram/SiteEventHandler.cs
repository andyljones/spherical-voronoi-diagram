using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Source.SphericalVoronoiDiagram.Events;
using C5;
using Events;
using UnityEngine;

public class SiteEventHandler
{
    public List<VoronoiVertex> VoronoiGraph; 

    private readonly BeachLine _beachLine;
    private readonly EventQueue _eventQueue;
    private readonly CircleEventFinder _finder;

    public SiteEventHandler(BeachLine beachLine, EventQueue queue)
    {
        VoronoiGraph = new List<VoronoiVertex>();
        _beachLine = beachLine;
        _eventQueue = queue;
        _finder = new CircleEventFinder(beachLine);
    }

    public void Handle(SiteEvent site)
    {
        CreateArcAt(site);
    }

    private void CreateArcAt(SiteEvent site)
    {
        var newArc = new Arc {Site = site};
        var containingArcs = _beachLine.ArcsContaining(newArc);

        if (_beachLine.Count == 0)
        {
            _beachLine.Add(newArc);
        }
        else if (_beachLine.Count == 1)
        {
            CreateArcInSingleElementBeachline(newArc);
        }
        else if (_beachLine.Count > 1 && containingArcs.Count == 1)
        {
            CreateArcWithinArc(containingArcs.Single(), newArc);
        }
        else if (_beachLine.Count > 1 && containingArcs.Count == 2)
        {
            CreateArcBetweenArcs(containingArcs, newArc);
        }
        else
        {
            throw new InvalidDataException("More than two arcs intersected by the site!");
        }
    }

    private void CreateArcInSingleElementBeachline(IArc newArc)
    {
        var onlyCurrentArc = _beachLine.Single();
        onlyCurrentArc.ConnectToLeftOf(newArc);
        newArc.ConnectToLeftOf(onlyCurrentArc);
        _beachLine.Add(newArc);
        //TODO: What to do if they're the same polar angle?
    }

    private void CreateArcWithinArc(IArc oldArc, IArc newArc)
    {
        _beachLine.Remove(oldArc);
        _eventQueue.TryDelete(oldArc.CircleEventHandle);
        
        var newNeighbouringArcs = oldArc.SplitAt(newArc);
        
        _beachLine.AddAll(newNeighbouringArcs);
        _beachLine.Add(newArc);

        var pointOnSweepline = newArc.Site.Position;
        CheckArcForCircleEvent(newNeighbouringArcs.First(), pointOnSweepline);
        CheckArcForCircleEvent(newNeighbouringArcs.Last(), pointOnSweepline);
    }

    private void CreateArcBetweenArcs(List<IArc> oldArcs, IArc newArc)
    {
        oldArcs[0].ConnectToLeftOf(newArc);
        oldArcs[1].ConnectToRightOf(newArc);

        _beachLine.Add(newArc);

        VoronoiGraph.Add(new VoronoiVertex { oldArcs.First().Site, oldArcs.Last().Site, newArc.Site });
    }

    private void CheckArcForCircleEvent(IArc arcToCheck, Vector3 pointOnSweepline)
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