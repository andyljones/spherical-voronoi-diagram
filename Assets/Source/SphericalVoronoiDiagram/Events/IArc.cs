using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5;
using Events;

namespace Assets.Source.SphericalVoronoiDiagram.Events
{
    public interface IArc
    {
        bool Contains(IArc arc);
        List<IArc> SplitAt(IArc splittingArc);
        void ConnectToLeftOf(IArc otherArc);
        void ConnectToRightOf(IArc otherArc);

        IArc LeftArc { get; set; }
        IArc RightArc { get; set; }

        SiteEvent Site { get; set; }
        IPriorityQueueHandle<IEvent> CircleEventHandle { get; set; }
    }
}
