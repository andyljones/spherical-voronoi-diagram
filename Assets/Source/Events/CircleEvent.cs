using UnityEngine;

namespace Events
{
    public class CircleEvent : IEvent
    {
        public readonly Vector3 Circumcenter;
        public readonly Arc CentralArc;

        public Vector3 Position { get; private set; }

        public CircleEvent(Arc arc, Vector3 circumcenter, float radius)
        {
            CentralArc = arc;
            Circumcenter = circumcenter;
            Position = CalculatePosition(circumcenter, radius);
        }

        private Vector3 CalculatePosition(Vector3 circumcenter, float radius)
        {
            var northPole = new Vector3(0, 0, 1);
            var polarAngleOfCenter = Mathf.Acos(Vector3.Dot(circumcenter, northPole));

            var polarAngleOfLowestPoint = polarAngleOfCenter + radius;
            var azimuthOfLowestPoint = Mathf.Atan2(-circumcenter.y, circumcenter.x);

            var lowestPoint = new Vector3(Mathf.Sin(polarAngleOfLowestPoint)*Mathf.Cos(azimuthOfLowestPoint), 
                                         -Mathf.Sin(polarAngleOfLowestPoint)*Mathf.Sin(azimuthOfLowestPoint),
                                          Mathf.Cos(polarAngleOfLowestPoint));

            return lowestPoint;
        }
    }
}
