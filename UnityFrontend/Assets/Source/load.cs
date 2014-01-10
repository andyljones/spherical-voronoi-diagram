using System;
using System.Collections.Generic;
using System.Linq;
using Generator;
using Graphics;
using MathNet.Numerics;
using UnityEngine;
using Random = System.Random;

public class load : MonoBehaviour
{
    private Random _random = new Random();

    private VoronoiDiagramDrawer _drawer;
    private VoronoiDiagram _diagram;

	// Use this for initialization
	void Start ()
	{
	    var positions = new List<double[]>
	    {
	        VectorAt(0, 0),
	        VectorAt(45, -45),
	        VectorAt(45, 45),
            VectorAt(90, 0)
	    };
        //positions = Enumerable.Range(0, 100).Select(i => CreateSphericalVector()).ToList();

        _diagram = new VoronoiDiagram(positions);

	    _drawer = new VoronoiDiagramDrawer(_diagram);
        
        Debug.Log(_diagram.Beachline);
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKey(KeyCode.N) || Input.GetKeyDown(KeyCode.F))
	    {
            _diagram.ProcessNextEvent();
            Debug.Log(_diagram.Beachline);
            Debug.Log(_diagram.CircleEventQueue);
            _drawer.UpdateVoronoiDiagram();
	    }
	}

    private double[] CreateSphericalVector()
    {
        var z = (float)(-1 + 2*_random.NextDouble());
        var azimuth = (float)(2 * Mathf.PI * _random.NextDouble());

        var x = Mathf.Sqrt(1 - z*z) * Mathf.Cos(azimuth);
        var y = Mathf.Sqrt(1 - z*z) * Mathf.Sin(azimuth);

        return new double[] {x, y, z};
    }

    private double[] VectorAt(double colatitude, float azimuth)
    {
        colatitude = Mathf.Deg2Rad*colatitude;
        azimuth = Mathf.Deg2Rad*azimuth;

        var x = Trig.Sine(colatitude)*Trig.Cosine(azimuth);
        var y = Trig.Sine(colatitude)*Trig.Sine(azimuth);
        var z = Trig.Cosine(colatitude);

        return new double[] {x, y, z};
    }
}
