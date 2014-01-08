namespace Generator
{
    public interface IArc
    {
        SiteEvent Site { get; }
        SiteEvent LeftNeighbour { get; set; }
    }
}
