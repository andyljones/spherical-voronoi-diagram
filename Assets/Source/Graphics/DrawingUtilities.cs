using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public static class DrawingUtilities
    {
        public static IEnumerable<float> AzimuthsInRange(float leftAzimuth, float rightAzimuth, int pointsPerRange)
        {
            if (leftAzimuth <= rightAzimuth)
            {
                var distance = rightAzimuth - leftAzimuth;
                return Enumerable.Range(0, pointsPerRange + 1).Select(i => leftAzimuth + i * distance / pointsPerRange);
            }
            else
            {
                var distanceFromLeftToZero = Mathf.Abs(2 * Mathf.PI - leftAzimuth);
                var distanceFromRightTo2Pi = Mathf.Abs(rightAzimuth);
                var distance = distanceFromLeftToZero + distanceFromRightTo2Pi;

                var numberOfpointsFromLeft = Mathf.FloorToInt(pointsPerRange * distanceFromLeftToZero / distance);
                var numberOfpointsFromRight = Mathf.CeilToInt(pointsPerRange * distanceFromRightTo2Pi / distance);

                var pointsFromLeft =
                    Enumerable.Range(0, numberOfpointsFromLeft + 1)
                        .Select(i => leftAzimuth + i * distanceFromLeftToZero / numberOfpointsFromLeft);

                var pointsFromRight =
                    Enumerable.Range(0, numberOfpointsFromRight + 1)
                        .Select(i => i * distanceFromRightTo2Pi / numberOfpointsFromRight);

                return pointsFromLeft.Concat(pointsFromRight);
            }
        }
    }
}
