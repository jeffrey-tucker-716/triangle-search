using System;
using System.Linq;
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
        const double MinDistanceBetweenPoints = 10.00d;
        readonly double MaxDistanceBetweenPoints = Math.Round(Math.Sqrt((Math.Pow(10, 2) + Math.Pow(10, 2))), 2);

        public TriangleDetails[] ResolveTriangleKeys(string delimitedKeys)
        {

            List<TriangleDetails> listTriangleDetails = new List<TriangleDetails>();
            var keys = parseTriangleKeys(delimitedKeys);
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

        public bool GetTriangleKeyFromVertices(TriangleDetails triangleDetails)
        {
            bool foundTriangleKey = false;
            if (!areCoordinatesValid(triangleDetails))
            {
                return foundTriangleKey;    // false
            }

            // or too far apart
            Point pointOppositeHypotenuse = Point.Empty;
            if (!doVerticesMeet(triangleDetails, ref pointOppositeHypotenuse))
            {
                return foundTriangleKey;    // false
            }
            // we should know which point is opposite to determine if it is the upper or lower.
            // figure out the left, right, top, bottom
            var yCoordinates = new List<int>()
            {
                triangleDetails.Vertex1.Y,
                triangleDetails.Vertex2.Y,
                triangleDetails.Vertex3.Y
            };
            var xCoordinates = new List<int>()
            {
                triangleDetails.Vertex1.X,
                triangleDetails.Vertex2.X,
                triangleDetails.Vertex3.X
            };

            int top = yCoordinates.ToArray().Min();
            int bottom = yCoordinates.ToArray().Max();
            int left = xCoordinates.ToArray().Min();
            int right = xCoordinates.ToArray().Max();
            string triangleKey = string.Empty;
            switch (bottom)
            {
                case 10:
                    triangleKey = "A";
                    break;
                case 20:
                    triangleKey = "B";
                    break;
                case 30:
                    triangleKey = "C";
                    break;
                case 40:
                    triangleKey = "D";
                    break;
                case 50:
                    triangleKey = "E";
                    break;
                case 60:
                    triangleKey = "F";
                    break;
                default:
                    throw new ApplicationException($"Could not determine the row from {bottom}");
            }

            if (pointOppositeHypotenuse.Y  == top && pointOppositeHypotenuse.X == right)
            {
                // this is the even one
                switch (right)
                {
                    case 10:
                        triangleKey += "2";
                        break;
                    case 20:
                        triangleKey += "4";
                        break;
                    case 30:
                        triangleKey += "6";
                        break;
                    case 40:
                        triangleKey += "8";
                        break;
                    case 50:
                        triangleKey += "10";
                        break;
                    case 60:
                        triangleKey += "12";
                        break;
                }
            }
            else if (pointOppositeHypotenuse.Y == bottom && pointOppositeHypotenuse.X == left)
            {
                switch (left)
                {
                    case 0:
                        triangleKey += "1";
                        break;
                    case 10:
                        triangleKey += "3";
                        break;
                    case 20:
                        triangleKey += "5";
                        break;
                    case 30:
                        triangleKey += "7";
                        break;
                    case 40:
                        triangleKey += "9";
                        break;
                    case 50:
                        triangleKey += "11";
                        break;
                }
            }
            triangleDetails.TriangleKey = triangleKey;
            foundTriangleKey = true;

            return foundTriangleKey;
        }

        private bool areCoordinatesValid(TriangleDetails triangleDetails)
        {
            // checks if all points are filled in.
            if (!areAllVerticesMissing(triangleDetails))
            {
                return false;
            }

            // if any of the points are out of range of 60x60 pixel grid, then false
            if (!verticesInGrid(triangleDetails))
            {
                return false;
            }

            // if any of the points are not divisible by 10, 
            if (!coordinatesDivisibleBy10(triangleDetails))
            {
                return false;
            }


            return true;

        }

        private bool areAllVerticesMissing(TriangleDetails triangleDetails)
        {
            if (triangleDetails.Vertex1 == Point.Empty && triangleDetails.Vertex2 == Point.Empty && triangleDetails.Vertex3 == Point.Empty)
            {
                return false;
            }
            return true;
        }

        private bool coordinatesDivisibleBy10(TriangleDetails triangleDetails)
        {
            if (triangleDetails.Vertex1.X % 10 != 0 || triangleDetails.Vertex1.Y % 10 != 0)
            {
                return false;
            }
            if (triangleDetails.Vertex2.X % 10 != 0 || triangleDetails.Vertex2.Y % 10 != 0)
            {
                return false;
            }
            if (triangleDetails.Vertex3.X % 10 != 0 || triangleDetails.Vertex3.Y % 10 != 0)
            {
                return false;
            }
            return true;
        }

        private bool verticesInGrid(TriangleDetails triangleDetails)
        {
            // if any of the points are out of range of 60x60 pixel grid, then false
            if (!pointInRange(triangleDetails.Vertex1, 60, 60))
            {
                return false;
            }
            if (!pointInRange(triangleDetails.Vertex2, 60, 60))
            {
                return false;
            }
            if (!pointInRange(triangleDetails.Vertex3, 60, 60))
            {
                return false;
            }
            return true;
        }

        private bool doVerticesMeet(TriangleDetails triangleDetails, ref Point pointOppositeHypotenuse)
        {
            double distance1 = Math.Round(Math.Sqrt(Math.Pow((triangleDetails.Vertex2.X - triangleDetails.Vertex1.X), 2) +
                Math.Pow((triangleDetails.Vertex2.Y - triangleDetails.Vertex1.Y), 2)), 2);
            double distance2 = Math.Round(Math.Sqrt(Math.Pow((triangleDetails.Vertex3.X - triangleDetails.Vertex1.X), 2) +
                Math.Pow((triangleDetails.Vertex3.Y - triangleDetails.Vertex1.Y), 2)), 2);
            double distance3 = Math.Round(Math.Sqrt(Math.Pow((triangleDetails.Vertex3.X - triangleDetails.Vertex2.X), 2) +
                Math.Pow((triangleDetails.Vertex3.Y - triangleDetails.Vertex2.Y), 2)), 2);

            // at least one distance == MaxDistanceBetweenPoints, and 2 distances equal MinDistanceBetweenPoints
            bool twoEquidistantLegs = false;
            bool hypotenuseExists = false;
            if (distance1 == MinDistanceBetweenPoints && distance2 == MinDistanceBetweenPoints)
            {
                twoEquidistantLegs = true;
            }
            else if (distance1 == MinDistanceBetweenPoints && distance3 == MinDistanceBetweenPoints)
            {
                twoEquidistantLegs = true;
            }
            else if (distance2 == MinDistanceBetweenPoints && distance3 == MinDistanceBetweenPoints)
            {
                twoEquidistantLegs = true;
            }
            pointOppositeHypotenuse = Point.Empty;
            if (distance1 == MaxDistanceBetweenPoints)
            {
                hypotenuseExists = true;
                pointOppositeHypotenuse = triangleDetails.Vertex3;
            }
            else if (distance2 == MaxDistanceBetweenPoints)
            {
                hypotenuseExists = true;
                pointOppositeHypotenuse = triangleDetails.Vertex2;
            }
            else if (distance3 == MaxDistanceBetweenPoints)
            {
                hypotenuseExists = true;
                pointOppositeHypotenuse = triangleDetails.Vertex1;
            }
                       
            if (hypotenuseExists && twoEquidistantLegs)
                return true;

            return false;
        }

        private bool pointInRange(Point vertex, int maxX, int maxY)
        {
            // min is always 0.
            if (vertex.X > maxX || vertex.X < 0)
                return false;
            if (vertex.Y > maxY || vertex.Y < 0)
                return false;
            return true;
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


        private string[] parseTriangleKeys(string delimitedKeys)
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

        private void calculateTriangleVertexes(TriangleDetails triangleDetails, bool isUpperRightTriangle, string rowName, int column)
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
                Console.WriteLine($"Exception is {e}");
            }
        }
    }
}
