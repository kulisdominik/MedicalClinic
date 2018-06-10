using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MedicalClinic.Data.Migrations;
using MedicalClinic.Models;
using MedicalClinic.Models.ClerkViewModels;
using MedicalClinic.Models.PatientViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinic.Controllers
{
    public class ClerkController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;

        public ClerkController(
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

        public IActionResult Patients()
        {
            var patientInfo = _context.ApplicationUser
                            .Join(
                                _context.PatientModel,
                                user => user.Id,
                                patient => patient.UserId,
                                (user, patient) => new PatientInfoViewModel
                                {
                                    Id = patient.Id,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName
                                }
                            )
                            .OrderBy(s => s.LastName)
                            .OrderBy(s => s.FirstName)
                            .ToList();

            return View(patientInfo);
        }

        [HttpGet]
        public IActionResult PatientCardInfo(string id)
        {
            var patientCard = _context.ApplicationUser
                             .Join(
                                 _context.ResidenceModel,
                                 applicationUser => applicationUser.ResidenceId,
                                 residence => residence.Id,
                                 (applicationUser, residence) => new { applicationUser, residence }
                             )
                             .Join(
                                 _context.PatientModel,
                                 app => app.applicationUser.Id,
                                 patient => patient.UserId,
                                 (app, patient) => new { app, patient }
                             )
                             .Where(d => d.patient.Id == id)
                             .Join(
                                 _context.PatientCardModel,
                                 pat => pat.patient.Id,
                                 card => card.PatientId,
                                 (pat, card) => new PatientCardViewModel
                                 {
                                     PatientId = id,
                                     Date = card.Date,
                                     FirstName = pat.app.applicationUser.FirstName,
                                     LastName = pat.app.applicationUser.LastName,
                                     PIN = pat.app.applicationUser.PIN,
                                     PhoneNum = pat.app.applicationUser.PhoneNum,
                                     Sex = pat.app.applicationUser.Sex,
                                     Country = pat.app.residence.Country,
                                     Street = pat.app.residence.Street,
                                     City = pat.app.residence.City,
                                     PostalCode = pat.app.residence.PostalCode,
                                     BuildingNum = pat.app.residence.BuildingNum,
                                     FlatNum = pat.app.residence.FlatNum
                                 }
                             )
                             .FirstOrDefault();

            if (patientCard == null)
            {
                patientCard = new PatientCardViewModel
                {
                    PatientId = id
                };
            }

            return View(patientCard);
        }

        [HttpPost]
        public IActionResult PatientCardInfo(PatientCardViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var updateUser = _context.PatientCardModel
                              .Join(
                                  _context.PatientModel,
                                  card => card.PatientId,
                                  pat => pat.Id,
                                  (card, pat) => new { card, pat }
                              )
                              .Where(c => c.pat.Id == model.PatientId)
                              .Join(
                                  _context.ApplicationUser,
                                  patCard => patCard.pat.UserId,
                                  user => user.Id,
                                  (patCard, user) => new { patCard, user }
                              )
                              .First();

            updateUser.user.FirstName = model.FirstName;
            updateUser.user.LastName = model.LastName;
            updateUser.user.PIN = model.PIN;
            updateUser.user.PhoneNum = model.PhoneNum;
            updateUser.user.Sex = model.Sex;

            _context.SaveChanges();

            var updateResidence = _context.PatientCardModel
                                  .Join(
                                      _context.PatientModel,
                                      card => card.PatientId,
                                      pat => pat.Id,
                                      (card, pat) => new { card, pat }
                                  )
                                  .Where(c => c.pat.Id == model.PatientId)
                                  .Join(
                                      _context.ApplicationUser,
                                      patCard => patCard.pat.UserId,
                                      user => user.Id,
                                      (patCard, user) => new { patCard, user }
                                   )
                                   .Join(
                                      _context.ResidenceModel,
                                      patCardUser => patCardUser.user.ResidenceId,
                                      residence => residence.Id,
                                      (patCardUser, residence) => new { patCardUser, residence }
                                  )
                                  .First();

            updateResidence.residence.Country = model.Country;
            updateResidence.residence.Street = model.Street;
            updateResidence.residence.City = model.City;
            updateResidence.residence.PostalCode = model.PostalCode;
            updateResidence.residence.BuildingNum = model.BuildingNum;
            updateResidence.residence.FlatNum = model.FlatNum;

            _context.SaveChanges();

            return RedirectToAction(nameof(PatientCardInfo));
        }

        [HttpGet]
        public IActionResult NewPatientCard(string id)
        {
            var patientCard = _context.PatientModel
                            .Join(
                                _context.ApplicationUser,
                                patient => patient.UserId,
                                applicationUser => applicationUser.Id,
                                (patient, applicationUser) => new { patient, applicationUser }
                            )
                            .Where(d => d.patient.Id == id)
                            .Join(
                                _context.ResidenceModel,
                                pat => pat.applicationUser.ResidenceId,
                                residence => residence.Id,
                                (pat, residence) => new PatientCardViewModel
                                {
                                    PatientId = id,
                                    Date = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    FirstName = pat.applicationUser.FirstName,
                                    LastName = pat.applicationUser.LastName,
                                    PIN = pat.applicationUser.PIN,
                                    PhoneNum = pat.applicationUser.PhoneNum,
                                    Sex = pat.applicationUser.Sex,
                                    Country = residence.Country,
                                    Street = residence.Street,
                                    City = residence.City,
                                    PostalCode = residence.PostalCode,
                                    BuildingNum = residence.BuildingNum,
                                    FlatNum = residence.FlatNum
                                }
                            )
                            .FirstOrDefault();

            return View(patientCard);
        }

        [HttpPost]
        public async Task<IActionResult> NewPatientCard(PatientCardViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var clerkUser = _context.ApplicationUser
                            .Join(
                                _context.ClerkModel,
                                appUser => appUser.Id,
                                clerk => clerk.UserId,
                                (appUser, clerk) => new { appUser, clerk }
                            )
                            .Where(d => d.appUser.Id == user.Id)
                            .SingleOrDefault();

            var newCard = new PatientCardModel
            {
                Date = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                PatientId = model.PatientId,
                ClerkId = clerkUser.clerk.Id
            };

            _context.PatientCardModel.Add(newCard);

            if(await _context.SaveChangesAsync() > 0)
            {
                var updateUser = _context.PatientModel
                              .Join(
                                  _context.ApplicationUser,
                                  patient => patient.UserId,
                                  appUser => appUser.Id,
                                  (patient, appUser) => new { patient, appUser }
                              )
                              .Where(c => c.patient.Id == model.PatientId)
                              .First();

                updateUser.appUser.FirstName = model.FirstName;
                updateUser.appUser.LastName = model.LastName;
                updateUser.appUser.PIN = model.PIN;
                updateUser.appUser.PhoneNum = model.PhoneNum;
                updateUser.appUser.Sex = model.Sex;

                _context.SaveChanges();

                var updateResidence = _context.PatientModel
                                      .Join(
                                          _context.ApplicationUser,
                                          patient => patient.UserId,
                                          appUser => appUser.Id,
                                          (patient, appUser) => new { patient, appUser }
                                       )
                                      .Where(c => c.patient.Id == model.PatientId)
                                       .Join(
                                          _context.ResidenceModel,
                                          patCardUser => patCardUser.appUser.ResidenceId,
                                          residence => residence.Id,
                                          (patCardUser, residence) => new { patCardUser, residence }
                                      )
                                      .First();

                updateResidence.residence.Country = model.Country;
                updateResidence.residence.Street = model.Street;
                updateResidence.residence.City = model.City;
                updateResidence.residence.PostalCode = model.PostalCode;
                updateResidence.residence.BuildingNum = model.BuildingNum;
                updateResidence.residence.FlatNum = model.FlatNum;

                _context.SaveChanges();

                return RedirectToAction("PatientCardInfo", "Clerk", model);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Visits(string id)
        {
            var userCard = _context.ApplicationUser
                            .Join(
                                _context.PatientModel,
                                appUser => appUser.Id,
                                patient => patient.UserId,
                                (appUser, patient) => new { appUser, patient }
                            )
                            .Where(d => d.patient.Id == id)
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

            return RedirectToAction("Patients");
        }

        [HttpGet]
        public IActionResult ConfirmVisit()
        {
            var userVisits = _context.ApplicationUser
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
                            .Join(
                                _context.AppointmentModel,
                                appUserPatCard => appUserPatCard.card.Id,
                                visit => visit.PatientCardId,
                                (appUserPatCard, visit) => new { appUserPatCard, visit }
                            )
                            .Where(d => d.visit.IsConfirmed == 0)
                            .Join(
                                _context.DoctorModel,
                                appUserPatCardVisit => appUserPatCardVisit.visit.DoctorId,
                                doctor => doctor.Id,
                                (appUserPatCardVisit, doctor) => new { appUserPatCardVisit, doctor }
                            )
                            .Join(
                                _context.ApplicationUser,
                                cardVisitDoctor => cardVisitDoctor.doctor.UserId,
                                applicationUser => applicationUser.Id,
                                (cardVisitDoctor, applicationUser) => new VisitsToConfirmViewModel
                                {
                                    Id = cardVisitDoctor.appUserPatCardVisit.visit.Id,
                                    DateOfApp = cardVisitDoctor.appUserPatCardVisit.visit.DateOfApp,
                                    Hour = cardVisitDoctor.appUserPatCardVisit.visit.Hour,
                                    DoctorFirstName = applicationUser.FirstName,
                                    DoctorLastName = applicationUser.LastName,
                                    PatientFirstName = cardVisitDoctor.appUserPatCardVisit.appUserPatCard.appUserPat.appUser.FirstName,
                                    PatientLastName = cardVisitDoctor.appUserPatCardVisit.appUserPatCard.appUserPat.appUser.LastName,
                                    Specialization = cardVisitDoctor.doctor.Specialization
                                }
                            )
                            .ToList();

            var visits = new List<VisitsToConfirmViewModel>();

            DateTime now = DateTime.Today;

            foreach (VisitsToConfirmViewModel visit in userVisits)
            {
                DateTime myDate = DateTime.ParseExact(visit.DateOfApp, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (DateTime.Compare(myDate, now) > 0)
                {
                    visits.Add(visit);
                }
            }

            return View(visits.AsEnumerable());
        }

        [HttpGet]
        public IActionResult AcceptConfirmation(string id)
        {
            var userVisit = _context.ApplicationUser
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
                           .Join(
                               _context.AppointmentModel,
                               appUserPatCard => appUserPatCard.card.Id,
                               visit => visit.PatientCardId,
                               (appUserPatCard, visit) => new { appUserPatCard, visit }
                           )
                           .Where(d => (d.visit.IsConfirmed == 0) && (d.visit.Id == id))
                           .Join(
                               _context.DoctorModel,
                               appUserPatCardVisit => appUserPatCardVisit.visit.DoctorId,
                               doctor => doctor.Id,
                               (appUserPatCardVisit, doctor) => new { appUserPatCardVisit, doctor }
                           )
                           .Join(
                               _context.ApplicationUser,
                               cardVisitDoctor => cardVisitDoctor.doctor.UserId,
                               applicationUser => applicationUser.Id,
                               (cardVisitDoctor, applicationUser) => new VisitsToConfirmViewModel
                               {
                                   Id = cardVisitDoctor.appUserPatCardVisit.visit.Id,
                                   DateOfApp = cardVisitDoctor.appUserPatCardVisit.visit.DateOfApp,
                                   Hour = cardVisitDoctor.appUserPatCardVisit.visit.Hour,
                                   DoctorFirstName = applicationUser.FirstName,
                                   DoctorLastName = applicationUser.LastName,
                                   PatientFirstName = cardVisitDoctor.appUserPatCardVisit.appUserPatCard.appUserPat.appUser.FirstName,
                                   PatientLastName = cardVisitDoctor.appUserPatCardVisit.appUserPatCard.appUserPat.appUser.LastName,
                                   Specialization = cardVisitDoctor.doctor.Specialization
                               }
                           )
                           .Single();

            return View(userVisit);
        }

        [HttpPost]
        public IActionResult AcceptConfirmation(VisitsToConfirmViewModel model)
        {
            var updateVisit = _context.AppointmentModel
                              .Single(d => d.Id == model.Id);

            updateVisit.IsConfirmed = 1;
            _context.SaveChanges();

            return RedirectToAction("ConfirmVisit");
        }
    }
}