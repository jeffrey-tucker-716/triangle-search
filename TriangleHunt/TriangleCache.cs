using System;
using System.Collections.Generic;
using System.Text;
using TriangleHunt.Models;

namespace TriangleHunt
{
    public sealed class TriangleCache
    {
        private static readonly TriangleCache instance;
        private static Dictionary<string, TriangleDetails> AllGridTriangles = new Dictionary<string, TriangleDetails>();

        private TriangleCache() {
            var resolver = new TriangleResolver();
            var keys = resolver.AllTriangleKeys();
            foreach(var key in keys)
            {
                var triangleDetails = resolver.ResolveTriangleKey(key);
                AllGridTriangles.Add(key, triangleDetails);
            }
        }

        static TriangleCache()
        {
            instance = new TriangleCache();
        }

        public static TriangleCache Instance
        {
            get
            {
                return instance;
            }
        }

        public TriangleDetails Lookup(string key)
        {
            TriangleDetails found = null;
            AllGridTriangles.TryGetValue(key, out found);
            return found;
        }

        
        public IEnumerable<TriangleDetails> AllTriangleDetails()
        {
            return AllGridTriangles.Values;
        }
    }
}
