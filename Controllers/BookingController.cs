using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Models;
using Restaurant.Models.Data;
using Restaurant.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    public class BookingController : Controller
    {
        private ApplicationContext _context;
        private readonly UserManager<User> _user;

        public BookingController(ApplicationContext context, UserManager<User> user)
        {
            _context = context;
            _user = user;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Step1()
        {
            return PartialView("_Step1");
        }

        [HttpPost]
        public IActionResult Step1(Step1ViewModel model)
        {
            return RedirectToAction("Step2", model);
        }

        /////////////////////////////////////////////

        [HttpGet]
        public IActionResult Step2(Step1ViewModel model)
        {
            ViewData["Date"] = model.Date;
            ViewData["Persons"] = model.Persons;
            var freeTables = _context.Tables.Where(d => d.Seats >= model.Persons);
            var orderedTables = _context.TableOrders
                .Where(d =>
                d.OrderTime.Day == model.Date.Day &&
                d.OrderTime.Month == model.Date.Month &&
                d.OrderTime.Year == model.Date.Year &&
                d.Active == true
                ).Select(d => d.TableId);
            if (orderedTables != null)
            {
                var tables = freeTables.Where(x => !orderedTables.Any(b => b == x.Id));
                return PartialView("_Step2",tables);
            }
            else
                return PartialView("_Step2",freeTables);
        }

        //[HttpPost]
        //public IActionResult Step2(int id, Step1ViewModel model)
        //{            
        //    return RedirectToAction("Step3",new { id, model });
        //}

        /////////////////////////////////////////////

        [HttpGet]
        public IActionResult Step3(int id, string date, int persons)
        {
            //string dt = date.ToString("dd-MM-yyyy");
            return PartialView("_Step3",new Step3ViewModel { TableId = id, Date = date, Persons = persons });
        }

        [HttpPost]
        public IActionResult Step3(Step3ViewModel model)
        {
            return RedirectToAction("Order", model);
        }

        /////////////////////////////////////////////

        [Authorize]
        public IActionResult Order(Step3ViewModel model)
        {
            DateTime orderTime = Convert.ToDateTime(model.Date);
            TimeSpan ts = new TimeSpan(model.Hours, model.Minutes, 00);
            orderTime = orderTime + ts;

            if(model.Comment == null)
            {
                model.Comment = "";
            }
            if (_user.GetUserId(User) != null)
            {
                TableOrder to = new TableOrder
                {
                    TableId = model.TableId,
                    UserId = _user.GetUserId(User),
                    OrderTime = orderTime,
                    Active = true,
                    Persons = model.Persons,
                    Comment = model.Comment
                };

                if (model.Checkbox == true)
                {
                    string orderList = SessionHelper.GetObjectInString(HttpContext.Session, "cart");
                    if (orderList != null)
                    {
                        DishOrder dishOrder = new DishOrder
                        {
                            UserId = _user.GetUserId(User),
                            TableId = model.TableId,
                            Active = true,
                            OrderType = "in",
                            OrderTime = orderTime,
                            Comment = "",
                            OrderListJson = orderList
                        };
                        _context.Entry(dishOrder).State = EntityState.Added;
                        _context.SaveChanges();
                    }
                    _context.Entry(to).State = EntityState.Added;
                    _context.SaveChanges();
                    
                    SessionHelper.ClearObject(HttpContext.Session, "cart");
                }
                else
                {
                    _context.Entry(to).State = EntityState.Added;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Menu", "Home");
        }
            
    
    }
}
