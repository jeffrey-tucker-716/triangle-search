using NUnit.Framework;
using System;
using TriangleHunt;
using TriangleHunt.Models;
using TriangleSearchWebapp.Controllers;

namespace TriangleSearchWebapp.Tests
{
    public class SearchTests
    {
        TrianglesController trianglesController;
        [SetUp]
        public void Setup()
        {
            trianglesController = new TrianglesController(new TriangleResolver());
        }

        [Test]
        public void WhenGetAllTrianglesThenShouldReturnAll()
        {
            var triangles = trianglesController.Get();
            int count = 0;
            foreach(var triangle in triangles)
            {
                count++;
                printTriangleDetails(triangle);
            }
            Assert.IsTrue(count == 72);
        }

        [Test]
        public void WhenGetF12ThenShouldReturnF12Details()
        {
            var triangleDetails = trianglesController.Get("F12");
            Assert.IsNotNull(triangleDetails);
            printTriangleDetails(triangleDetails);
            Assert.IsTrue(triangleDetails.IsValid);
            Assert.IsTrue(triangleDetails.TriangleKey == "F12");
            // F12 { X = 60,Y = 50} { X = 50,Y = 50}  { X = 60,Y = 60}
            Assert.IsTrue(triangleDetails.Vertex1.X == 60 && triangleDetails.Vertex1.Y == 50);
            Assert.IsTrue(triangleDetails.Vertex2.X == 50 && triangleDetails.Vertex2.Y == 50);
            Assert.IsTrue(triangleDetails.Vertex3.X == 60 && triangleDetails.Vertex3.Y == 60);
        }

        [Test]
        public void WhenReverseSearchForF12TriangleKeyThenShouldReturnF12()
        {
            var triangleDetails = trianglesController.Get("50,50", "60,50", "60,60");
            Assert.IsNotNull(triangleDetails);
            printTriangleDetails(triangleDetails);
            Assert.IsTrue(triangleDetails.IsValid);
            Assert.IsTrue(triangleDetails.TriangleKey == "F12");
        }

        private void printTriangleDetails(TriangleDetails triangleDetails)
        {
            Console.WriteLine($"key={triangleDetails.TriangleKey}, vertex1={triangleDetails.Vertex1}, vertex2={triangleDetails.Vertex2}, vertex3={triangleDetails.Vertex3}");
        }
    }
}