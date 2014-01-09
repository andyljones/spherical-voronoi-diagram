using System;

namespace Generator
{
    public class CircleEvent
    {
        private readonly Arc _leftArc;
        private readonly Arc _middleArc;
        private readonly Arc _rightArc;

        public double Priority { get; private set; }

        public CircleEvent(Arc leftArc, Arc middleArc, Arc rightArc)
        {
            _leftArc = leftArc;
            _middleArc = middleArc;
            _rightArc = rightArc;

            Priority = CalculatePriority(leftArc.Site.Position, middleArc.Site.Position, rightArc.Site.Position);
        }

        private static double CalculatePriority(Vector3 a, Vector3 b, Vector3 c)
        {
            var v = (a - b).CrossMultiply(c - b);
            var n = new Vector3(0, 0, 1);

            var vz = v[2];
            var va = v.ScalarMultiply(a);

            var z = va*vz - Math.Sqrt((1 - vz*vz)*(1 - va*va));
            var sign = va > -vz ? 1 : -1;

            return sign*(1 + z);
        }
    }
}
