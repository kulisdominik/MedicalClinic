using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalClinic.Data.Migrations;
using MedicalClinic.Models;
using MedicalClinic.Models.ClerkViewModels;
using MedicalClinic.Models.PatientViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinic.Controllers
{
    public class ClerkController : Controller
    {
        private ApplicationDbContext _context;

        public ClerkController(ApplicationDbContext context)
        {
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
                                     Id = card.Id,
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
                             .First();

            return View(patientCard);
        }

        [HttpPost]
        public IActionResult PatientCardInfo(PatientCardViewModel model)
        {
            var updateUser = _context.PatientCardModel
                              .Join(
                                  _context.PatientModel,
                                  card => card.PatientId,
                                  pat => pat.Id,
                                  (card, pat) => new { card, pat }
                              )
                              .Where(c => c.pat.Id == model.Id)
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
                                  .Where(c => c.pat.Id == model.Id)
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
    }
}