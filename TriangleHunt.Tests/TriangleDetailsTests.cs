using NUnit.Framework;
using System;

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
        public void WhenGiveA0KeyThenReturnsNullTriangleDetails()
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
        public void WhenGivenD5KeyThenReturnsLastTriangleDetails()
        {

            var triangleDetails = triangleResolver.ResolveTriangleKey("D5");
            Assert.IsTrue(triangleDetails.IsValid);
            Assert.IsTrue(triangleDetails.TriangleKey == "D5");
            Console.WriteLine($"{triangleDetails.TriangleKey} {triangleDetails.Vertex1} {triangleDetails.Vertex2} {triangleDetails.Vertex3}");

            Assert.IsTrue(triangleDetails.Vertex1.X == 20 && triangleDetails.Vertex1.Y == 40);
            //Assert.IsTrue(triangleDetails.Vertex2.X == 0 && triangleDetails.Vertex2.Y == 0);
            //Assert.IsTrue(triangleDetails.Vertex3.X == 10 && triangleDetails.Vertex3.Y == 10);
        }
    }
}