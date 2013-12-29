using System.Linq;
using UnityEngine;
using CyclicalSkipList;
using System.Collections;

public class load : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
        var beachline = new Beachline();
        beachline.Insert(new Site(new Vector3(1f, 0f, 1f)));
        beachline.Insert(new Site(new Vector3(2f, 0f, 1f)));
        beachline.Insert(new Site(new Vector3(4f, 0f, 1f)));
        beachline.Insert(new Site(new Vector3(8f, 0f, 1f)));

	    beachline.Sweepline.Z = 0.08f;
        Debug.Log(beachline);

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
}
