using System;

namespace Generator
{
    public class Arc
    {
        public SiteEvent Site;
        public SiteEvent LeftNeighbour;

        public override string ToString()
        {
            return String.Format("[{0}|{1}]", LeftNeighbour, Site);
        }
    }
}
