using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Diag = System.Diagnostics;
using C5;
using Events;

public class BeachLine : TreeSet<Arc>
{
    public BeachLine() : base(new AzimuthalArcComparer())
    {
    }

    public List<Arc> ArcsContaining(Arc arc)
    {
        var results = new List<Arc>();

        var predecessor = CircularPredecessor(arc);
        if (predecessor != null && 
            predecessor.Contains(arc))
        {
            results.Add(predecessor);
        }

        var successor = CircularSuccessor(arc);
        if (successor != null && 
            successor != predecessor &&
            successor.Contains(arc))
        {
            results.Add(successor);
        }

        return results;
    }

    public bool TryRemove(Arc arc)
    {
        if (this.Contains(arc))
        {
            var predecessor = CircularPredecessor(arc);
            var successor = CircularSuccessor(arc);

            predecessor.ConnectToLeftOf(successor);
            Remove(arc);
            return true;
        }
        else
        {
            return false;
        }

    }

    public Arc CircularPredecessor(Arc arc)
    {
        Arc predecessor;

        if (!TryPredecessor(arc, out predecessor) && !base.IsEmpty)
        {
            predecessor = FindMax();
        }

        return predecessor;
    }

    public Arc CircularSuccessor(Arc arc)
    {
        Arc successor;

        if (!TrySuccessor(arc, out successor) && !base.IsEmpty)
        {
            successor = FindMin();
        }

        return successor;
    }
}