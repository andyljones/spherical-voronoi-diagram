using System;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public static class LatLongGridDrawer
    {
        public static int NumberOfPointsPerLatitude = 100;
        public static int NumberOfPointsPerLongitude = 50;

        public static float ScaleFactor = 0.99f;

        public static void DrawGrid()
        {
            var gridObject = new GameObject("LatLong Grid");

            foreach (var colatitude in Enumerable.Range(1, 17).Select(i => i*Mathf.PI/18))
            {
                var latitudeObject = DrawLatitude(colatitude);
                latitudeObject.transform.parent = gridObject.transform;
            }

            foreach (var azimuth in Enumerable.Range(0, 36).Select(i => i * 2*Mathf.PI / 36))
            {
                var longitudeObject = DrawLongitude(azimuth);
                longitudeObject.transform.parent = gridObject.transform;
            }
        }

        private static GameObject DrawLatitude(float colatitude)
        {
            var azimuths = DrawingUtilities.AzimuthsInRange(0, 2*Mathf.PI, NumberOfPointsPerLatitude);

            var vertices = 
                azimuths.Select(
                azimuth => MathUtils.CreateVectorAt(colatitude, azimuth))
                .ToArray();

            var latitudeObject = 
                DrawingUtilities.CreateLineObject(
                "Colatitude " + Mathf.Rad2Deg*colatitude, 
                vertices, 
                "Boundaries");

            return latitudeObject;
        }


        private static GameObject DrawLongitude(float azimuth)
        {
            var azimuths = DrawingUtilities.AzimuthsInRange(0, Mathf.PI, NumberOfPointsPerLongitude);

            var vertices = 
                azimuths.Select(
                colatitude => MathUtils.CreateVectorAt(colatitude, azimuth))
                .ToArray();

            var longitudeObject = 
                DrawingUtilities.CreateLineObject(
                "Longitude " + Mathf.Rad2Deg*azimuth,
                vertices, 
                "Boundaries");

            return longitudeObject;
        }

        //private static GameObject DrawLabel(float colatitude, float azimuth)
        //{
            
        //}
    }
}
