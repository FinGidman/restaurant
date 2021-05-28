using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models.Data;
using Restaurant.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    public class InfrastructureController : Controller
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationContext _context;
        private IWebHostEnvironment _environment;

        public InfrastructureController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationContext context,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {

            //string contentPath = this._environment.ContentRootPath;
            return View(_context.Dishes.ToList());
        }

        [HttpGet]
        public IActionResult AddDish()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDish(DishViewModel model)
        {
            string extenstion = Path.GetExtension(model.Photo.FileName);
            string filename = Path.GetFileName(model.Photo.FileName);
            if (extenstion == ".jpg" || extenstion == ".png")
            {
                var path = Path.Combine(_environment.WebRootPath, "images", model.Photo.FileName);
                var stream = new FileStream(path, FileMode.Create);
                model.Photo.CopyTo(stream);

                if (model.Description == null)
                {
                    model.Description = "";
                }

                Dish dish = new Dish
                {
                    Name = model.Name,
                    Category = model.Category,
                    Price = model.Price,
                    Description = model.Description,
                    Photo = "../images/" + filename
                };
                _context.Entry(dish).State = EntityState.Added;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditDish(int id)
        {
            Dish dish = _context.Dishes.Find(id);
            DishViewModel model = new DishViewModel
            {
                Id = dish.Id,
                Name = dish.Name,
                Category = dish.Category,
                Price = dish.Price,
                Description = dish.Description,
                Photopath = dish.Photo
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditDish(DishViewModel model)
        {
            if (model.Photo != null)
            {
                string extenstion = Path.GetExtension(model.Photo.FileName);
                string filename = Path.GetFileName(model.Photo.FileName);
                var photopath = "../images/" + filename;
                if (extenstion == ".jpg" || extenstion == ".png")
                {
                    if (!_context.Dishes.Any(x => x.Photo.Contains(photopath)))
                    {
                        var path = Path.Combine(_environment.WebRootPath, "images", model.Photo.FileName);
                        var stream = new FileStream(path, FileMode.Create);
                        model.Photo.CopyTo(stream);
                    }

                }

                Dish dish = new Dish
                {
                    Id = model.Id,
                    Name = model.Name,
                    Category = model.Category,
                    Price = model.Price,
                    Description = model.Description,
                    Photo = photopath
                };
                _context.Entry(dish).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                Dish dish = new Dish
                {
                    Id = model.Id,
                    Name = model.Name,
                    Category = model.Category,
                    Price = model.Price,
                    Description = model.Description,
                    Photo = model.Photopath
                };
                _context.Entry(dish).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteDish(int id)
        {
            Dish dish = _context.Dishes.Find(id);
            _context.Dishes.Remove(dish);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
