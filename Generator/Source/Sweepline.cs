using System;
using System.Diagnostics;
using MathNet.Numerics;

namespace Generator
{
    public class Sweepline
    {
        public double Colatitude
        {
            get
            {
                return Trig.InverseCosine(Z);
            }
            set
            {
                Z = Trig.Cosine(value);
            }
        }

        //TODO: Move from Z to priority.
        public double Z
        {
            get
            {
                return Math.Abs(Priority) - 1;
            }
            set
            {
                Priority = value + 1;
            }
        }

        public double Priority;
        
        public override string ToString()
        {
            return String.Format("{0,3:N0}", Trig.RadianToDegree(Colatitude));
        }
    }
}
