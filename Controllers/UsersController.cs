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
            var userRoles = await _userManager.GetRolesAsync(user);

            var users = await _userManager.GetUsersInRoleAsync("user");
            var employees = await _userManager.GetUsersInRoleAsync("employee");
            users = users.Concat(employees).ToList();

            return View(users);          
        }

        ///////////////////////////////////////////

        public IActionResult OrderedDishesPartial()
        {
            if (User.IsInRole("user"))
            {
                var orderedDishes = _context.DishOrders
                    .Where(t => t.UserId == _userManager.GetUserId(User))
                    .OrderByDescending(t => t.Active)
                    .OrderByDescending(t => t.OrderTime);
                return PartialView("_OrderedDishes", orderedDishes);
            }
            else
            {
                var orderedDishes = _context.DishOrders
                    .OrderByDescending(t => t.Active)
                    .OrderByDescending(t => t.OrderTime);
                return PartialView("_OrderedDishes", orderedDishes);
            }
        }

        public IActionResult OrderedTablesPartial()
        {
            if (User.IsInRole("user"))
            {
                var orderedTables = _context.TableOrders
                    .Where(t => t.UserId == _userManager.GetUserId(User))
                    .OrderByDescending(t => t.Active)
                    .OrderByDescending(t => t.OrderTime);
                return PartialView("_OrderedTables", orderedTables);
            }
            else
            {
                var orderedTables = _context.TableOrders
                    .OrderByDescending(t => t.Active)
                    .OrderByDescending(t => t.OrderTime);
                return PartialView("_OrderedTables", orderedTables);
            }
        }

        public IActionResult CloseDishOrder(int id)
        {
            var order = _context.DishOrders.Find(id);
            order.Active = false;
            _context.DishOrders.Update(order);
            _context.SaveChanges();
            if (User.IsInRole("user"))
            {
                var orderedDishes = _context.DishOrders
                    .Where(t => t.UserId == _userManager.GetUserId(User))
                    .OrderByDescending(t => t.Active)
                    .OrderByDescending(t => t.OrderTime);
                return PartialView("_OrderedDishes", orderedDishes);
            }
            else
            {
                var orderedDishes = _context.DishOrders
                    .OrderByDescending(t => t.Active)
                    .OrderByDescending(t => t.OrderTime);
                return PartialView("_OrderedDishes", orderedDishes);
            }
        }

        public IActionResult CloseTableOrder(int id)
        {
            var order = _context.TableOrders.Find(id);
            order.Active = false;
            _context.TableOrders.Update(order);
            _context.SaveChanges();

            if (order.TableId != 0)
            {
                var dishes = _context.DishOrders
                    .Where(x => x.TableId == order.TableId
                    && x.UserId == _userManager.GetUserId(User)
                    && x.OrderTime == order.OrderTime);

                if (dishes != null)
                {
                    foreach (var d in dishes)
                    {
                        d.Active = false;
                        _context.DishOrders.Update(d);                       
                    }
                    _context.SaveChanges();
                }
            }
            if (User.IsInRole("user"))
            {
                var orderedTables = _context.TableOrders
                    .Where(t => t.UserId == _userManager.GetUserId(User))
                    .OrderByDescending(t => t.Active)
                    .OrderByDescending(t => t.OrderTime);
                return PartialView("_OrderedTables", orderedTables);
            }
            else
            {
                var orderedTables = _context.TableOrders
                    .OrderByDescending(t => t.Active)
                    .OrderByDescending(t => t.OrderTime);
                return PartialView("_OrderedTables", orderedTables);
            }
        }

        public IActionResult SortOrderedDishes(string type)
        {
            switch (type)
            {
                case "future":
                    {
                        if (User.IsInRole("user"))
                        {
                            var list = _context.DishOrders
                                .Where(x => DateTime.Compare(x.OrderTime, DateTime.Now) > 0
                                && x.UserId == _userManager.GetUserId(User))
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedDishes", list);
                        }
                        else
                        {
                            var list = _context.DishOrders
                                .Where(x => DateTime.Compare(x.OrderTime, DateTime.Now) > 0)
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedDishes", list);
                        }
                    }
                case "today":
                    {
                        if (User.IsInRole("user"))
                        {
                            var list = _context.DishOrders
                                .Where(x => DateTime.Compare(x.OrderTime.Date, DateTime.Now.Date) == 0
                                && x.UserId == _userManager.GetUserId(User))
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedDishes", list);
                        }
                        else
                        {
                            var list = _context.DishOrders
                                .Where(x => DateTime.Compare(x.OrderTime.Date, DateTime.Now.Date) == 0)
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedDishes", list);
                        }
                    }
                case "active": 
                    {
                        if (User.IsInRole("user"))
                        {
                            var list = _context.DishOrders
                                .Where(x => x.Active == true && x.UserId == _userManager.GetUserId(User))
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedDishes", list);
                        }
                        else
                        {
                            var list = _context.DishOrders
                                .Where(x => x.Active == true)
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedDishes", list);
                        }
                    }
                case "closed":
                    {
                        if (User.IsInRole("user"))
                        {
                            var list = _context.DishOrders
                                .Where(x => x.Active == false 
                                && x.UserId == _userManager.GetUserId(User))
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedDishes", list);
                        }
                        else
                        {
                            var list = _context.DishOrders
                                .Where(x => x.Active == false)
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedDishes", list);
                        }
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
                        if (User.IsInRole("user"))
                        {
                            var list = _context.TableOrders
                                .Where(x => DateTime.Compare(x.OrderTime, DateTime.Now) > 0 
                                && x.UserId == _userManager.GetUserId(User))
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedTables", list);
                        }
                        else
                        {
                            var list = _context.TableOrders
                                .Where(x => DateTime.Compare(x.OrderTime, DateTime.Now) > 0)
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedTables", list);
                        }
                    }
                case "today":
                    {
                        if (User.IsInRole("user"))
                        {
                            var list = _context.TableOrders
                                .Where(x => DateTime.Compare(x.OrderTime.Date, DateTime.Now.Date) == 0 
                                && x.UserId == _userManager.GetUserId(User))
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedTables", list);
                        }
                        else
                        {
                            var list = _context.TableOrders
                                .Where(x => DateTime.Compare(x.OrderTime.Date, DateTime.Now.Date) == 0)
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedTables", list);
                        }
                    }
                case "active":
                    {
                        if (User.IsInRole("user"))
                        {
                            var list = _context.TableOrders
                                .Where(x => x.Active == true 
                                && x.UserId == _userManager.GetUserId(User))
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedTables", list);
                        }
                        else
                        {
                            var list = _context.TableOrders
                                .Where(x => x.Active == true)
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedTables", list);
                        }
                    }
                case "closed":
                    {
                        if (User.IsInRole("user"))
                        {
                            var list = _context.TableOrders
                                .Where(x => x.Active == false 
                                && x.UserId == _userManager.GetUserId(User))
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedTables", list);
                        }
                        else
                        {
                            var list = _context.TableOrders
                                .Where(x => x.Active == false)
                                .OrderByDescending(t => t.OrderTime);
                            return PartialView("_OrderedTables", list);
                        }
                    }
                default:
                    {
                        return RedirectToAction("OrderedTablesPartial");
                    }
            }
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

        public async Task<IActionResult> EditFromUserProfile()
        {
            string id = _userManager.GetUserId(User);
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, PersonName = user.PersonName, PersonSurname = user.PersonSurname };
            return View("Edit",model);
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

        ///////////////////////////////////////////////////////////////////////////

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

        public async Task<IActionResult> ChangePasswordFromUserProfile()
        {
            string id = _userManager.GetUserId(User);
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View("ChangePassword",model);
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

        /////////////////////////////////////////////////////////

        public IActionResult Dishes()
        {
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
            Dish dish = new Dish
            {
                Name = model.Name,
                Category = model.Category,
                Price = model.Price,
                Description = model.Description,
                Photo = model.Photo
            };
            return RedirectToAction("Dishes");
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
                Photo = dish.Photo
            };
            return View(new { model });
        }

        [HttpPost]
        public IActionResult EditDish(DishViewModel model)
        {
            Dish dish = new Dish
            {
                Name = model.Name,
                Category = model.Category,
                Price = model.Price,
                Description = model.Description,
                Photo = model.Photo
            };
            return RedirectToAction("Dishes");
        }

        public IActionResult DeleteDish(int id)
        {
            Dish dish = _context.Dishes.Find(id);
            _context.Dishes.Remove(dish);
            return View();
        }

    }
}
