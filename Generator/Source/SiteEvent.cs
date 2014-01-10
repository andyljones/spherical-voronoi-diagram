using System;
using MathNet.Numerics.LinearAlgebra;

namespace Generator
{
    public class SiteEvent : IComparable<SiteEvent>
    {
        public double Priority { get { return 1 + Position.Z; } }
        public Vector3 Position;

        public SiteEvent(Vector position)
        {
            Position = position.ToVector3();
        }

        public int CompareTo(SiteEvent other)
        {
            return Priority.CompareTo(other.Priority);
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}
