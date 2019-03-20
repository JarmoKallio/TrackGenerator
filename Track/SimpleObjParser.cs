using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TrackGenerator
{

    public class SimpleObjParser
    {
        Regex vertexRegex = new Regex(@"\u0076\s(-?\d*\.\d*)\s(-?\d*\.\d*)\s(-?\d*\.\d*)");
        Regex edgeRegex = new Regex(@"\u006c\s(\d*)\s(\d*)");

        public Vector3[] GetVertices(string filePath){
            string s = File.ReadAllText(filePath);
            MatchCollection matches = vertexRegex.Matches(s);

            if(matches.Count > 0){
                return VertexMatches(matches);
            } else {
                Debug.Log("Found no matches when extracting vertex data from file: "+ filePath);
                return null;
            }
        }

        public Vector2Int[] GetEdges(string filePath){
            string s = File.ReadAllText(filePath);
            MatchCollection matches = edgeRegex.Matches(s);

            if(matches.Count > 0){
                return EdgeMatches(matches);
            } else {
                Debug.Log("Found no matches when extracting edge data from file: "+ filePath);
                return null;
            }
        }

        private Vector3[] VertexMatches(MatchCollection matches){
            Vector3[] values = new Vector3[matches.Count];
            for(int i=0; i<matches.Count; i++){
                Match match = matches[i];
                GroupCollection groups = match.Groups;
                values[i] = new Vector3(Pfloat(match.Groups[1].Value),Pfloat(match.Groups[2].Value),Pfloat(match.Groups[3].Value));
            }
            return values;
        }

        private Vector2Int[] EdgeMatches(MatchCollection matches){
            Vector2Int[] values = new Vector2Int[matches.Count];
            for(int i=0; i<matches.Count; i++){
                Match match = matches[i];
                GroupCollection groups = match.Groups;
                values[i] = new Vector2Int(Pint(match.Groups[1].Value),Pint(match.Groups[2].Value));
            }
            values = setFirstVertexIndexZero(values);
            return values;
        }

        private Vector2Int[] setFirstVertexIndexZero(Vector2Int[] edges){
        //It seems Obj files start vertex indexing from 1 in its edges, so we have to subtract one from every value
            for(int i = 0; i<edges.Length;i++){
                edges[i].x -=1;
                edges[i].y -=1;
            }
        
        return edges;
        }

        private float Pfloat(string s){
            float f=0f;

            try
            {
                f = float.Parse(s, CultureInfo.InvariantCulture.NumberFormat);    
            }
            catch (FormatException)
            {
                Debug.Log("Could not parse string " + s + " to float.");
            }
            return f;
        }

        private int Pint(string s){
            int a=0;

            try
            {
                a = Int32.Parse(s);    
            }
            catch (FormatException)
            {
                Debug.Log("Could not parse string " + s + " to int.");
            }

            return a;
        }

    }

}