using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedicalClinic.Models;
using MedicalClinic.Models.DoctorViewModels;
using MedicalClinic.Data.Migrations;
using Microsoft.AspNetCore.Identity;

namespace MedicalClinic.Controllers
{
    public class DoctorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;

        public DoctorController(
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

        public async Task<IActionResult> Calendar()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var visits = _context.ApplicationUser
                            .Join(
                                _context.DoctorModel,
                                applicationUser => applicationUser.Id,
                                doctor => doctor.UserId,
                                (applicationUser, doctor) => new { applicationUser, doctor }
                            )
                            .Where(d => d.applicationUser.Id == user.Id)
                            .Join(
                                _context.AppointmentModel,
                                doc => doc.doctor.Id,
                                app => app.DoctorId,
                                (doc, app) => new CalendarViewModel
                                {
                                    Id = app.Id,
                                    FirstName = doc.applicationUser.FirstName,
                                    LastName = doc.applicationUser.LastName,
                                    Specialization = doc.doctor.Specialization,
                                    DateOfApp = app.DateOfApp,
                                    Notes = app.Notes
                                }
                            )
                            .ToList();

            return View(visits);
        }
    }
}