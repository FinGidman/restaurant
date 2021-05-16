using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public BookingController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Step1(string returnUrl = null)
        {
            return View(new Step1ViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public IActionResult Step1(Step1ViewModel model)
        {
            return RedirectToAction("Step2","Booking",model);
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
            if(orderedTables != null)
            {
                var tables = freeTables.Where(x => !orderedTables.Any(b => b == x.Id));
                return View(tables);
            }
            else
                return View(freeTables);   
        }

        //[HttpPost]
        //public IActionResult Step2(int id, Step1ViewModel model)
        //{            
        //    return RedirectToAction("Step3",new { id, model });
        //}

        /////////////////////////////////////////////

        [HttpGet]
        public IActionResult Step3(int id, DateTime date, int persons)
        {
            return View(new Step3ViewModel { TableId = id, Date = date, Persons = persons });
        }

        [HttpPost]
        public IActionResult Step3(Step3ViewModel model)
        {
            return RedirectToAction("Order", model);
        }

        /////////////////////////////////////////////

        public IActionResult Order(Step3ViewModel model)
        {
            return View("Menu", "Home");
        }
    }
}
