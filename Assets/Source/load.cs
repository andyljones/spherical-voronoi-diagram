using System;
using System.Linq;
using UnityEngine;
using CyclicalSkipList;
using System.Collections;

public class load : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        var beachline = new Beachline();
        //beachline.Insert(MathUtils.SiteAt(26, 117));
        //beachline.Insert(MathUtils.SiteAt(97, -122));
        //beachline.Insert(MathUtils.SiteAt(100, -69));

        beachline.Insert(MathUtils.SiteAt(0, 0));
        beachline.Insert(MathUtils.SiteAt(45, 45));

	    var arcs = beachline.ToList();
        Debug.Log(beachline);
        //Debug.Log(arcs[1].CompareTo(arcs[2]));

        beachline.Sweepline.Z = Mathf.Cos(Mathf.PI / 180 * 50f);

	    BeachlineDrawer.DrawBeachline(beachline);

        //var intersection = BeachlineDrawer.PointOnEllipse(beachline.First(), beachline.First().AzimuthOfLeftIntersection());
        //DrawLineThrough(intersection);

        DrawLineThrough(new Vector3(1, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void DrawLineThrough(Vector3 vector)
    {
        var vectorObject = new GameObject("Point " + vector);
        var vectorRenderer = vectorObject.AddComponent<MeshRenderer>();
        var vectorMeshFilter = vectorObject.AddComponent<MeshFilter>();
        vectorMeshFilter.mesh.vertices = new[] { 1.1f *vector, new Vector3(0, 0, 0) };
        vectorMeshFilter.mesh.SetIndices(Enumerable.Range(0, 2).ToArray(), MeshTopology.LineStrip, 0);
    }

    private SiteEvent SiteAt(float colatitude, float azimuth)
    {
        colatitude = colatitude*Mathf.PI/180;
        azimuth = azimuth*Mathf.PI/180;

        var x = Mathf.Sin(colatitude)*Mathf.Cos(azimuth);
        var y = Mathf.Sin(colatitude) * -Mathf.Sin(azimuth);
        var z = Mathf.Cos(colatitude);

        return new SiteEvent(new Vector3(x, y, z));
    }
}
