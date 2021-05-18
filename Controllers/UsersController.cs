using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Models.Data;
using Restaurant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationContext _context;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var userRole = await _userManager.GetRolesAsync(user);
                  
                return View(_userManager.Users.ToList());          
        }

        ///////////////////////////////////////////

        public IActionResult OrderedDishesPartial()
        {
            var orderedDishes = _context.DishOrders.Where(t => t.UserId == _userManager.GetUserId(User)).OrderByDescending(t => t.Active);
            return PartialView("_OrderedDishes",orderedDishes);
        }

        public IActionResult OrderedTablesPartial()
        {
            var orderedTables = _context.TableOrders.Where(t => t.UserId == _userManager.GetUserId(User)).OrderByDescending(t => t.Active);
            return PartialView("_OrderedTables",orderedTables);
        }

        public IActionResult CloseDishOrder(int id)
        {
            var order = _context.DishOrders.Find(id);
            order.Active = false;
            _context.DishOrders.Update(order);
            _context.SaveChanges();
            var orderedDishes = _context.DishOrders.Where(t => t.UserId == _userManager.GetUserId(User)).OrderByDescending(t => t.Active);
            return PartialView("_OrderedDishes", orderedDishes);
        }

        public IActionResult CloseTableOrder(int id)
        {
            var order = _context.TableOrders.Find(id);
            order.Active = false;
            _context.TableOrders.Update(order);
            _context.SaveChanges();
            var orderedTables = _context.TableOrders.Where(t => t.UserId == _userManager.GetUserId(User)).OrderByDescending(t => t.Active);
            return PartialView("_OrderedTables", orderedTables);
        }

        ///////////////////////////////////////////

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, PersonName = model.PersonName, PersonSurname = model.PersonSurname };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, PersonName = user.PersonName, PersonSurname = user.PersonSurname };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.PersonName = model.PersonName;
                    user.PersonSurname = model.PersonSurname;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
    }
}
