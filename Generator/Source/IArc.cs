namespace Generator
{
    public interface IArc
    {
        SiteEvent Site { get; set; }
        SiteEvent LeftNeighbour { get; set; }
    }
}
