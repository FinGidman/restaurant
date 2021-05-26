using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    public class EmployeeController : Controller
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationContext _context;

        public EmployeeController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult OrderedDishesPartial()
        {
            var orderedDishes = _context.DishOrders.OrderByDescending(t => t.Active);
            return PartialView("_OrderedDishes", orderedDishes);
        }

        public IActionResult OrderedTablesPartial()
        {
            var orderedTables = _context.TableOrders.OrderByDescending(t => t.Active);
            return PartialView("_OrderedTables", orderedTables);
        }

        public IActionResult CloseDishOrder(int id)
        {
            var order = _context.DishOrders.Find(id);
            order.Active = false;
            _context.DishOrders.Update(order);
            _context.SaveChanges();
            var orderedDishes = _context.DishOrders.OrderByDescending(t => t.Active);
            return PartialView("_OrderedDishes", orderedDishes);
        }

        public IActionResult CloseTableOrder(int id)
        {
            var order = _context.TableOrders.Find(id);
            order.Active = false;
            _context.TableOrders.Update(order);
            _context.SaveChanges();
            var orderedTables = _context.TableOrders.OrderByDescending(t => t.Active);
            return PartialView("_OrderedTables", orderedTables);
        }

        public IActionResult SortOrderedDishes(string type)
        {
            switch (type)
            {
                case "future":
                    {
                        var list = _context.DishOrders.Where(x =>
                        DateTime.Compare(x.OrderTime, DateTime.Now) > 0);
                        return PartialView("_OrderedDishes", list);
                    }
                case "today":
                    {
                        var list = _context.DishOrders.Where(x =>
                        DateTime.Compare(x.OrderTime.Date, DateTime.Now.Date) == 0);
                        return PartialView("_OrderedDishes", list);
                    }
                case "active":
                    {
                        var list = _context.DishOrders.Where(x => x.Active == true);
                        return PartialView("_OrderedDishes", list);
                    }
                case "closed":
                    {
                        var list = _context.DishOrders.Where(x => x.Active == false);
                        return PartialView("_OrderedDishes", list);
                    }
                default:
                    {
                        return RedirectToAction("OrderedDishesPartial");
                    }
            }
        }

        public IActionResult SortOrderedTables(string type)
        {
            switch (type)
            {
                case "future":
                    {
                        var list = _context.TableOrders.Where(x =>
                        DateTime.Compare(x.OrderTime, DateTime.Now) > 0);
                        return PartialView("_OrderedTables", list);
                    }
                case "today":
                    {
                        var list = _context.TableOrders.Where(x =>
                        DateTime.Compare(x.OrderTime.Date, DateTime.Now.Date) == 0);
                        return PartialView("_OrderedTables", list);
                    }
                case "active":
                    {
                        var list = _context.TableOrders.Where(x => x.Active == true);
                        return PartialView("_OrderedTables", list);
                    }
                case "closed":
                    {
                        var list = _context.TableOrders.Where(x => x.Active == false);
                        return PartialView("_OrderedTables", list);
                    }
                default:
                    {
                        return RedirectToAction("OrderedTablesPartial");
                    }
            }
        }
    }
}
