using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Endpoint
{
    public Site LeftSite;
    public Site RightSite;

    public Endpoint(Site leftSite, Site rightSite)
    {
        LeftSite = leftSite;
        RightSite = rightSite;
    }
}