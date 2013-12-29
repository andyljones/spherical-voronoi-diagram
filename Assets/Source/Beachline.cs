using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CyclicalSkipList;
using UnityEngine;

public class Beachline : IEnumerable<Arc>
{
    private readonly Skiplist<Arc> _intersections;
    public readonly Sweepline Sweepline;

    public int Count { get; private set; }

    public Beachline()
    {
        Sweepline = new Sweepline(1);
        _intersections = new Skiplist<Arc>();
    }

    public void Insert(Site site)
    {
        if (Count <= 1)
        {
            Sweepline.Z = site.Position.z;
            var arc = new Arc(site, Sweepline);
            _intersections.Add(arc);

            var node = _intersections.FetchNode(arc);
            arc.LeftNeighbour = node.Left.Key.Site;
            arc.RightNeighbour = node.Right.Key.Site;

            node.Left.Key.RightNeighbour = arc.Site;
            node.Right.Key.LeftNeighbour = arc.Site;

            Count++;
        }
        else
        {
            Sweepline.Z = site.Position.z;
            var arcA = new Arc(site, Sweepline);

            var arcBeingSplit = _intersections.FetchNode(arcA).Key;

            arcA.LeftNeighbour = arcBeingSplit.Site;
            arcA.RightNeighbour = arcBeingSplit.Site;
            _intersections.Add(arcA);

            var arcB = new Arc(arcBeingSplit.Site, Sweepline);
            arcB.LeftNeighbour = site;
            arcB.RightNeighbour = arcBeingSplit.RightNeighbour;

            arcBeingSplit.RightNeighbour = site;
        
            _intersections.Add(arcB);

            Count++;
        }
    }

    public IEnumerator<Arc> GetEnumerator()
    {
        return _intersections.GetEnumerator();
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