using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalClinic.Data.Migrations;
using MedicalClinic.Models;
using MedicalClinic.Models.PatientViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinic.Controllers
{
    public class PatientCardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;

        public PatientCardController(
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

        public async Task<IActionResult> PatientCard(PatientCardViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var patientCard = _context.ApplicationUser
                            .Join(
                                _context.ResidenceModel,
                                applicationUser => applicationUser.ResidenceId,
                                residence => residence.Id,
                                (applicationUser, residence) => new { applicationUser, residence }
                            )
                            .Where(d => d.applicationUser.Id == user.Id)
                            .Join(
                                _context.PatientModel,
                                app => app.applicationUser.Id,
                                patient => patient.UserId,
                                (app, patient) => new { app, patient }
                            )
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
                            .FirstOrDefault();

            if (patientCard == null)
            {
                patientCard = new PatientCardViewModel { };
            }

            return View(patientCard);
        }
    }
}