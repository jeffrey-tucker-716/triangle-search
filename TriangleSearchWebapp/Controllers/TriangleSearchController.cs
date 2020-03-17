using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TriangleHunt.Interfaces;
using TriangleSearchWebapp.Models;

namespace TriangleSearchWebapp.Controllers
{
    public class TriangleSearchController : Controller
    {
        readonly ITriangleResolver _triangleResolver;
        TriangleSearchViewModel triangleSearchViewModel = new TriangleSearchViewModel();
        public TriangleSearchController(ITriangleResolver triangleResolver)
        {
            // get list of Triangle keys
            _triangleResolver = triangleResolver;
            // set in the view model
            triangleSearchViewModel.TriangleKeys = _triangleResolver.AllTriangleKeys();
        }
        public IActionResult Index()
        {
            return View(triangleSearchViewModel);
        }
    }
}