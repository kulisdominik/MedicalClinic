using System;
using System.Collections.Generic;
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

        public IActionResult Patients(PatientInfoViewModel model)
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
                                    Date = DateTime.Now.ToString(),
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
                Date = DateTime.Now.ToString(),
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
    }
}