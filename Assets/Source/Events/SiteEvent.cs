using UnityEngine;

namespace Events
{
    public class SiteEvent
    {
        public readonly Vector3 Position;

        public SiteEvent()
        {   
        }

        public SiteEvent(Vector3 position)
        {
            Position = position;
        }
    }
}