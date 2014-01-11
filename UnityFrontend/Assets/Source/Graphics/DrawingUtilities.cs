using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IridiumVector3 = Generator.Vector3;

namespace Graphics
{
    public static class DrawingUtilities
    {
        public static IEnumerable<float> AnglesInRange(float leftAzimuth, float rightAzimuth, int pointsPerRange)
        {
            float distance;
            if (rightAzimuth >= leftAzimuth)
            {
                distance = rightAzimuth - leftAzimuth;
            }
            else
            {
                distance = 2*Mathf.PI - (leftAzimuth - rightAzimuth);
            }
            var azimuths = Enumerable.Range(0, pointsPerRange+1).Select(i => leftAzimuth + i*distance/pointsPerRange);

            return azimuths;
        }

        public static GameObject CreateLineObject(String name, Vector3[] points, String materialName)
        {
            var gameObject = new GameObject(name);
            var meshFilter = gameObject.AddComponent<MeshFilter>();
            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material = Resources.Load(materialName, typeof(Material)) as Material;

            var mesh = meshFilter.mesh;
            mesh.vertices = points;
            mesh.SetIndices(Enumerable.Range(0, points.Count()).ToArray(), MeshTopology.LineStrip, 0);
            mesh.RecalculateNormals();
            mesh.uv = Enumerable.Repeat(new Vector2(0, 0), points.Count()).ToArray();

            return gameObject;
        }

        public static void UpdateLineObject(Mesh mesh, Vector3[] vertices)
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.uv = Enumerable.Repeat(new Vector2(0, 0), vertices.Count()).ToArray();
            mesh.SetIndices(Enumerable.Range(0, vertices.Count()).ToArray(), MeshTopology.LineStrip, 0);
            mesh.RecalculateNormals();
        }

        public static Vector3 ToUnityVector3(this IridiumVector3 iridiumVector)
        {
            return new Vector3((float) iridiumVector.X, -(float) iridiumVector.Y, (float) iridiumVector.Z);
        }


        public static float AzimuthOf(Vector3 v)
        {
            return Mathf.Atan2(-v.y, v.x);
        }

        public static float ColatitudeOf(Vector3 v)
        {
            return Mathf.Acos(v.z);
        }

        public static Vector3 CreateVectorAt(float colatitude, float azimuth)
        {
            var x = Mathf.Sin(colatitude)*Mathf.Cos(azimuth);
            var y = -Mathf.Sin(colatitude)*Mathf.Sin(azimuth);
            var z = Mathf.Cos(colatitude);

            return new Vector3(x, y, z);
        }

        public static IEnumerable<Vector3> VerticesOnGreatArc(Vector3 a, Vector3 b, int numberOfVertices)
        {
            if (a == b)
            {
                return new List<Vector3>();
            }

            var normal = Vector3.Cross(a, b);
            var perpendicularToA = Vector3.Cross(normal, a).normalized;

            var maxAngle = Mathf.Acos(Vector3.Dot(a, b));
            var angles = AnglesInRange(0, maxAngle, numberOfVertices);

            var vertices = angles.Select(angle => Mathf.Cos(angle) * a + Mathf.Sin(angle) * perpendicularToA);

            return vertices;
        }
    }
}
