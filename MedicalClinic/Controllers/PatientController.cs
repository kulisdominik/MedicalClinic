using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MedicalClinic.Data.Migrations;
using MedicalClinic.Models;
using MedicalClinic.Models.PatientViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinic.Controllers
{
    public class PatientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;

        public PatientController(
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

        public async Task<IActionResult> VisitRegistration()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var patientCard = _context.ApplicationUser
                            .Where(d => d.Id == user.Id)
                            .Join(
                                _context.PatientModel,
                                appUser => appUser.Id,
                                patient => patient.UserId,
                                (appUser, patient) => new { appUser, patient }
                            )
                            .Join(
                                _context.PatientCardModel,
                                appUserPat => appUserPat.patient.Id,
                                card => card.PatientId,
                                (appUserPat, card) => new { appUserPat, card }
                            )
                            .SingleOrDefault();

            var doctors = _context.ApplicationUser
                            .Join(
                                _context.DoctorModel,
                                appUser => appUser.Id,
                                doctor => doctor.UserId,
                                (appUser, doctor) => new { appUser, doctor }
                            )
                            .Join(
                                _context.WorkHours,
                                doc => doc.doctor.Id,
                                hours => hours.DoctorId,
                                (doc, hours) => new VisitRegistrationViewModel
                                {
                                    Id = doc.doctor.Id,
                                    Specialization = doc.doctor.Specialization,
                                    FirstName = doc.appUser.FirstName,
                                    LastName = doc.appUser.LastName,
                                    DayofWeek = hours.DayofWeek,
                                    StartHour = hours.StartHour,
                                    EndHour = hours.EndHour
                                }
                            )
                            .ToList();

            if(patientCard != null)
            {
                foreach(VisitRegistrationViewModel doctor in doctors)
                {
                    doctor.CardId = patientCard.card.Id;
                }
            }
            
            return View(doctors);
        }

        [HttpGet]
        public async Task<IActionResult> SelectDate(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var patientCard = _context.ApplicationUser
                            .Where(d => d.Id == user.Id)
                            .Join(
                                _context.PatientModel,
                                appUser => appUser.Id,
                                patient => patient.UserId,
                                (appUser, patient) => new { appUser, patient }
                            )
                            .Join(
                                _context.PatientCardModel,
                                appUserPat => appUserPat.patient.Id,
                                card => card.PatientId,
                                (appUserPat, card) => new { appUserPat, card }
                            )
                            .SingleOrDefault();

            var grades = _context.GradeModel
                        .Join(
                            _context.AppointmentModel,
                            gr => gr.AppointmentId,
                            app => app.Id,
                            (gr, app) => new { gr, app }
                        )
                        .Where(d => d.app.DoctorId == id)
                        .ToList();

            var doctorInfo = _context.ApplicationUser
                            .Join(
                                _context.DoctorModel,
                                appUser => appUser.Id,
                                doctor => doctor.UserId,
                                (appUser, doctor) => new { appUser, doctor }
                            )
                            .Where(d => d.doctor.Id == id)
                            .Join(
                                _context.WorkHours,
                                doc => doc.doctor.Id,
                                hours => hours.DoctorId,
                                (doc, hours) => new { doc, hours }
                            )
                            .Single();

            var visitRegistrationInfo = new VisitRegistrationViewModel
            {
                Id = doctorInfo.doc.doctor.Id,
                CardId = patientCard.card.Id,
                FirstName = doctorInfo.doc.appUser.FirstName,
                LastName = doctorInfo.doc.appUser.LastName,
                DayofWeek = doctorInfo.hours.DayofWeek,
                StartHour = doctorInfo.hours.StartHour,
                EndHour = doctorInfo.hours.EndHour,
                Grade = new List<GradeModel>()
            };

            visitRegistrationInfo.Grade.Clear();

            foreach (var gr in grades)
            {
                var grade = new GradeModel
                {
                    Grade = gr.gr.Grade,
                    Comment = gr.gr.Comment
                };

                visitRegistrationInfo.Grade.Add(grade);
            }

            return View(visitRegistrationInfo);
        }

        [HttpPost]
        public IActionResult SelectDate(VisitRegistrationViewModel model)
        {
            return RedirectToAction("SelectHour", "Patient", model);
        }

        [HttpGet]
        public IActionResult SelectHour(VisitRegistrationViewModel model)
        {
            var newVisitInfo = new NewVisitViewModel
            {
                Id = model.Id,
                CardId = model.CardId,
                Hours = new List<string>(),
                Grade = model.Grade
            };

            var visits = _context.AppointmentModel
                        .Where(d => (d.DoctorId == model.Id) && (d.DateOfApp == model.SelectedDate))
                        .Select(p => new { hour = p.Hour })
                        .ToList();

            newVisitInfo.Hours.Clear();

            string startHour = model.StartHour;
            string endHour = model.EndHour;

            startHour = startHour.Replace(":", string.Empty);
            endHour = endHour.Replace(":", string.Empty);

            int sHour = Int32.Parse(startHour);
            int eHour = Int32.Parse(endHour);

            int hour = sHour;

            while ((hour + 30) < eHour)
            {
                string newHour = hour.ToString().Insert(hour.ToString().Length - 2, ":");
                newVisitInfo.Hours.Add(newHour);
                
                foreach (var app in visits)
                {
                    if(app.hour == newHour)
                    {
                        newVisitInfo.Hours.Remove(newHour);
                    }
                }

                if ((hour + 30) % 100 >= 60)
                    hour = hour + 100 - 30;
                else
                    hour = hour + 30;
            }

            return View(newVisitInfo);
        }

        [HttpPost]
        public IActionResult SelectHour(NewVisitViewModel model)
        {
            var newVisit = new AppointmentModel
            {
                DateOfApp = model.SelectedDate,
                Hour = model.SelectedHour,
                IsConfirmed = 0,
                DoctorId = model.Id,
                PatientCardId = model.CardId
            };

            _context.AppointmentModel.Add(newVisit);
            _context.SaveChanges();

            return RedirectToAction(nameof(CancelVisit));
        }

        [HttpGet]
        public async Task<IActionResult> CancelVisit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userCard = _context.ApplicationUser
                            .Where(d => d.Id == user.Id)
                            .Join(
                                _context.PatientModel,
                                appUser => appUser.Id,
                                patient => patient.UserId,
                                (appUser, patient) => new { appUser, patient }
                            )
                            .Join(
                                _context.PatientCardModel,
                                appUserPat => appUserPat.patient.Id,
                                card => card.PatientId,
                                (appUserPat, card) => new { appUserPat, card }
                            )
                            .SingleOrDefault();

            var visits = new List<VisitHistoryViewModel>();

            if (userCard != null)
            {
                var userVisits = _context.PatientCardModel
                            .Where(d => d.Id == userCard.card.Id)
                            .Join(
                                _context.AppointmentModel,
                                card => card.Id,
                                visit => visit.PatientCardId,
                                (card, visit) => new { card, visit }
                            )
                            .Join(
                                _context.DoctorModel,
                                cardVisit => cardVisit.visit.DoctorId,
                                doctor => doctor.Id,
                                (cardVisit, doctor) => new { cardVisit, doctor }
                            )
                            .Join(
                                _context.ApplicationUser,
                                cardVisitDoctor => cardVisitDoctor.doctor.UserId,
                                applicationUser => applicationUser.Id,
                                (cardVisitDoctor, applicationUser) => new VisitHistoryViewModel
                                {
                                    Id = cardVisitDoctor.cardVisit.visit.Id,
                                    IsConfirmed = cardVisitDoctor.cardVisit.visit.IsConfirmed,
                                    DateOfApp = cardVisitDoctor.cardVisit.visit.DateOfApp,
                                    Hour = cardVisitDoctor.cardVisit.visit.Hour,
                                    DoctorFirstName = applicationUser.FirstName,
                                    DoctorLastName = applicationUser.LastName,
                                    Specialization = cardVisitDoctor.doctor.Specialization
                                }
                            )
                            .ToList();

                DateTime now = DateTime.Today;

                foreach (VisitHistoryViewModel visit in userVisits)
                {
                    DateTime myDate = DateTime.ParseExact(visit.DateOfApp, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (DateTime.Compare(myDate, now) > 0)
                    {
                        visits.Add(visit);
                    }
                }
            }

            return View(visits.AsEnumerable());
        }

        [HttpGet]
        public IActionResult AcceptCancellation(string id)
        {
            var userVisit = _context.AppointmentModel
                            .Where(d => d.Id == id)
                            .Join(
                                _context.DoctorModel,
                                visit => visit.DoctorId,
                                doctor => doctor.Id,
                                (visit, doctor) => new { visit, doctor }
                            )
                            .Join(
                                _context.ApplicationUser,
                                visitDoctor => visitDoctor.doctor.UserId,
                                applicationUser => applicationUser.Id,
                                (visitDoctor, applicationUser) => new VisitHistoryViewModel
                                {
                                    Id = visitDoctor.visit.Id,
                                    IsConfirmed = visitDoctor.visit.IsConfirmed,
                                    DateOfApp = visitDoctor.visit.DateOfApp,
                                    Hour = visitDoctor.visit.Hour,
                                    DoctorFirstName = applicationUser.FirstName,
                                    DoctorLastName = applicationUser.LastName,
                                    Specialization = visitDoctor.doctor.Specialization
                                }
                            )
                            .Single();

            return View(userVisit);
        }

        [HttpPost]
        public IActionResult AcceptCancellation(VisitHistoryViewModel model)
        {
            var updateVisit = _context.AppointmentModel
                              .Single(d => d.Id == model.Id);

            _context.AppointmentModel.Remove(updateVisit);
            _context.SaveChanges();

            return RedirectToAction("CancelVisit");
        }
    }
}
 