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

namespace MedicalClinic.Controllers
{
    public class MessageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;

        public MessageController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                throw new Exception("Error: Current user is null!");
            }

            var temp = _context.MessageModel.Where(u =>
                u.ReceiverEmail == currentUser.Email ||
                u.SenderEmail == currentUser.Email
                ).ToList().OrderBy(s => s.Date);

            return View(temp);
        }

        [HttpGet, ActionName("SendPrivateMessage")]
        public async Task<IActionResult> SendPrivateMessageAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                throw new Exception("Error: Current user is null!");
            }

            MessageViewModel model = new MessageViewModel
            {
                Content = "Miejsce na twoja wiadomość...",
                ReceiversEmail = new List<SelectListItem>(),
            };


            foreach (var user in _context.Users)
            {
                if (user.Email != currentUser.Email)
                {
                    model.ReceiversEmail.Add(
                        new SelectListItem { Text = user.Email, Value = user.Email }
                    );
                }
            }

            return View(model);
        }

        [HttpPost, ActionName("SendPrivateMessage")]
        public async Task<IActionResult> SendPrivateMessageAsync(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser == null)
                {
                    throw new Exception("Error: Current user is null!");
                }

                MessageModel message = new MessageModel
                {
                    Date = DateTime.Now,
                    Content = model.Content,
                    ReceiverEmail = model.ReceiverEmail,
                    SenderEmail = currentUser.Email
                };

                await _context.MessageModel.AddAsync(message);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}