using System.Collections.Generic;
using System.IO;
using System.Linq;
using Events;

public class SiteEventHandler
{
    private readonly BeachLine _beachLine;
    private readonly EventQueue _eventQueue;
    private readonly CircleEventFinder _finder;

    public SiteEventHandler(BeachLine beachLine, EventQueue queue)
    {
        _beachLine = beachLine;
        _eventQueue = _eventQueue;
        _finder = new CircleEventFinder(beachLine);
    }

    public void Handle(SiteEvent site)
    {
        CreateArcAt(site);
    }

    private void CreateArcAt(SiteEvent site)
    {
        var newArc = new Arc(site);
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

    private void CreateArcInSingleElementBeachline(Arc newArc)
    {
        var onlyCurrentArc = _beachLine.Single();
        onlyCurrentArc.ConnectToLeftOf(newArc);
        newArc.ConnectToLeftOf(onlyCurrentArc);
        _beachLine.Add(newArc);
        //TODO: What to do if they're the same polar angle?
    }

    private void CreateArcWithinArc(Arc oldArc, Arc newArc)
    {
        _beachLine.Remove(oldArc);

        var newNeighbouringArcs = oldArc.SplitAt(newArc);
        _beachLine.AddAll(newNeighbouringArcs);
        _beachLine.Add(newArc);

        _finder.Check(newNeighbouringArcs.First(), newArc.Site.Position);
        _finder.Check(newNeighbouringArcs.Last(), newArc.Site.Position);
    }

    private void CreateArcBetweenArcs(List<Arc> oldArcs, Arc newArc)
    {
        oldArcs[0].ConnectToLeftOf(newArc);
        newArc.ConnectToLeftOf(oldArcs[1]);
        _beachLine.Add(newArc);

        _finder.Check(newArc, newArc.Site.Position);
    }
}