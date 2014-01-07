using System;
using MathNet.Numerics;

namespace Generator
{
    public class Sweepline
    {
        public double Colatitude;

        public override string ToString()
        {
            return String.Format("{0,3:N0}", Trig.RadianToDegree(Colatitude));
        }
    }
}
