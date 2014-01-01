using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using CyclicalSkipList;
using UnityEngine;

public class Beachline : IEnumerable<Arc>
{
    private readonly Skiplist<Arc> _arcs = new Skiplist<Arc> {InOrder = Arc.AreInOrder};
    public readonly Sweepline Sweepline = new Sweepline(2);

    //TODO: Test.
    public int Count { get; private set; }

    public IEnumerable<Arc> Insert(SiteEvent siteEvent)
    {
        Sweepline.Z = siteEvent.Position.z;
        if (Count <= 1)
        {
            return InsertOneOfFirstTwoSites(siteEvent);
        }
        else
        {
            return InsertSiteOtherThanTheFirstTwo(siteEvent);
        }
    }

    private IEnumerable<Arc> InsertOneOfFirstTwoSites(SiteEvent siteEvent)
    {
        var arc = new Arc(siteEvent, Sweepline);
        _arcs.Add(arc);

        var node = _arcs.FetchNode(arc);
        arc.LeftNeighbour = node.Left.Key.SiteEvent;
        arc.RightNeighbour = node.Right.Key.SiteEvent;

        node.Left.Key.RightNeighbour = arc.SiteEvent;
        node.Right.Key.LeftNeighbour = arc.SiteEvent;

        Count = Count + 1;

        return new List<Arc>();
    }

    private IEnumerable<Arc> InsertSiteOtherThanTheFirstTwo(SiteEvent siteEvent)
    {
        var arcA = new Arc(siteEvent, Sweepline);

        var arcBeingSplit = _arcs.FetchNode(arcA).Key;

        arcA.LeftNeighbour = arcBeingSplit.SiteEvent;
        arcA.RightNeighbour = arcBeingSplit.SiteEvent;
        _arcs.Add(arcA);

        var arcB = new Arc(arcBeingSplit.SiteEvent, Sweepline)
        {
            LeftNeighbour = siteEvent,
            RightNeighbour = arcBeingSplit.RightNeighbour
        };

        arcBeingSplit.RightNeighbour = siteEvent;

        _arcs.Add(arcB);

        Count = Count + 2;

        var newCircleEvents = new List<Arc> { arcBeingSplit, arcB };

        return newCircleEvents;
    }

    //TODO: Test.
    public bool Remove(Arc arc)
    {
        Sweepline.Z = arc.SiteEvent.Position.z;
        var node = _arcs.FetchNode(arc);
        node.Left.Key.RightNeighbour = arc.RightNeighbour;
        node.Right.Key.LeftNeighbour = arc.LeftNeighbour;

        var removalSuccessful = _arcs.Remove(arc);

        Count = Count - 1;

        return removalSuccessful;
    }

    public IEnumerator<Arc> GetEnumerator()
    {
        return _arcs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return BeachlineStringFormatter.ConvertToString(this);
    }
}