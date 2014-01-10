using System.Collections.Generic;
using System.Linq;
using Generator;
using Graphics;
using UnityEngine;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public class load : MonoBehaviour
{
    private Random _random = new Random();

    private VoronoiDiagramDrawer _drawer;
    private VoronoiDiagram _diagram;

	// Use this for initialization
	void Start ()
	{
	    var positions = new List<double[]>();
        //{
        //    new double[] {0, 0, 1},
        //    new double[] {1, 0, 0}
        //};
        positions = Enumerable.Range(0, 100).Select(i => CreateSphericalVector()).ToList();

        _diagram = new VoronoiDiagram(positions);

	    _drawer = new VoronoiDiagramDrawer(_diagram);
        
        Debug.Log(_diagram.Beachline);
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKey(KeyCode.N) || Input.GetKeyDown(KeyCode.F))
	    {
            _diagram.ProcessNextEvent();
            _drawer.UpdateVoronoiDiagram();
            Debug.Log(_diagram.Beachline);
	    }
	}

    private double[] CreateSphericalVector()
    {
        var z = (float)(-1 + 2*_random.NextDouble());
        var azimuth = (float)(2 * Mathf.PI * _random.NextDouble());

        var x = Mathf.Sqrt(1 - z*z) * Mathf.Cos(azimuth);
        var y = -Mathf.Sqrt(1 - z*z) * Mathf.Sin(azimuth);

        return new double[] {x, y, z};
    }
}
