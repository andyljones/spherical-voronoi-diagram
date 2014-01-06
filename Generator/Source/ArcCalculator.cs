using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

namespace Generator
{
    public static class ArcCalculator
    {
        public static Vector PointOnArcAboveVector(this Arc arc, Vector vector, Sweepline sweepline)
        {
            var p = arc.Site.Position;

            var xi = sweepline.Colatitude;

            var theta_p = Trig.InverseCosine(p[2]);
            var phi_p = Trig.InverseTangentFromRational(p[1], p[0]);

            var phi = Trig.InverseTangentFromRational(vector[1], vector[0]);

            var tanTheta = (Trig.Cosine(xi) - Trig.Cosine(theta_p))/
                           (Trig.Sine(theta_p)*Trig.Cosine(phi - phi_p) - Trig.Sine(xi));

            var theta = Trig.InverseTangent(tanTheta);

            var x = Trig.Sine(theta)*Trig.Cosine(phi);
            var y = Trig.Sine(theta)*Trig.Sine(phi);
            var z = Trig.Cosine(theta);

            return new Vector(new[] {x, y, z});
        }
    }
}
