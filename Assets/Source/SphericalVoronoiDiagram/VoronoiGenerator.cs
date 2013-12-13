using System.Collections.Generic;
using Events;
using Simulator.ShallowFluid;

namespace SphericalVoronoiDiagram
{
    public class VoronoiGenerator<T>
    {
        private EventQueue _queue;
        private readonly Dictionary<T, SiteEvent> _siteDictionary; 

        public VoronoiGenerator(VectorField<T> positions)
        {
            _queue = new EventQueue();
            _siteDictionary = new Dictionary<T, SiteEvent>();

            foreach (var nodeAndPosition in positions)
            {
                var site = new SiteEvent(nodeAndPosition.Value);
                _queue.Add(site);
                _siteDictionary.Add(nodeAndPosition.Key, site);
            }

            var beachline = new BeachLine();
            var circleHandler = new CircleEventHandler(beachline, _queue);
            var siteHandler = new SiteEventHandler(beachline, _queue);

            while (!_queue.IsEmpty)
            {
                var nextEvent = _queue.DeleteMin();

                if (nextEvent is SiteEvent)
                {
                    siteHandler.Handle(nextEvent as SiteEvent);
                }
                else if (nextEvent is CircleEvent)
                {
                    circleHandler.Handle(nextEvent as CircleEvent);
                }
            }
        }
    }
}
