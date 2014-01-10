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

            foreach (var colatitude in Enumerable.Range(1, 17).Select(i => i*Mathf.PI/18))
            {
                var labelObject = DrawLabelsAtColatitude(colatitude);
                labelObject.transform.parent = gridObject.transform;
            }
        }

        private static GameObject DrawLatitude(float colatitude)
        {
            var azimuths = DrawingUtilities.AzimuthsInRange(0, 2*Mathf.PI, NumberOfPointsPerLatitude);

            var vertices = 
                azimuths.Select(
                azimuth => DrawingUtilities.CreateVectorAt(colatitude, azimuth))
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
                colatitude => DrawingUtilities.CreateVectorAt(colatitude, azimuth))
                .ToArray();

            var longitudeObject = 
                DrawingUtilities.CreateLineObject(
                "Longitude " + Mathf.Rad2Deg*azimuth,
                vertices, 
                "Boundaries");

            return longitudeObject;
        }

        private static GameObject DrawLabelsAtColatitude(float colatitude)
        {
            var azimuths = DrawingUtilities.AzimuthsInRange(0, 2 * Mathf.PI, 36);

            var labels = azimuths.Take(36).Select(azimuth => DrawLabel(colatitude, azimuth));

            var parentObject = new GameObject("Latitude Labels " + Mathf.Rad2Deg * colatitude);
            foreach (var label in labels)
            {
                label.transform.parent = parentObject.transform;
            }

            return parentObject;
        }

        private static GameObject DrawLabel(float colatitude, float azimuth)
        {
            var text = String.Format("{0,3:N0}  {1,3:N0}", Mathf.Rad2Deg*colatitude, Mathf.Rad2Deg*azimuth);

            var labelObject = new GameObject("Label " + text);

            var normal = DrawingUtilities.CreateVectorAt(colatitude, azimuth);
            var localEast = Vector3.Cross(normal, new Vector3(0, 0, 1));
            var localNorth = Vector3.Cross(localEast, normal);
            labelObject.transform.position = normal;
            labelObject.transform.rotation = Quaternion.LookRotation(-normal, localNorth);

            var textMesh = labelObject.AddComponent<TextMesh>();
            textMesh.text = text;
            textMesh.font = Resources.Load("ARIAL", typeof (Font)) as Font;
            textMesh.renderer.material = Resources.Load("OneSidedMaterial", typeof(Material)) as Material;
            textMesh.characterSize = 0.005f;
            textMesh.anchor = TextAnchor.UpperCenter;

            return labelObject;
        }
    }
}
