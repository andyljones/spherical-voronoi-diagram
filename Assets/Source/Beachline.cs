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
    private readonly Sweepline _sweepline;

    public Beachline(Sweepline sweepline)
    {
        _sweepline = sweepline;
        _intersections = new Skiplist<Arc>();
    }

    public Beachline(IEnumerable<Arc> intersections, Sweepline sweepline)
        : this(sweepline)
    {
        _intersections = SkiplistFactory.CreateFrom(intersections);
    }

    public void Insert(Site site)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<Arc> GetEnumerator()
    {
        return _intersections.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}