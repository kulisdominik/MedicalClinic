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
                (u.ReceiverEmail == currentUser.Email && u.ReceiverVisibility) ||
                (u.SenderEmail == currentUser.Email && u.SenderVisibility)
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
                //Content = "Miejsce na twoja wiadomość...",
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

        [HttpGet, ActionName("SendMessageToAllClerk")]
        public IActionResult SendMessageToAllClerk()
        {
            GuestMessageViewModel guestMessage = new GuestMessageViewModel
            {
                //Content = "Wiadomość...",
                //SenderEmail = "guest@MedicalClinic.com"
            };

            return View(guestMessage);
        }

        [HttpPost, ActionName("SendMessageToAllClerk")]
        public async Task<IActionResult> SendMessageToAllClerkAsync(GuestMessageViewModel model)
        {
            if(ModelState.IsValid)
            {
                var clerks = await _userManager.GetUsersInRoleAsync("Clerk");

                if(clerks == null)
                {
                    return View();
                }

                foreach(var clerk in clerks)
                {
                    MessageModel message = new MessageModel
                    {
                        Date = DateTime.Now,
                        Content = model.Content,
                        ReceiverEmail = clerk.Email,
                        SenderEmail = model.SenderEmail
                    };
                    // TODO: Make this method real async 
                    await _context.MessageModel.AddAsync(message);
                }
                await _context.SaveChangesAsync();

                return RedirectToRoute("default");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var message = _context.MessageModel.SingleOrDefault(u => u.Id == id);
            return View(message);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            MessageModel message = _context.MessageModel.SingleOrDefault(m => m.Id == id);

            ApplicationUser currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                throw new Exception("Error: Current user is null!");
            }

            if(currentUser.Email == message.ReceiverEmail)
            {
                message.ReceiverVisibility = false;
            }
            else
            {
                message.SenderVisibility = false;
            }

            _context.Update(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}