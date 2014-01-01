using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5;

public class CircleEventQueue : IEnumerable<CircleEvent>
{
    private IPriorityQueue<CircleEvent> _circleEvents;
    private Dictionary<Arc, IPriorityQueueHandle<CircleEvent>> _arcToCircleDictionary; 

    public CircleEventQueue()
    {
        _circleEvents = new IntervalHeap<CircleEvent>();
        _arcToCircleDictionary = new Dictionary<Arc, IPriorityQueueHandle<CircleEvent>>();
    }

    public CircleEvent FindMax()
    {
        return _circleEvents.FindMax();
    }

    public CircleEvent DeleteMax()
    {
        var circleEvent = _circleEvents.DeleteMax();
        _arcToCircleDictionary.Remove(circleEvent.Arc);

        return circleEvent;
    }

    public bool IsEmpty()
    {
        return _circleEvents.IsEmpty;
    }

    public void UpdateArcs(IEnumerable<Arc> arcs)
    {
        foreach (var arc in arcs)
        {
            if (_arcToCircleDictionary.ContainsKey(arc))
            {
                var handle = _arcToCircleDictionary[arc];
                _circleEvents.Delete(handle);
                _arcToCircleDictionary.Remove(arc);
            }

            var newCircleEvent = new CircleEvent(arc);
            IPriorityQueueHandle<CircleEvent> newHandle = null;
            _circleEvents.Add(ref newHandle, newCircleEvent);            
            _arcToCircleDictionary.Add(arc, newHandle);
        }
    }

    public IEnumerator<CircleEvent> GetEnumerator()
    {
        return _circleEvents.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}