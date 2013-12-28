using System.Linq;
using UnityEngine;
using CyclicalSkipList;
using System.Collections;

public class load : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
        var a = new Vector3(6f, 0f, 1f).normalized;
	    var b = new Vector3(0f, -1f, 1f).normalized;
	    var h = 0f;

	    MakeEllipse(a, h);
        MakeEllipse(b, h);

	    var siteA = new Site(a);
	    var siteB = new Site(b);
	    var arcA = new Arc(siteA, new Sweepline(h)) {LeftNeighbour = siteB, RightNeighbour = siteB};

	    var p = arcA.AzimuthOfLeftIntersection();
        //var x = Mathf.Sqrt(1 - h*h)*Mathf.Cos(p);
        //var y = Mathf.Sqrt(1 - h*h)*Mathf.Sin(p);

        var v = EllipseDrawer.PointOnEllipse(a, h, p);
        //var v = 1.1f * new Vector3(Mathf.Cos(p), -Mathf.Sin(p), 0);

        Debug.Log(Vector3.Dot(a, v) + "; " + Vector3.Dot(b, v));
        Debug.Log(p*180/Mathf.PI);

	    var pObj = new GameObject("p: "+v);
	    var pRenderer = pObj.AddComponent<MeshRenderer>();
	    var pFilter = pObj.AddComponent<MeshFilter>();
        pFilter.mesh.vertices = new[] { 1.5f * v, new Vector3(0, 0, 0) };
        pFilter.mesh.SetIndices(Enumerable.Range(0, 2).ToArray(), MeshTopology.LineStrip, 0);

        //var poleObj = new GameObject("Pole");
        //var poleRenderer = poleObj.AddComponent<MeshRenderer>();
        //var poleFilter = poleObj.AddComponent<MeshFilter>();
        //poleFilter.mesh.vertices = new[] {new Vector3(0, 0, -1.5f), new Vector3(0, 0, 1.5f)};
        //poleFilter.mesh.SetIndices(Enumerable.Range(0, 2).ToArray(), MeshTopology.LineStrip, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private static void MakeEllipse(Vector3 focus, float height)
    {
        const int count = 1000;

        var ellipsePoints = EllipseDrawer.PointsOnEllipse(focus, height, count);
        var ellipseObj = new GameObject("Ellipse" + focus);
        var ellipseRenderer = ellipseObj.AddComponent<MeshRenderer>();
        var ellipseFilter = ellipseObj.AddComponent<MeshFilter>();
        ellipseFilter.mesh.vertices = ellipsePoints.ToArray();
        ellipseFilter.mesh.SetIndices(Enumerable.Range(0, count+1).ToArray(), MeshTopology.LineStrip, 0);
    }
}
