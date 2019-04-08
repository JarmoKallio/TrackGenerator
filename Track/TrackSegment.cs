using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrackGenerator;

namespace TrackGenerator
{
	public class TrackSegment
	{

	private Vector3[] baseVertices;
	private Vector2Int[] edges;
	private VertexPath path;
	private int totalVertexNumber;
	private int vertsPerFace = 6;
	private int[] baseTriangles;
	private int[] triangles;
	private Vector3[] normals;
	private Vector3[] finalVertices;
	private Mesh mesh;

	public TrackSegment(Vector3[] baseVertices, VertexPath path, Vector2Int[] edges, Mesh m){
		this.baseVertices = baseVertices;
		this.path = path;
		this.edges = edges;
		this.totalVertexNumber = baseVertices.Length * path.getLenght();
		this.mesh = m;

		BuildTrackSegment();
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
		finalVertices = new Vector3[totalVertexNumber];
		
		for(int v = 0; v < path.getLenght(); v++){
			Vector3 vertex = new Vector3();
			Matrix4x4 m = GenerateRotationMatrix(path.getRotation(v));	
			
			Debug.Log(path.getRotation(v));

			for(int z = 0; z < baseVertices.Length; z++){
				//rotate
				vertex = m.MultiplyPoint3x4(baseVertices[z]);
				//translate
				vertex = vertex + path.getVertex(v);
				//add to list
				finalVertices[baseVertices.Length*(v) + z] = vertex;
			}
		}
	}

	private Matrix4x4 GenerateRotationMatrix(float rotation){
		Quaternion rot = Quaternion.Euler(0,rotation, 0);
		return Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
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
			baseTriangles[p+2] = edges[i].y + (baseVertices.Length);

			baseTriangles[p+3] = edges[i].x;
			baseTriangles[p+4] = edges[i].y + (baseVertices.Length);
			baseTriangles[p+5] = edges[i].x + (baseVertices.Length);
		
			p+=6;
		}

	}

	private void GenerateExtrudedTriangles(){
		/* 	Faces and triangles fill the gaps between consecutive dublicated vertex groups, so the 
			triangle groups are dublicated one time less than vertex groups. Size of a vertex group
			is equal to that of original models vertex count. Size of a triangel group is equal to
			number of edges between original vertices times number of vertices per face. The later one
			is six. It is assumed that face contains two triangles. */
		int numberOfExtrusions = path.getLenght()-1;
		triangles = new int[(totalVertexNumber * vertsPerFace) - (baseVertices.Length*vertsPerFace)];
		int c=0;
		for(int x = 0; x < numberOfExtrusions; x++){
			for(int i = 0; i < baseTriangles.Length; i++){
				triangles[c] = baseTriangles[i]+(x*baseVertices.Length);
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