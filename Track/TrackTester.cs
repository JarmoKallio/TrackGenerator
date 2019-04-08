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
    private Mesh mesh;
    private Vector3[] trackVertices;
    private VertexPath path;
    private Vector2Int[] trackEdges;
    private TrackSegment seg;
    private string objFilePath = "Assets/TrackGenerator/Track/res/TestMesh.obj";

	void Start () {
        ParseObj();
        createTestPath();
        Create();
	}
	
	void Update () {

    }

    private void Create(){
        var mf = GetComponent<MeshFilter>();
		mesh = new Mesh();
		mf.mesh = mesh;
        seg = new TrackSegment(trackVertices, path, trackEdges, mesh);

    }

    private void ParseObj(){
        SimpleObjParser parser = new SimpleObjParser();
        trackVertices = parser.GetVertices(objFilePath);
        trackEdges = parser.GetEdges(objFilePath);
    }

    private void createTestPath(){
        
/* 
        Vector3[] pathPoints = new Vector3[4];
        pathPoints[0] = new Vector3(0,0,0);
        pathPoints[1] = new Vector3(0,0,3);
        pathPoints[2] = new Vector3(3,0,3);
        pathPoints[3] = new Vector3(3,0,0);
*/
        
        Vector3[] pathPoints = new Vector3[8];

        pathPoints[0] = new Vector3(0,0,0);
        pathPoints[1] = new Vector3(0,0,2);
        pathPoints[2] = new Vector3(-1,0,4);
        pathPoints[3] = new Vector3(-3,0,5);

        pathPoints[4] = new Vector3(-5,0,5);
        pathPoints[5] = new Vector3(-7,0,4);
        pathPoints[6] = new Vector3(-8,0,2);
        pathPoints[7] = new Vector3(-8,0,0);
        
        /*
        pathPoints[0] = new Vector3(0,0,0);
        pathPoints[1] = new Vector3(0,0,2);
        pathPoints[2] = new Vector3(1,0,4);
        pathPoints[3] = new Vector3(3,0,5);

        pathPoints[4] = new Vector3(5,0,5);
        pathPoints[5] = new Vector3(7,0,4);
        pathPoints[6] = new Vector3(8,0,2);
        pathPoints[7] = new Vector3(8,0,0);
        */
        /* 
        path[0] = new Vector3(0,0,0);
        path[1] = new Vector3(0,0,2);
        path[2] = new Vector3(1,0,4);
        path[3] = new Vector3(3,0,5);
        path[4] = new Vector3(5,0,5);
        path[5] = new Vector3(7,0,4);
        */
        float rotationOfFirstVert = 0f;
        float rotationOfLastVert = -180f;
        path = new VertexPath(pathPoints,rotationOfFirstVert, rotationOfLastVert);
    }

}

}
