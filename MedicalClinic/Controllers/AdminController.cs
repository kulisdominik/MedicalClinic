using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalClinic.Data.Migrations;
using MedicalClinic.Models;
using MedicalClinic.Models.AdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private ApplicationDbContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

        [HttpGet, ActionName("Edit")]
        public async Task<IActionResult> EditAsync(string id)
        {
            EditViewModel model = new EditViewModel();
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            model.ApplicationRoles = _roleManager.Roles.Select(role => new SelectListItem
            {
                Text = role.Name,
                Value = role.Id
            }).ToList();

            if(user != null)
            {
                model.UserName = user.UserName;
                model.Email = user.Email;
                model.ApplicationRoleId = _roleManager.Roles.Single(role => role.Name == _userManager.GetRolesAsync(user).Result.Single()).Id;
            }

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(string id, EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            
                if(user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;

                    string roles = _userManager.GetRolesAsync(user).Result.Single();
                    string userRoleId = _roleManager.Roles.Single(r => r.Name == roles).Id;

                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if(result.Succeeded)
                    {
                        if(userRoleId != model.ApplicationRoleId)
                        {
                            IdentityResult roleResult = await _userManager.RemoveFromRoleAsync(user, roles);

                            if(roleResult.Succeeded)
                            {
                                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                                if (applicationRole != null)
                                {
                                    IdentityResult newRoleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                                    if (newRoleResult.Succeeded)
                                    {
                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return View(model);
        }

        public IActionResult Details(string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            return View(user);
        }


        [HttpGet]
        public IActionResult Delete(string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var movie = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            _context.Users.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction("EditUser");
        }
    }
}