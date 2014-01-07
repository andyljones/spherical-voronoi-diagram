namespace Generator
{
    public interface IOrderableArc
    {
        Vector3 LeftIntersection(Sweepline sweepline);
    }

    public class ArcOrderer
    {
        private Sweepline _sweepline;

        public ArcOrderer(Sweepline sweepline)
        {
            _sweepline = sweepline;
        }

        public bool AreInOrder(IOrderableArc a, IOrderableArc b, IOrderableArc c)
        {
            var aLeft = a.LeftIntersection(_sweepline);
            var bLeft = b.LeftIntersection(_sweepline);
            var cLeft = c.LeftIntersection(_sweepline);
            
            var areInOrder = (cLeft - bLeft).CrossMultiply(aLeft - bLeft)[2] >= 0;

            return areInOrder;
        }
    }
}
