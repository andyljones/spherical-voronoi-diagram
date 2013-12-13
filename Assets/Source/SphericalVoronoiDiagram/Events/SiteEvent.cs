using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class SiteEvent : IEvent
    {
        public Vector3 Position { get; private set; }

        public SiteEvent()
        {   
        }

        public SiteEvent(Vector3 position)
        {
            Position = position;
        }
    }
}