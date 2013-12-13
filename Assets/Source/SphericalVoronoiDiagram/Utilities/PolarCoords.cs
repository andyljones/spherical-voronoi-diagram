using UnityEngine;

namespace SphericalVoronoiDiagram.Utilities
{
    public static class PolarCoords
    {
        public static float PolarAngle(Vector3 u)
        {
            return Mathf.Acos(u.normalized.z);
        }

        public static float Azimuth(Vector3 u)
        {
            return Mathf.Atan2(-u.y, u.x);
        }

        public static Vector3 Vector(float polarAngle, float azimuth)
        {
            return new Vector3(Mathf.Sin(polarAngle)*Mathf.Cos(azimuth),
                              -Mathf.Sin(polarAngle)*Mathf.Sin(azimuth),
                               Mathf.Cos(polarAngle));
        }
    }
}
