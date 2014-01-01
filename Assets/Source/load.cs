using System;
using System.Collections.Generic;
using System.Linq;
using Graphics;
using UnityEngine;
using CyclicalSkipList;
using System.Collections;
using Random = System.Random;

public class load : MonoBehaviour
{
    private Random _random = new Random();

    private VoronoiDiagramDrawer _drawer;
    private VoronoiDiagram _diagram;

	// Use this for initialization
	void Start ()
	{
        var positions = Enumerable.Range(0, 100).Select(i => CreateSphericalVector());
        //var positions = new List<Vector3> { MathUtils.CreateVectorAt(9, 248), MathUtils.CreateVectorAt(15, 222), MathUtils.CreateVectorAt(19, 260) };

        _diagram = new VoronoiDiagram(positions);

	    _drawer = new VoronoiDiagramDrawer(_diagram);

        Debug.Log(_diagram.Beachline);
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyDown(KeyCode.N))
	    {
            _diagram.ProcessNextEvent();
            _drawer.UpdateVoronoiDiagram();
            Debug.Log(_diagram.Beachline);
	    }
	}

    private Vector3 CreateSphericalVector()
    {
        var z = (float)(-1 + 2*_random.NextDouble());
        var azimuth = (float)(2 * Mathf.PI * _random.NextDouble());

        var x = Mathf.Sqrt(1 - z*z) * Mathf.Cos(azimuth);
        var y = -Mathf.Sqrt(1 - z*z) * Mathf.Sin(azimuth);

        return new Vector3(x, y, z);
    }
}
