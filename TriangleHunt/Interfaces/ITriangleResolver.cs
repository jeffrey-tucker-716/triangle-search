using System;
using System.Collections.Generic;
using System.Text;
using TriangleHunt.Models;

namespace TriangleHunt.Interfaces
{
    public interface ITriangleResolver
    {
        /// <summary>
        /// Gets all the TriangleDetails there are in the 60x60 grid
        /// </summary>
        /// <returns></returns>
        TriangleDetails[] GetAll();

        /// <summary>
        /// Gets just the one based on the id from cache or calculates it.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TriangleDetails GetOne(string key);
       
        /// <summary>
        /// This routine calculates the details
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TriangleDetails ResolveTriangleKey(string key);

        /// <summary>
        /// Does a reverse lookup for the key based on vertices
        /// </summary>
        /// <param name="triangleDetails"></param>
        /// <returns></returns>
        bool GetTriangleKeyFromVertices(TriangleDetails triangleDetails);
    }
}
