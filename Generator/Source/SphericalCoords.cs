using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Generator
{
    public class SphericalCoords : Vector
    {
        public double Theta { get { return this[0]; } }
        public double Phi { get { return this[1]; } }

        public SphericalCoords(double colatitude, double azimuth) : base(new[] {colatitude, azimuth}) {}

        public override string ToString()
        {
            return String.Format("({0,3:N0},{1,3:N0})", Trig.RadianToDegree(Theta), Trig.RadianToDegree(Phi));
        }
    }
}
