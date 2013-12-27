using System.Collections.Generic;

public class IntersectionComparer : IComparer<Intersection>
{
    public int Compare(Intersection x, Intersection y)
    {
        var leftLongitude = x.Longitude();
        var rightLongitude = y.Longitude();

        return leftLongitude.CompareTo(rightLongitude);
    }
}