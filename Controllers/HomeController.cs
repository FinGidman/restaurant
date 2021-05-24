using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant.Models;
using Restaurant.Models.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult  Menu() =>  View(_context.Dishes.ToList()); 

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult CircleBtn()
        {
            var value = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (value != null)
            {
                int count = 0;
                for (int i = 0; i < value.Count; i++)
                {
                    count += value.ElementAt(i).Count;
                }
                ViewData["Count"] = count;
            }
            else
                ViewData["Count"] = 0;
            return PartialView("_CircleBtn");
        }

        public IActionResult Sort(string category)
        {            
            return View("Menu",_context.Dishes.Where(x => x.Category == category).ToList());
        }
    }
}
