using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5;
using Comparers;
using Events;

public class EventQueue : IntervalHeap<IEvent>
{
    public EventQueue() : base(new LexicographicEventComparer())
    {
    
    }
}