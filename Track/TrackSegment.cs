using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrackGenerator;

namespace TrackGenerator
{
	public class TrackSegment
	{

	private Vector3[] vertices;
	private Vector2Int[] edges;
	private float yRotation;
	private int totalVertexNumber;
	private int dublicateNumber;
	private int vertsPerFace = 6;
	private int[] baseTriangles;
	private int[] triangles;
	private Vector3[] normals;
	private Vector3[] finalVertices;
	private Mesh mesh;

	public TrackSegment(Vector3[] v, Vector2Int[] e, float yR, int dn, Mesh m){
		if(dn>0){
		vertices = v;
		edges = e;
		yRotation = yR;
		dublicateNumber = dn;
		totalVertexNumber = vertices.Length * (dublicateNumber +1);
		mesh =m;

		BuildTrackSegment();
		}
	}

	public void UpdateSegment(float yR, int dn){
		yRotation = yR;
		dublicateNumber = dn;
		UpdateTrackSegment();
	}

	private void UpdateTrackSegment(){
		BuildVertices();
		mesh.vertices = finalVertices;
		mesh.RecalculateNormals();
	}

	private void BuildTrackSegment(){		
		BuildVertices();
		GenerateTriangles();
		GeneratePlaceholderNormals();

		mesh.vertices = finalVertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.RecalculateNormals();
	}

	private void BuildVertices(){
		Vector3[] currentVertices = vertices;
		finalVertices = new Vector3[totalVertexNumber];
		
		//adding the original vertices to the list as is
		for(int a = 0; a<vertices.Length;a++){
			finalVertices[a] = vertices[a];
		}

		//create roration matrix
		Quaternion rot = Quaternion.Euler(0,yRotation, 0);
		Matrix4x4 m = Matrix4x4.TRS(Vector3.zero,rot, Vector3.one);
		//Matrix4x4 inv = m.inverse;
		
		
		//adding the following ones transformed
		
		for(int v = 1; v <= dublicateNumber; v++){
			int transformAlongAxis = v;	
			//rotate vertices and add to list
			for(int z = 0; z< currentVertices.Length;z++){
				currentVertices[z] = m.MultiplyPoint3x4(currentVertices[z]);

				currentVertices[z]= currentVertices[z] + new Vector3(0,0,transformAlongAxis*v);

				finalVertices[vertices.Length*(v) + z] = currentVertices[z];
			}
		}
	}

	private void GenerateTriangles(){
		GenerateBaseTriangles();
		GenerateExtrudedTriangles();
	}

	private void GenerateBaseTriangles(){
		baseTriangles = new int[edges.Length*vertsPerFace];

		int p = 0;

		for(int i = 0; i < edges.Length; i++ ){
			baseTriangles[p] = edges[i].x;
			baseTriangles[p+1] = edges[i].y;
			baseTriangles[p+2] = edges[i].y + (vertices.Length);

			baseTriangles[p+3] = edges[i].x;
			baseTriangles[p+4] = edges[i].y + (vertices.Length);
			baseTriangles[p+5] = edges[i].x + (vertices.Length);
		
			p+=6;
		}

	}

	private void GenerateExtrudedTriangles(){
		/* 	Faces and triangles fill the gabs between consecutive dublicated vertex groups, so the 
			triangle groups are dublicated one time less than vertex groups. Size of a vertex group
			is equal to that of original models vertex count. Size of a triangel group is equal to
			number of edges between original vertices times number of vertices per face. The later one
			is of course six. */

		triangles = new int[(totalVertexNumber * vertsPerFace) - (vertices.Length*vertsPerFace)];
		int c=0;
		for(int x = 0; x < dublicateNumber; x++){
			for(int i = 0; i < baseTriangles.Length; i++){
				triangles[c] = baseTriangles[i]+(x*vertices.Length);
				c++;
			}
		}
	}

	private void GeneratePlaceholderNormals(){
		normals = new Vector3[totalVertexNumber];
		for(int i =0; i<normals.Length;i++){
			normals[i] = new Vector3(1,1,1);
		}

	}


	}
    
}