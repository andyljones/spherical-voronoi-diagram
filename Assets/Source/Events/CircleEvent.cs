using UnityEngine;

namespace Events
{
    public class CircleEvent
    {
        public readonly Vector3 Circumcenter;
        public readonly float MinimumPolarAngle;

        public CircleEvent(Vector3 circumcenter, float radius)
        {
            Circumcenter = circumcenter;
            MinimumPolarAngle = CalculateMinimumPolarAngle(circumcenter, radius);
        }

        private float CalculateMinimumPolarAngle(Vector3 circumcenter, float radius)
        {
            var northPole = new Vector3(0, 0, 1);
            var angleBetweenCenterAndPole = Mathf.Acos(Vector3.Dot(circumcenter, northPole));
            return angleBetweenCenterAndPole + radius;
        }
    }
}
