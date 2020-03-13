using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TriangleHunt.Models
{
    public class TriangleDetails
    {
        public string TriangleKey { get; set; } = string.Empty;
        public Point Vertex1 { get; set; } = Point.Empty;
        public Point Vertex2 { get; set; } = Point.Empty;
        public Point Vertex3 { get; set; } = Point.Empty;

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(TriangleKey) || (Vertex1 == Point.Empty && Vertex2 == Point.Empty && Vertex3 == Point.Empty))
                {
                    return false;
                }
                return true;
            }
        }

    }
}
