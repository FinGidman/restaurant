using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;
using Restaurant.Models.Data;
using Restaurant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    public class BasketController : Controller
    {
        private ApplicationContext _context;

        public BasketController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            decimal sum = cart.Sum(i => i.Count * i.Dish.Price);
            ViewBag.Sum = sum;
            return View(cart);
        }

        public IActionResult Add(int id)
        {
            Dish dish = _context.Dishes.FirstOrDefault(a => a.Id == id);
            if (dish != null)
            {
                if(SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
                {
                    List<Item> cart = new List<Item>();
                    cart.Add(new Item { Dish = dish, Count = 1 });
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                else
                {
                    List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                    Item item = cart
                            .Where(a => a.Dish.Id == dish.Id)
                            .FirstOrDefault();
                    if (item != null)
                    {
                        cart[cart.IndexOf(item)].Count++;
                    }
                    else
                    {
                        cart.Add(new Item { Dish = dish, Count = 1 });
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
            }
            return RedirectToAction("Menu", "Home");
        }


        //delat tut
        public IActionResult Remove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            Item item = cart.FirstOrDefault(a => a.Dish.Id == id);
            if(item != null)
            {
                if(item.Count == 1)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Count--;
                }
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index","Basket");
        }
    }
}
