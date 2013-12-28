using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CyclicalSkipList;
using UnityEngine;

public class Beachline : IEnumerable<Intersection>
{
    private readonly Skiplist<Intersection> _intersections;

    public Beachline(IEnumerable<Intersection> intersections)
    {
        _intersections = SkiplistFactory.CreateFrom(intersections);
    }

    public IEnumerator<Intersection> GetEnumerator()
    {
        return _intersections.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}