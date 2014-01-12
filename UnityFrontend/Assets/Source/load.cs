using System;
using System.Collections.Generic;
using System.Linq;
using Generator;
using Graphics;
using Grids.GeodesicGridGenerator;
using Initialization;
using MathNet.Numerics;
using UnityEngine;
using Random = System.Random;

public class load : MonoBehaviour
{
    private Random _random = new Random();

    private VoronoiDiagramDrawer _drawer;
    private VoronoiDiagram _diagram;

    private bool _hasFailed = false;

	// Use this for initialization
	void Start ()
	{
        //var options = new Options {Radius = 1, Resolution = 2};
        //var vertices = new GeodesicGrid(options).Faces.SelectMany(face => face.Vertices).Distinct();
        //var positions = vertices.Select(vertex => UnityVectorToDoubleArray(vertex.Position)).ToArray();
        var positions = new List<double[]>
        {
            VectorAt(0, 0),
            VectorAt(45, -45),
            VectorAt(45, 45)
            //VectorAt(45, 135),
            //VectorAt(90, 0),
            //VectorAt(90, 90)
        };
        //positions = Enumerable.Range(0, 500).Select(i => CreateSphericalVector()).ToArray();
        //var positions1 = Enumerable.Range(0, 2).Select(i => VectorAt(10, 360 * i / 10.0f)).ToList();
        //var positions2 = Enumerable.Range(0, 2).Select(i => VectorAt(40, 360 * i / 10.0f)).ToList();
        //var positions = positions1.Concat(positions2).ToList();

        _diagram = new VoronoiDiagram(positions);
	    _drawer = new VoronoiDiagramDrawer(_diagram);
	}
	
	// Update is called once per frame
	void Update () {

        if (!_hasFailed && (Input.GetKey(KeyCode.N) || Input.GetKeyDown(KeyCode.F)))
	    {
	        try
	        {
	            _diagram.ProcessNextEvent();
	            _sweeplinePriority = (float) _diagram.Beachline.Sweepline.Priority;
	        }
	        catch (Exception exception)
	        {
	            _hasFailed = true;
                Debug.Log(exception);
	        }
            Debug.Log("Beachline: " + _diagram.Beachline);
            Debug.Log("Circles: " + _diagram.CircleEventQueue);
	    }
	    _diagram.Beachline.Sweepline.Priority = _sweeplinePriority;
        _drawer.UpdateVoronoiDiagram();
	}

    private float _sweeplinePriority = 0.0F;
    void OnGUI()
    {
        _sweeplinePriority = GUI.VerticalSlider(new Rect(25, 10, 25, 500), _sweeplinePriority, 2F, -2F);
        GUI.TextArea(new Rect(50, 10, 1000, 25), _diagram.Beachline.ToString());
        GUI.TextArea(new Rect(50, 36, 1000, 25), _diagram.CircleEventQueue.ToString());

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

    private double[] UnityVectorToDoubleArray(UnityEngine.Vector3 v)
    {
        return new double[] {v.x, v.y, v.z};
    }
}
