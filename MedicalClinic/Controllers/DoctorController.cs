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

        [HttpGet]
        public async Task<IActionResult> Calendar()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var visits = _context.ApplicationUser
                            .Where(d => d.Id == user.Id)
                            .Join(
                                _context.DoctorModel,
                                applicationUser => applicationUser.Id,
                                doctor => doctor.UserId,
                                (applicationUser, doctor) =>  new CalendarViewModel
                                {
                                    Id = doctor.Id,
                                    FirstName = applicationUser.FirstName,
                                    LastName = applicationUser.LastName,
                                    Specialization = doctor.Specialization,
                                    DateOfApp = new List<string>(),
                                    Visits = new List<VisitsViewModel>()
                                }
                            )
                            .Single();

            visits.DateOfApp.Clear();

            var dates = _context.DoctorModel
                        .Where(d => d.UserId == user.Id)
                        .Join(
                            _context.AppointmentModel,
                            doctor => doctor.Id,
                            app => app.DoctorId,
                            (doctor, app) => new { doctor, app }
                        )
                        .ToList();

            foreach(var date in dates)
            {
                visits.DateOfApp.Add(date.app.DateOfApp);
            }

            return View(visits);
        }

        [HttpPost]
        public async Task<IActionResult> Calendar(CalendarViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var visits = _context.ApplicationUser
                            .Where(d => d.Id == user.Id)
                            .Join(
                                _context.DoctorModel,
                                applicationUser => applicationUser.Id,
                                doctor => doctor.UserId,
                                (applicationUser, doctor) => new CalendarViewModel
                                {
                                    Id = doctor.Id,
                                    FirstName = applicationUser.FirstName,
                                    LastName = applicationUser.LastName,
                                    Specialization = doctor.Specialization,
                                    DateOfApp = new List<string>(),
                                    Visits = new List<VisitsViewModel>()
                                }
                            )
                            .Single();

            visits.DateOfApp.Clear();

            var dates = _context.DoctorModel
                        .Where(d => d.UserId == user.Id)
                        .Join(
                            _context.AppointmentModel,
                            doctor => doctor.Id,
                            app => app.DoctorId,
                            (doctor, app) => new { doctor, app }
                        )
                        .ToList();

            foreach (var date in dates)
            {
                visits.DateOfApp.Add(date.app.DateOfApp);
            }

            visits.Visits.Clear();

            var userVisits = _context.AppointmentModel
                            .Where(d => (d.DateOfApp == model.SelectedDate) && (d.DoctorId == model.Id))
                            .Join(
                                _context.PatientCardModel,
                                docVisit => docVisit.PatientCardId,
                                card => card.Id,
                                (docVisit, card) => new { docVisit, card }
                            )
                            .Join(
                                _context.PatientModel,
                                docVisitCard => docVisitCard.card.PatientId,
                                patient => patient.Id,
                                (docVisitCard, patient) => new { docVisitCard, patient }
                            )
                            .Join(
                                _context.ApplicationUser,
                                cardVisitDoctorPatient => cardVisitDoctorPatient.patient.UserId,
                                applicationUser => applicationUser.Id,
                                (cardVisitDoctorPatient, applicationUser) => new VisitsViewModel
                                {
                                    Id = cardVisitDoctorPatient.docVisitCard.docVisit.Id,
                                    DateOfApp = cardVisitDoctorPatient.docVisitCard.docVisit.DateOfApp,
                                    PatientFirstName = applicationUser.FirstName,
                                    PatientLastName = applicationUser.LastName
                                }
                            )
                            .ToList();
            
            visits.Visits = userVisits;

            return View(visits);
        }
    }
}