using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;

public class BeachLine
{
    private SortedDictionary<IEvent, IEvent> _dictionary;

    public BeachLine()
    {
        var comparer = new EventComparer();
        _dictionary = new SortedDictionary<IEvent, IEvent>();
    }
}