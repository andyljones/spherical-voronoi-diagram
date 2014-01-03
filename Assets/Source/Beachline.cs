using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CyclicalSkipList;
using UnityEngine;

public class Beachline : IEnumerable<Arc>
{
    private readonly Skiplist<Arc> _arcs = new Skiplist<Arc> {InOrder = ArcOrderer.AreInOrder};
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

        if (Count == 2)
        {
            node.Left.Key.UpdateLeftEdge();
            arc.UpdateLeftEdge();
        }

        return new List<Arc>();
    }

    private IEnumerable<Arc> InsertSiteOtherThanTheFirstTwo(SiteEvent siteEvent)
    {
        var arcA = new Arc(siteEvent, Sweepline);
        var arcB = new Arc(siteEvent, Sweepline);

        _arcs.Insert(arcA);
        _arcs.Insert(arcB);
        Count = Count + 2;

        var nodeA = _arcs.FetchNode(arcA);
        var nodeB = _arcs.FetchNode(arcB);

        var orderedNodes = OrderNodes(nodeA, nodeB);
        var leftArc = orderedNodes[0].Key;
        var rightArc = orderedNodes[1].Key;

        var arcBeingSplit = orderedNodes[0].Left.Key;

        leftArc.LeftNeighbour = arcBeingSplit.SiteEvent;
        leftArc.RightNeighbour = arcBeingSplit.SiteEvent;
        leftArc.UpdateLeftEdge();

        rightArc.LeftNeighbour = siteEvent;
        rightArc.SiteEvent = arcBeingSplit.SiteEvent;
        rightArc.RightNeighbour = arcBeingSplit.RightNeighbour;
        rightArc.UpdateLeftEdge();

        arcBeingSplit.RightNeighbour = siteEvent;

        return new List<Arc> {arcBeingSplit, rightArc};
    }

    private List<INode<Arc>> OrderNodes(INode<Arc> nodeA, INode<Arc> nodeB)
    {
        if (nodeA.Right == nodeB)
        {
            return new List<INode<Arc>> {nodeA, nodeB};
        }
        if (nodeA.Left == nodeB)
        {
            return new List<INode<Arc>> {nodeB, nodeA};
        }
        else throw new ArgumentException("Nodes are not neighbours!");
    }

    //TODO: Test.
    public IEnumerable<Arc> Remove(Arc arc)
    {
        Sweepline.Z = Sweepline.Z + 0.0001f;
        var node = _arcs.FetchNode(arc);
        var successfulRemoval = _arcs.Remove(arc);
        Sweepline.Z = Sweepline.Z - 0.0001f;

        node.Left.Key.RightNeighbour = arc.RightNeighbour;
        node.Right.Key.LeftNeighbour = arc.LeftNeighbour;
        node.Right.Key.UpdateLeftEdge();

        UnityEngine.Debug.Log(successfulRemoval);

        Count = Count - 1;

        return new List<Arc> {node.Left.Key, node.Right.Key};
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