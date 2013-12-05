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

    public void CreateArcAt(SiteEvent site)
    {
        var newArc = new Arc(site);

        if (Count == 0)
        {
            Add(newArc);
        }
        else if (Count == 1)
        {
            var onlyCurrentArc = this.Single();
            onlyCurrentArc.ConnectToLeftOf(newArc);
            newArc.ConnectToLeftOf(onlyCurrentArc);
            Add(newArc);
        }
        else
        {
            var containingArcs = ArcsContaining(newArc);

            if (containingArcs.Count == 1)
            {
                var containingArc = containingArcs.Single();
                Remove(containingArc);
                var newNeighbouringArcs = containingArc.SplitAt(newArc);
                AddAll(newNeighbouringArcs);
                Add(newArc);
            }
            else if (containingArcs.Count == 2)
            {
                containingArcs[0].ConnectToLeftOf(newArc);
                newArc.ConnectToLeftOf(containingArcs[1]);
                Add(newArc);
            }
            else
            {
                throw new InvalidDataException("More than two arcs intersected by the site!");
            }
        }
    }

    private List<Arc> ArcsContaining(Arc arc)
    {
        var results = new List<Arc>();

        var predecessor = CircularPredecessor(arc);
        if (predecessor.Contains(arc))
        {
            results.Add(predecessor);
        }

        var successor = CircularSuccessor(arc);
        if (successor.Contains(arc) && successor != predecessor)
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

    private Arc CircularPredecessor(Arc arc)
    {
        Arc predecessor;

        if (!TryPredecessor(arc, out predecessor) && !base.IsEmpty)
        {
            predecessor = FindMax();
        }

        return predecessor;
    }

    private Arc CircularSuccessor(Arc arc)
    {
        Arc successor;

        if (!TrySuccessor(arc, out successor) && !base.IsEmpty)
        {
            successor = FindMin();
        }

        return successor;
    }
}