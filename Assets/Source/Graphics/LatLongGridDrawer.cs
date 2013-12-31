using System;
using System.Linq;
using UnityEngine;

namespace Graphics
{
    public static class LatLongGridDrawer
    {
        public static int NumberOfPointsPerLatitude = 100;
        public static int NumberOfPointsPerLongitude = 50;
        
            public static Material BoundaryMaterial = Resources.Load("Boundaries", typeof(Material)) as Material;

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
            var latitudeObject = new GameObject("Latitude " + colatitude * 180/Mathf.PI);
            var latitudeMeshFilter = latitudeObject.AddComponent<MeshFilter>();
            var latitudeRenderer = latitudeObject.AddComponent<MeshRenderer>();
            latitudeRenderer.material = Resources.Load("Boundaries", typeof(Material)) as Material;

            var azimuths = DrawingUtilities.AzimuthsInRange(0, 2*Mathf.PI, NumberOfPointsPerLatitude);

            var z = Mathf.Cos(colatitude);

            var scaleFactor = Mathf.Sqrt(1 - z*z);
            Func<float, Vector3> pointAtAzimuth =
                azimuth => ScaleFactor * new Vector3(scaleFactor * Mathf.Cos(azimuth), -scaleFactor * Mathf.Sin(azimuth), z);

            var points = azimuths.Select(azimuth => pointAtAzimuth(azimuth)).ToArray();

            latitudeMeshFilter.mesh.vertices = points;
            latitudeMeshFilter.mesh.SetIndices(
                Enumerable.Range(0, points.Count()).ToArray(),
                MeshTopology.LineStrip,
                0);

            latitudeMeshFilter.mesh.RecalculateNormals();
            latitudeMeshFilter.mesh.uv = Enumerable.Repeat(new Vector2(0, 0), points.Count()).ToArray();

            return latitudeObject;
        }


        private static GameObject DrawLongitude(float azimuth)
        {
            var longitudeObject = new GameObject("Longitude " + azimuth * 180 / Mathf.PI);
            var longitudeMeshFilter = longitudeObject.AddComponent<MeshFilter>();
            var longitudeRenderer = longitudeObject.AddComponent<MeshRenderer>();
            longitudeRenderer.material = Resources.Load("Boundaries", typeof(Material)) as Material;

            var azimuths = DrawingUtilities.AzimuthsInRange(0, Mathf.PI, NumberOfPointsPerLongitude);

            var x = Mathf.Cos(azimuth);
            var y = -Mathf.Sin(azimuth);

            Func<float, Vector3> pointAtAzimuth =
                colatitude => ScaleFactor*new Vector3(x*Mathf.Sin(colatitude), y*Mathf.Sin(colatitude), Mathf.Cos(colatitude));

            var points = azimuths.Select(colatitude => pointAtAzimuth(colatitude)).ToArray();

            longitudeMeshFilter.mesh.vertices = points;
            longitudeMeshFilter.mesh.SetIndices(
                Enumerable.Range(0, points.Count()).ToArray(),
                MeshTopology.LineStrip,
                0);

            longitudeMeshFilter.mesh.RecalculateNormals();
            longitudeMeshFilter.mesh.uv = Enumerable.Repeat(new Vector2(0, 0), points.Count()).ToArray();

            return longitudeObject;
        }
    }
}
