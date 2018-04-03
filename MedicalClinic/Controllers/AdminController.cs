/* Michał Dyrcz Base #1
    1. Zamiana ApplicationUser na User
    2. Przy stworzniu obiektu user zamiana user.username na user.LastName
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalClinic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinic.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<User> userManager;

        public AdminController(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.LastName = model.UserName;
                user.Email = model.Email;

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View();
        }
    }
}
