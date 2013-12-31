using System;
using System.Collections.Generic;
using System.Linq;
using Graphics;
using UnityEngine;
using CyclicalSkipList;
using System.Collections;

public class load : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    var positions = new List<Vector3>
	    {
	        MathUtils.CreateVectorAt(45, 0),
	        MathUtils.CreateVectorAt(90, -10),
	        MathUtils.CreateVectorAt(90, 10),
	        MathUtils.CreateVectorAt(100, 0)
	    };

        var generator = new VoronoiGenerator(positions);
        generator.ProcessNextEvent();
        generator.ProcessNextEvent();
        generator.ProcessNextEvent();
        generator.ProcessNextEvent();
        generator.ProcessNextEvent();

        VoronoiGeneratorDrawer.DrawVoronoiGenerator(generator);

        Debug.Log(generator.Beachline);

        //var beachline = new Beachline();
        //beachline.Insert(new SiteEvent(positions[0]));
        //beachline.Insert(new SiteEvent(positions[1]));

        //BeachlineDrawer.DrawBeachline(beachline);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
