namespace Generator
{
    public class LiveVertex : IVertex
    {
        private readonly IArc _arc;
        private readonly Sweepline _sweepline;

        public Vector3 Position
        {
            get
            {
                return _position ?? _arc.PointOfIntersection(_sweepline);
            }
            set
            {
                _position = value;
            }
        }
        private Vector3 _position;

        public LiveVertex(IArc arc, Sweepline sweepline)
        {
            _arc = arc;
            _sweepline = sweepline;
        }
    }
}
