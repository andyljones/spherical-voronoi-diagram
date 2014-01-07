namespace Generator
{
    public class SiteEvent
    {
        public Vector3 Position;

        public SiteEvent() {}

        public SiteEvent(double x, double y, double z)
        {
            Position = new Vector3(x, y, z);
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}
