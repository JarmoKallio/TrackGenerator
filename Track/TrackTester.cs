using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrackGenerator;
using System;
using System.IO;

namespace TrackGenerator
{

public class TrackTester: MonoBehaviour
{
	public float rotAngle;
	public int dublicateNumber=1;
    private Mesh mesh;
    private int started =0;
    private Vector3[] trackVertices;
    private Vector2Int[] trackEdges;
    private TrackSegment seg;
    private string objFilePath = "Assets/Track/res/TestMesh.obj";

	void Start () {
        ParseObj();
        Create();
	}
	
    // Update is called once per frame
	void Update () {

    }

    private void Create(){
        var mf = GetComponent<MeshFilter>();
		mesh = new Mesh();
		mf.mesh = mesh;

        seg = new TrackSegment(trackVertices,trackEdges,rotAngle,dublicateNumber, mesh);

    }

    private void ParseObj(){
        SimpleObjParser parser = new SimpleObjParser();
        trackVertices = parser.GetVertices(objFilePath);
        trackEdges = parser.GetEdges(objFilePath);
    }
}

}
