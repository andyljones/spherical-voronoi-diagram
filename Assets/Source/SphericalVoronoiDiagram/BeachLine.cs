using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Source.SphericalVoronoiDiagram.Events;
using Diag = System.Diagnostics;
using C5;
using Events;

public class BeachLine : TreeSet<IArc>
{
    public BeachLine() : base(new AzimuthalArcComparer())
    {
    }

    public List<IArc> ArcsContaining(IArc arc)
    {
        var results = new List<IArc>();

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

    public bool TryRemove(IArc arc)
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

    public IArc CircularPredecessor(IArc arc)
    {
        IArc predecessor;

        if (!TryPredecessor(arc, out predecessor) && !base.IsEmpty)
        {
            predecessor = FindMax();
        }

        return predecessor;
    }

    public IArc CircularSuccessor(IArc arc)
    {
        IArc successor;

        if (!TrySuccessor(arc, out successor) && !base.IsEmpty)
        {
            successor = FindMin();
        }

        return successor;
    }
}