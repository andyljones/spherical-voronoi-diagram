using System;
using System.Collections.Generic;
using System.Linq;
using Graphics;
using UnityEngine;
using CyclicalSkipList;
using System.Collections;

public class load : MonoBehaviour
{
    private VoronoiDiagramDrawer _drawer;
    private VoronoiDiagram _diagram;

	// Use this for initialization
	void Start ()
	{
	    var positions = new List<Vector3>
	    {
	        MathUtils.CreateVectorAt(45, 0),
	        MathUtils.CreateVectorAt(95, 5),
	        MathUtils.CreateVectorAt(90, 10),
	        MathUtils.CreateVectorAt(100, 0)
	    };

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
}
