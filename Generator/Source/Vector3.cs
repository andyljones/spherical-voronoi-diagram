using MathNet.Numerics.LinearAlgebra;

namespace Generator
{
    public class Vector3 : Vector
    {
        public double X { get { return this[0]; } }
        public double Y { get { return this[1]; } }
        public double Z { get { return this[2]; } }
        
        public Vector3(double x, double y, double z) : base(new[] {x, y, z}) {}

        public override string ToString()
        {
            return this.SphericalCoordinates().ToString();
        }
    }
}
