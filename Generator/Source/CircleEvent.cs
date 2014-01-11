using System;
using MathNet.Numerics;

namespace Generator
{
    public class CircleEvent : IComparable<CircleEvent>
    {
        public readonly IArc LeftArc;
        public readonly IArc MiddleArc;
        public readonly IArc RightArc;

        public double Priority { get; private set; }

        public CircleEvent(IArc leftArc, IArc middleArc, IArc rightArc)
        {
            LeftArc = leftArc;
            MiddleArc = middleArc;
            RightArc = rightArc;

            Priority = CalculatePriority();
        }

        public Vector3 Center()
        {
            var a = LeftArc.Site.Position;
            var b = MiddleArc.Site.Position;
            var c = RightArc.Site.Position;

            return (a - b).CrossMultiply(c - b).Normalize().ToVector3();
        }

        private double CalculatePriority()
        {
            var a = LeftArc.Site.Position;
            var v = Center();

            var vz = v[2];
            var va = v.ScalarMultiply(a);

            var z = va*vz - Math.Sqrt((1 - vz*vz)*(1 - va*va));
            var sign = va > -vz ? 1 : -1;

            return sign*(1 + z);
        }

        public int CompareTo(CircleEvent other)
        {
            return this.Priority.CompareTo(other.Priority);
        }

        public override string ToString()
        {
            return String.Format("({0}, {1,10:N9})", MiddleArc, Priority);
        }
    }
}
