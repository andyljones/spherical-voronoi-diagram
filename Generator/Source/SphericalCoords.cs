using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Generator
{
    public class SphericalCoords : Vector
    {
        public double Colatitude { get { return this[0]; } }
        public double Azimuth { get { return this[1]; } }

        public SphericalCoords(double colatitude, double azimuth) : base(new[] {colatitude, azimuth}) {}

        public override string ToString()
        {
            return String.Format("({0,3:N0},{1,3:N0})", Trig.RadianToDegree(Colatitude), Trig.RadianToDegree(Azimuth));
        }
    }
}
