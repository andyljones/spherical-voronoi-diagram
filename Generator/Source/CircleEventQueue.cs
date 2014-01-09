using System.Collections.Generic;
using C5;

namespace Generator
{
    public class CircleEventQueue
    {
        private readonly IPriorityQueue<CircleEvent> _queue;
        private readonly Dictionary<IArc, IPriorityQueueHandle<CircleEvent>> _queueHandles;

        private readonly double _terminatingPriority;

        public CircleEventQueue(double terminatingPriority)
        {
            _queue = new IntervalHeap<CircleEvent>();
            _queueHandles = new Dictionary<IArc, IPriorityQueueHandle<CircleEvent>>();
            _terminatingPriority = terminatingPriority;
        }

        #region Insert methods
        public void TryInsertAll(List<CircleEvent> circleEvents)
        {
            circleEvents.ForEach(circleEvent => TryInsert(circleEvent));
        }

        public void TryInsert(CircleEvent circleEvent)
        {
            if (HasThreeDistinctSites(circleEvent) &&
                WillOccurBeforeTermination(circleEvent))
            {
                TryRemoveEventCorrespondingTo(circleEvent.MiddleArc);

                IPriorityQueueHandle<CircleEvent> eventHandle = null;
                _queue.Add(ref eventHandle, circleEvent);
                
                _queueHandles.Add(circleEvent.MiddleArc, eventHandle);
            }
        }

        private bool HasThreeDistinctSites(CircleEvent circleEvent)
        {
            return (circleEvent.LeftArc.Site != circleEvent.MiddleArc.Site ||
                    circleEvent.MiddleArc.Site != circleEvent.RightArc.Site ||
                    circleEvent.RightArc.Site != circleEvent.LeftArc.Site);
        }

        private bool WillOccurBeforeTermination(CircleEvent circleEvent)
        {
            return circleEvent.Priority > _terminatingPriority;
        }

        private void TryRemoveEventCorrespondingTo(IArc arc)
        {
            if (_queueHandles.ContainsKey(arc))
            {
                _queue.Delete(_queueHandles[arc]);
                _queueHandles.Remove(arc);
            }
        }
        #endregion

        public double HighestPriority()
        {
            return _queue.FindMax().Priority;
        }

        public bool IsEmpty()
        {
            return _queue.IsEmpty;
        }

        public IArc PopHighestPriorityArc()
        {
            var maxCircleEvent = _queue.FindMax();

            TryRemoveEventCorrespondingTo(maxCircleEvent.LeftArc);
            TryRemoveEventCorrespondingTo(maxCircleEvent.MiddleArc);
            TryRemoveEventCorrespondingTo(maxCircleEvent.RightArc);

            return maxCircleEvent.MiddleArc;
        }

    }
}
