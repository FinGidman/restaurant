using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<User> _user;

        public BasketController(ApplicationContext context, UserManager<User> user)
        {
            _context = context;
            _user = user;
        }

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if(cart != null)
            {
                ViewBag.Sum = cart.Sum(i => i.Count * i.Dish.Price);
                return View(cart);
            }
            else
            {
                ViewBag.Sum = 0;
                return View();
            }        
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

        [HttpGet]
        public IActionResult Order()
        {
            var tables = _context.TableOrders.Where(t => t.UserId == _user.GetUserId(User) && t.Active == true);
            if (tables != null)
            {
                return View(new OrderDishViewModel { OrderedTables = tables.ToList()});
            }
            else
            {
                return View(new OrderDishViewModel { OrderedTables = null });
            }
        }

        [HttpPost]
        public IActionResult Order(OrderDishViewModel model)
        {
            string orderList = SessionHelper.GetObjectInString(HttpContext.Session, "cart");
            if(model.Comment == null)
            {
                model.Comment = "";
            }
            if (model.Type == "In")
            {
                DishOrder dishOrder = new DishOrder
                {
                    UserId = _user.GetUserId(User),
                    TableId = model.TableId,
                    Active = true,
                    OrderType = "in",
                    OrderTime = DateTime.Now,
                    Comment = model.Comment,
                    OrderListJson = orderList
                };
                _context.Entry(dishOrder).State = EntityState.Added;
                _context.SaveChanges();
                //SessionHelper.ClearObject(HttpContext.Session, "cart");
            }
            else if(model.Type == "Out")
            {
                DishOrder dishOrder = new DishOrder
                {
                    UserId = _user.GetUserId(User),
                    TableId = 0,
                    Active = true,
                    OrderType = "out",
                    OrderTime = DateTime.Now,
                    Comment = model.Comment,
                    OrderListJson = orderList
                };
                _context.Entry(dishOrder).State = EntityState.Added;
                _context.SaveChanges();
                //SessionHelper.ClearObject(HttpContext.Session, "cart");
            }

            return RedirectToAction("Menu", "Home");
        }
    }
}
