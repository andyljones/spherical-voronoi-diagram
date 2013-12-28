using System.Collections.Generic;

public class IntersectionComparer : IComparer<Intersection>
{
    public int Compare(Intersection x, Intersection y)
    {
        var leftLongitude = x.Azimuth;
        var rightLongitude = y.Azimuth;

        return leftLongitude.CompareTo(rightLongitude);
    }
}