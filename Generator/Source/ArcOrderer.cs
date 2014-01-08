using MathNet.Numerics.LinearAlgebra;

namespace Generator
{
    public class ArcOrderer
    {
        private Sweepline _sweepline;

        public ArcOrderer(Sweepline sweepline)
        {
            _sweepline = sweepline;
        }

        public bool AreInOrder(IArc a, IArc b, IArc c)
        {
            var aLeft = a.LeftIntersection(_sweepline);
            var bLeft = b.LeftIntersection(_sweepline);
            var cLeft = c.LeftIntersection(_sweepline);
            
            var areInOrder = (cLeft - bLeft).CrossMultiply(aLeft - bLeft)[2] >= 0;

            return areInOrder;
        }

        public static bool AreInOrder(Vector3 a, Vector3 b, Vector3 c)
        {
            if (Vector.AlmostEqual(b, c) || Vector.AlmostEqual(a, c))
            {
                return false;
            }
            else
            {
                return (c - b).CrossMultiply(a - b)[2] >= 0;                       
            }
        }
    }
}
