using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrackGenerator;

namespace TrackGenerator
{
    /*
    Contains a list of points on a path and rotations around y axis. Can be used
    to dublicate meshes, rotate them and displace them. The path of VertexPath give the
    position where each dublication goes and the rotation values tell the amount of y rotation
    the dublicated mesh receives.
     */

    public class VertexPath
    {
        private Vector3[] vertices;
        private float[] rotations;
        private Vector3 normalOfFirstVertex;
        public VertexPath(Vector3[] path, float rotOfFirstVertex, float rotOfLastVertex)
        {
            this.vertices = path;
            this.rotations = new float[path.Length];
            this.rotations[0] = rotOfFirstVertex;
            this.rotations[path.Length - 1] = rotOfLastVertex;

            this.normalOfFirstVertex = CalculateNormalVector(path[0], rotOfFirstVertex);
            CalculateYRotations();
        }

        public Vector3 getVertex(int index)
        {
            return vertices[index];
        }

        public float getRotation(int index)
        {
            return rotations[index];
        }

        public int getLenght()
        {
            return this.rotations.Length;
        }
        private void CalculateYRotations()
        {
            if (vertices.Length > 2)
            {
                Vector3 yAxis = new Vector3(0, 1, 0);
                Vector3 from;
                Vector3 to;

                from = this.normalOfFirstVertex;

                //we first calculate rotations between each vector and the first vector
                for (int i = 1; i < vertices.Length; i++)
                {
                    to = vertices[i] - vertices[i - 1];

                    rotations[i] = Vector3.SignedAngle(from, to, yAxis);
                }

                //we average two consecutive rotations, excluding the last one
                for (int i = 1; i < vertices.Length - 2; i++)
                {
                    rotations[i] = (rotations[i] + rotations[i + 1]) / 2f;
                }

            }
            else
            {
                Debug.Log("Lenght of path should be at least three points.");
            }

        }

        private Vector3 CalculateNormalVector(Vector3 position, float angle)
        {
            // Here it is assumed we are calculating normal vector for a face, the middle point of
            // which is input "position" in xy plane that has been rotated the amount of input "angle"
            // around y-axis
            Vector3 xyNormal = new Vector3(0, 0, 1);

            Quaternion rot = Quaternion.Euler(0, angle, 0);
            Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
            Vector3 normal = m.MultiplyPoint3x4(xyNormal);

            return normal;
        }

    }

}