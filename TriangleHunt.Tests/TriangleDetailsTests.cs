using NUnit.Framework;
using System;
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
            Console.WriteLine($"{triangleDetails.TriangleKey} {triangleDetails.Vertex1} {triangleDetails.Vertex2} {triangleDetails.Vertex3}");
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
        }

        [Test]
        public void WhenGivenF12KeyThenReturnsLastTriangleDetails()
        {

            var triangleDetails = triangleResolver.ResolveTriangleKey("F12");
            Assert.IsTrue(triangleDetails.IsValid);
            Assert.IsTrue(triangleDetails.TriangleKey == "F12");
            Console.WriteLine($"{triangleDetails.TriangleKey} {triangleDetails.Vertex1} {triangleDetails.Vertex2} {triangleDetails.Vertex3}");
            // F12 { X = 60,Y = 50} { X = 50,Y = 50}  { X = 60,Y = 60}
            Assert.IsTrue(triangleDetails.Vertex1.X == 60 && triangleDetails.Vertex1.Y == 50);
            Assert.IsTrue(triangleDetails.Vertex2.X == 50 && triangleDetails.Vertex2.Y == 50);
            Assert.IsTrue(triangleDetails.Vertex3.X == 60 && triangleDetails.Vertex3.Y == 60);
        }
               
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

        [Test]
        public void WhenGivenD5KeyThenReturnsMiddleTriangleDetails()
        {
            var triangleDetails = triangleResolver.ResolveTriangleKey("D5");
            Assert.IsTrue(triangleDetails.IsValid);
            Assert.IsTrue(triangleDetails.TriangleKey == "D5");
            Console.WriteLine($"{triangleDetails.TriangleKey} {triangleDetails.Vertex1} {triangleDetails.Vertex2} {triangleDetails.Vertex3}");
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
            var triangleDetailsSansKey = new TriangleDetails();
            bool success = triangleResolver.GetTriangleKeyFromVertices(triangleDetailsSansKey);
            Assert.IsFalse(success);
            Assert.IsTrue(string.IsNullOrEmpty(triangleDetailsSansKey.TriangleKey));
        }

        [Test]
        public void WhenGivenTooBigTriangleCoordinatesThenReturnsNoTriangleKey()
        {
            var triangleDetailsSansKey = new TriangleDetails()
            {   // twice the size of accepted triangle
                Vertex1 = new System.Drawing.Point(0, 0),
                Vertex2 = new System.Drawing.Point(20, 0),
                Vertex3 = new System.Drawing.Point(20, 20)
            };
            bool success = triangleResolver.GetTriangleKeyFromVertices(triangleDetailsSansKey);
            Assert.IsFalse(success);
            Assert.IsTrue(string.IsNullOrEmpty(triangleDetailsSansKey.TriangleKey));
        }
    }
}