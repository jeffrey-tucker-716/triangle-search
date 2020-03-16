using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TriangleHunt;
using TriangleHunt.Interfaces;
using TriangleHunt.Models;
using System.Drawing;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TriangleSearchWebapp.Controllers
{
    [Route("api/[controller]")]
    public class TrianglesController : Controller
    {
        readonly ITriangleResolver _triangleResolver = null;

        public TrianglesController(ITriangleResolver triangleResolver)
        {
            _triangleResolver = triangleResolver;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<TriangleDetails> Get()
        {
            return _triangleResolver.GetAll();
        }

        /// <summary>
        /// This variant is only for reverse lookup of the triangle key.
        /// All the vertices should be in the form "0,10"
        /// This is to allow for caching of the results (perfectly RESTful architecture)
        /// Sample route: /triangles?v1=0,10&v2=10,10&v3=0,0
        /// https://stackoverflow.com/questions/207477/restful-url-design-for-search
        /// </summary>
        /// <param name="v1">first vertex in (X,Y) point format (e.g.): "0,0" </param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns>TriangleDetails with key filled in</returns>
        [HttpGet]
        public TriangleDetails Get(string v1, string v2, string v3)
        {
            TriangleDetails triangleDetails = new TriangleDetails();

            try
            {
                Point localPoint;
                if (GetPointFromVertexString(v1, out localPoint))
                {
                    triangleDetails.Vertex1 = localPoint;
                }
                else
                {
                    throw new ArgumentException($"Could not parse vertex1 from querystring ${v1}");
                }
                if (GetPointFromVertexString(v2, out localPoint))
                {
                    triangleDetails.Vertex2 = localPoint;
                }
                else
                {
                    throw new ArgumentException($"Could not parse vertex2 from querystring ${v2}");
                }
                if (GetPointFromVertexString(v3, out localPoint))
                {
                    triangleDetails.Vertex3 = localPoint;
                }
                else
                {
                    throw new ArgumentException($"Could not parse vertex3 from querystring ${v2}");
                }
                if (!_triangleResolver.GetTriangleKeyFromVertices(triangleDetails))
                {
                    // log it at least.  the TriangleDetails already says it is invalid
                }
            }
            catch { }
            
            return triangleDetails;
        }

        private bool GetPointFromVertexString(string vertex, out Point point)
        {
            bool foundCoordinates = false;
            point = Point.Empty;
            var coordinates = StringToIntList(vertex);
            try
            {
                if (coordinates.Count() == 2)
                {
                    point.X = coordinates.First<int>();
                    point.Y = coordinates.ElementAt<int>(1);
                    foundCoordinates = true;
                }
            }
            catch { }   // catch a FormatException if the vertex string is like:  "10,"
            return foundCoordinates;
        }
        private IEnumerable<int> StringToIntList(string str)
        {
            return (str ?? "").Split(',').Select<string, int>(int.Parse);
        }

        // GET api/<controller>/A1
        [HttpGet("{id}")]
        public TriangleDetails Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            var triangleDetails = _triangleResolver.GetOne(id);
            return triangleDetails;
        }

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
