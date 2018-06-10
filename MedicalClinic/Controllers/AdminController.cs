using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalClinic.Data.Migrations;
using MedicalClinic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditUser()
        {
            return View(_context.Users.Where(u => u.IsActive).ToList());
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if(ModelState.IsValid)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            return View(user);
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
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);

            user.IsActive = false;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();
            return RedirectToAction("EditUser");
        }
    }
}