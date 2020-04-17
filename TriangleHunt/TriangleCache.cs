using System;
using System.Collections.Generic;
using System.Text;
using TriangleHunt.Models;

namespace TriangleHunt
{
    public sealed class TriangleCache
    {
        private static volatile TriangleCache _instance;
        private static Dictionary<string, TriangleDetails> _allGridTriangles = new Dictionary<string, TriangleDetails>();
        private static readonly object _synchLock = new object();

        private TriangleCache() {
            var resolver = new TriangleResolver();
            var keys = resolver.AllTriangleKeys();
            foreach (var key in keys)
            {
                var triangleDetails = resolver.ResolveTriangleKey(key);
                _allGridTriangles.Add(key, triangleDetails);
            }
        }

        /// <summary>
        /// From classic singleton pattern thread safe with a lock
        /// </summary>
        public static TriangleCache Instance 
        {
            get
            {
                if (_instance != null) 
                    return _instance;
                lock(_synchLock)
                {
                    _instance = new TriangleCache();
                }
                return _instance;
            }
        }

        

        public TriangleDetails Lookup(string key)
        {
            TriangleDetails found = null;
            _allGridTriangles.TryGetValue(key, out found);
            return found;
        }

        
        public IEnumerable<TriangleDetails> AllTriangleDetails()
        {
            return _allGridTriangles.Values;
        }
    }
}
