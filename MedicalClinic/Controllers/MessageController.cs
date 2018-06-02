using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalClinic.Data.Migrations;
using MedicalClinic.Models;
using MedicalClinic.Models.MessageViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace MedicalClinic.Controllers
{
    public class MessageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private ApplicationDbContext _context;

        public MessageController(
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
            return View();
        }

        [HttpGet, ActionName("SendMessage")]
        public async Task<IActionResult> SendMessageAsync()
        {
            var currnetUser = await _userManager.GetUserAsync(User);
            var receivers = _context.Users.Where(u => u.Id == currnetUser.Id).ToList();
            if (currnetUser == null)
            {
                return View();
            }

            var model = new SendMessageViewModel
            {
                Content = "...",
                Receivers = receivers.Select(u => new SelectListItem() { Text = u.Email, Value = u.Email }),
                Sender = currnetUser.Email
            };

            return View(model);
        }

        [HttpGet, ActionName("SendMessage")]
        public IActionResult SendMessage(SendMessageViewModel model)
        {
            return View(model);
        }
    }
}