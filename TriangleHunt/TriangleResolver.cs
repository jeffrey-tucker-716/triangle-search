using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using TriangleHunt.Models;

namespace TriangleHunt
{
    public class TriangleResolver
    {
        const string keyMatchPattern = @"[A-Fa-f]([1-9]\b|1[0-2]){1}\b";
        const string rowMatchPattern = @"[A-Fa-f]";
        const string columnMatchPattern = @"([1-9]\b|1[0-2]){1}";

        public string[] ParseTriangleKeys(string delimitedKeys)
        {
            List<string> listKeys = new List<string>();
            MatchCollection matches = Regex.Matches(delimitedKeys, keyMatchPattern);
            foreach (Match m in matches)
            {
                string key = m.ToString();
                listKeys.Add(key);
            }
            return listKeys.ToArray();
        }
        public TriangleDetails[] ResolveTriangleKeys(string delimitedKeys)
        {

            List<TriangleDetails> listTriangleDetails = new List<TriangleDetails>();
            var keys = ParseTriangleKeys(delimitedKeys);
            foreach(var key in keys)
            {
                var localDetails = ResolveTriangleKey(key);
                listTriangleDetails.Add(localDetails);
            }
            return listTriangleDetails.ToArray();
        }
        public TriangleDetails ResolveTriangleKey(string key)
        {
            var triangleDetails = new TriangleDetails();
            if (validateKey(key))
            {
                triangleDetails = calculateVertexCoordinates(key);
            }


            return triangleDetails;
        }

        private bool validateKey(string key)
        {
            // there should only be a single match
            MatchCollection matches = Regex.Matches(key, keyMatchPattern);
            if (matches.Count != 1)
            {
                return false;
            }
            return true;
        }

        private TriangleDetails calculateVertexCoordinates(string key)
        {
            TriangleDetails triangleDetails = new TriangleDetails();
            triangleDetails.TriangleKey = key;
            MatchCollection columnMatch = Regex.Matches(key, columnMatchPattern);
            MatchCollection rowMatch = Regex.Matches(key, rowMatchPattern);
            if (columnMatch.Count == 1 && rowMatch.Count == 1)
            {
              
                string rowName = rowMatch[0].ToString();  // A-F
                string columnName = columnMatch[0].ToString();  // 1-12 
                int column = int.Parse(columnName);
                // the triangle is even so the upper right hand triangle of the pair
                bool isUpperRightTriangle = column % 2 == 0;
                calculateTriangleVertexes(triangleDetails, isUpperRightTriangle, rowName, column);
            }
                    
            return triangleDetails;
        }

        void calculateTriangleVertexes(TriangleDetails triangleDetails, bool isUpperRightTriangle, string rowName, int column)
        {
            
            try
            {
                // points ON the hypotenuse (top-left and bottom-right) are the same for both triangles
                // points opposite the hypotenuse differ according to the isUpperRightTriangle flag.
                int top = 0, bottom = 0, left = 0, right = 0;
                // for the y-coordinates of our 10x10 square
                // A -> 0 is top, 10 is bottom, 
                // B -> 10 is top, 20 is bottom, 
                // C -> 20 is top, 30 is bottom 
                // D -> 30 is top, 40 is bottom 
                // E -> 40 is top, 50 is bottom 
                // F -> 50 is top, 60 is bottom 
                switch (rowName.ToUpper())
                {
                    case "A":
                        top = 0;
                        bottom = 10;
                        break;
                    case "B":
                        top = 10;
                        bottom = 20;
                        break;
                    case "C":
                        top = 20;
                        bottom = 30;
                        break;
                    case "D":
                        top = 30;
                        bottom = 40;
                        break;
                    case "E":
                        top = 40;
                        bottom = 50;
                        break;
                    case "F":
                        top = 50;
                        bottom = 60;
                        break;
                    default:
                        throw new ApplicationException($"Unexpected row named {rowName}");
                }

                // 1-2 -> 0 is left, 10 is right
                // 3-4 -> 10 is left, 20 is right
                // 5-6 -> 20 is left, 30 is right
                // 7-8 -> 30 is left, 40 is right
                // 9-10 -> 40 is left, 50 is right
                // 11-12 -> 50 is left, 60 is right
                if (column == 1 || column == 2)
                {
                    left = 0;
                    right = 10;
                }
                else if (column == 3 || column == 4)
                {
                    left = 10;
                    right = 20;
                }
                else if (column == 5 || column == 6)
                {
                    left = 20;
                    right = 30;
                }
                else if (column == 7 || column == 8)
                {
                    left = 30;
                    right = 40;
                }
                else if (column == 9 || column == 10)
                {
                    left = 40;
                    right = 50;
                }
                else if (column == 11 || column == 12)
                {
                    left = 50;
                    right = 60;
                }
                else
                {
                    throw new ApplicationException($"Encountered invalid column number {column}");
                }

                // set the points on the hypotenuse first V2 (left top) , V3 (right, bottom)
                triangleDetails.Vertex1 = (isUpperRightTriangle) ? new Point(right, top) : new Point(left, bottom);
                triangleDetails.Vertex2 = new Point(left, top);
                triangleDetails.Vertex3 = new Point(right, bottom);
 
            }
            catch(Exception e)
            {
                // TODO: log out the exception to Azure telemetry or whatever
            }
        }
    }
}
