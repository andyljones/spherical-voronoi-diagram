namespace Generator
{
    public class DeadVertex : IVertex
    {
        public Vector3 Position { get; private set; }

        public DeadVertex(CircleEvent circleEvent)
        {
            Position = circleEvent.Center();
        }
    }
}
