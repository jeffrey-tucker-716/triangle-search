using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TriangleHunt.Models;

namespace TriangleHunt.Tests
{
    public class TriangleDetailsTests
    {
        TriangleResolver triangleResolver;

        [SetUp]
        public void Setup()
        {
            triangleResolver = new TriangleResolver();
        }

        [Test]
        public void WhenGivenA1KeyThenReturnsFirstTriangleDetails()
        {

            var triangleDetails = triangleResolver.ResolveTriangleKey("A1");
            Assert.IsTrue(triangleDetails.IsValid);
            Assert.IsTrue(triangleDetails.TriangleKey == "A1");
            printTriangleDetails(triangleDetails);

            // {X=0,Y=10} {X=0,Y=0} {X=10,Y=10}
            Assert.IsTrue(triangleDetails.Vertex1.X == 0 && triangleDetails.Vertex1.Y == 10);
            Assert.IsTrue(triangleDetails.Vertex2.X == 0 && triangleDetails.Vertex2.Y == 0);
            Assert.IsTrue(triangleDetails.Vertex3.X == 10 && triangleDetails.Vertex3.Y == 10);
        }

        [Test]
        public void WhenGiveA0KeyThenReturnsInvalidTriangleDetails()
        {
            var triangleDetails = triangleResolver.ResolveTriangleKey("A0");
            Assert.IsFalse(triangleDetails.IsValid);
            printTriangleDetails(triangleDetails);
        }

        [Test]
        public void WhenGivenF12KeyThenReturnsLastTriangleDetails()
        {

            var triangleDetails = triangleResolver.ResolveTriangleKey("F12");
            printTriangleDetails(triangleDetails);
            Assert.IsTrue(triangleDetails.IsValid);
            Assert.IsTrue(triangleDetails.TriangleKey == "F12");

            // F12 { X = 60,Y = 50} { X = 50,Y = 50}  { X = 60,Y = 60}
            Assert.IsTrue(triangleDetails.Vertex1.X == 60 && triangleDetails.Vertex1.Y == 50);
            Assert.IsTrue(triangleDetails.Vertex2.X == 50 && triangleDetails.Vertex2.Y == 50);
            Assert.IsTrue(triangleDetails.Vertex3.X == 60 && triangleDetails.Vertex3.Y == 60);

        }

        [Test]
        public void WhenGivenF12KeyThenGetOneReturnsLastTriangleDetails()
        {
            var triangleDetails = triangleResolver.GetOne("F12");
            Assert.IsTrue(triangleDetails.IsValid);
            Assert.IsTrue(triangleDetails.TriangleKey == "F12");
            // F12 { X = 60,Y = 50} { X = 50,Y = 50}  { X = 60,Y = 60}
            Assert.IsTrue(triangleDetails.Vertex1.X == 60 && triangleDetails.Vertex1.Y == 50);
            Assert.IsTrue(triangleDetails.Vertex2.X == 50 && triangleDetails.Vertex2.Y == 50);
            Assert.IsTrue(triangleDetails.Vertex3.X == 60 && triangleDetails.Vertex3.Y == 60);
            printTriangleDetails(triangleDetails);
        }

#if NEEDED
        [Test]
        public void WhenGivenListOfValidAndInvalidKeysThenReturnsSetOfValidTriangleDetails()
        {
            string triangleKeys = "a1,B2,G15,F11";
            var triangleDetailsArray = triangleResolver.ResolveTriangleKeys(triangleKeys);
            Assert.IsTrue(triangleDetailsArray != null && triangleDetailsArray.Length == 3);
            // only return the valid ones!
            foreach (var item in triangleDetailsArray)
            {
                Console.WriteLine($"{item.TriangleKey.ToUpper()} {item.Vertex1} {item.Vertex2} {item.Vertex3}");
            }
        }
#endif

        [Test]
        public void WhenGivenD5KeyThenReturnsMiddleTriangleDetails()
        {
            var triangleDetails = triangleResolver.ResolveTriangleKey("D5");
            Assert.IsTrue(triangleDetails.IsValid);
            Assert.IsTrue(triangleDetails.TriangleKey == "D5");
            printTriangleDetails(triangleDetails);

            // D5 {X=20,Y=40} {X=20,Y=30} {X=30,Y=40}
            Assert.IsTrue(triangleDetails.Vertex1.X == 20 && triangleDetails.Vertex1.Y == 40);
            Assert.IsTrue(triangleDetails.Vertex2.X == 20 && triangleDetails.Vertex2.Y == 30);
            Assert.IsTrue(triangleDetails.Vertex3.X == 30 && triangleDetails.Vertex3.Y == 40);
        }

        [Test]
        public void WhenGivenValidCoordinatesThenReturnsA2TriangleKey()
        {
            // {X=10,Y=0} {X=0,Y=0} {X=10,Y=10}
            var triangleDetailsSansKey = new TriangleDetails()
            {
                Vertex1 = new System.Drawing.Point(10, 0),
                Vertex2 = new System.Drawing.Point(0, 0),
                Vertex3 = new System.Drawing.Point(10, 10)
            };
            bool success = triangleResolver.GetTriangleKeyFromVertices(triangleDetailsSansKey);
            Assert.IsTrue(success);
            Assert.AreEqual("A2", triangleDetailsSansKey.TriangleKey.ToUpper());
        }

        [Test]
        public void WhenGivenInvalidCoordinatesThenReturnsNoTriangleKey()
        {
            var triangleDetailsSansKey = new TriangleDetails()
            {
                Vertex1 = new System.Drawing.Point(11, 0),
                Vertex2 = new System.Drawing.Point(0, 0),
                Vertex3 = new System.Drawing.Point(11, 11)
            };
            bool success = triangleResolver.GetTriangleKeyFromVertices(triangleDetailsSansKey);
            Assert.IsFalse(success);
            Assert.IsTrue(string.IsNullOrEmpty(triangleDetailsSansKey.TriangleKey));
        }

        [Test]
        public void WhenGivenInvalidCoordinatesOutsideGridThenReturnsNoTriangleKey()
        {
            var triangleDetailsSansKey = new TriangleDetails()
            {
                Vertex1 = new System.Drawing.Point(71, 0),
                Vertex2 = new System.Drawing.Point(0, 0),
                Vertex3 = new System.Drawing.Point(11, 81)
            };
            bool success = triangleResolver.GetTriangleKeyFromVertices(triangleDetailsSansKey);
            Assert.IsFalse(success);
            Assert.IsTrue(string.IsNullOrEmpty(triangleDetailsSansKey.TriangleKey));
        }

        [Test]
        public void WhenGivenMissingCoordinatesThenReturnsNoTriangleKey()
        {
            var triangleDetails = new TriangleDetails();
            bool success = triangleResolver.GetTriangleKeyFromVertices(triangleDetails);
            Assert.IsFalse(success);
            Assert.IsTrue(string.IsNullOrEmpty(triangleDetails.TriangleKey));
            printTriangleDetails(triangleDetails);
        }

        [Test]
        public void WhenGivenTooBigTriangleCoordinatesThenReturnsNoTriangleKey()
        {
            var triangleDetails = new TriangleDetails()
            {   // twice the size of accepted triangle
                Vertex1 = new System.Drawing.Point(0, 0),
                Vertex2 = new System.Drawing.Point(20, 0),
                Vertex3 = new System.Drawing.Point(20, 20)
            };
            bool success = triangleResolver.GetTriangleKeyFromVertices(triangleDetails);
            Assert.IsFalse(success);
            Assert.IsTrue(string.IsNullOrEmpty(triangleDetails.TriangleKey));
            printTriangleDetails(triangleDetails);
        }

        [Test]
        public void WhenGetAllTrianglesThenReturnsAll()
        {
            var allTriangles = triangleResolver.GetAll();
            Assert.IsNotNull(allTriangles);
            Assert.IsTrue(allTriangles.Length == 72);
            foreach (var triangleDetails in allTriangles)
            {
                printTriangleDetails(triangleDetails);
            }
        }

        [Test]
        public void WhenVertexParseThenGetBackPoint()
        {
            string vertex = "10,0";
            var coordinates = StringToIntList(vertex);
            Assert.IsTrue(coordinates.Count() == 2);
            foreach (var item in coordinates.ToArray())
            {
                Console.WriteLine($"coordinate={item}");
            }
        }

        [Test]
        public void WhenVertexParseMissingYCoordinateThenGetException()
        {
            string badVertex = "20,";
            try
            {
                var coordinates = StringToIntList(badVertex);
                Assert.IsTrue(coordinates.Count() == 1);
            }
            catch (FormatException fe)
            {
                Console.WriteLine($"Hit the expected format exception {fe}");
            }
        }

        [Test]
        public void WhenVertexParseHasOnlyXThenFlagIt()
        {
            string badVertex = "20";
            try
            {
                var coordinates = StringToIntList(badVertex);
                if (coordinates.Count() == 1)
                    throw new ApplicationException($"There should be X,Y coordinates in '{badVertex}'");
            }
            catch (ApplicationException e)
            {
                Console.WriteLine($"Hit the expected exception {e}");
            }
        }

        /// <summary>
        /// Just wanted to test drive this
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private IEnumerable<int> StringToIntList(string str)
        {
            return (str ?? "").Split(',').Select<string, int>(int.Parse);
        }

        private void printTriangleDetails(TriangleDetails triangleDetails)
        {
            Console.WriteLine($"key={triangleDetails.TriangleKey}, vertex1={triangleDetails.Vertex1}, vertex2={triangleDetails.Vertex2}, vertex3={triangleDetails.Vertex3}");
        }
    }
}