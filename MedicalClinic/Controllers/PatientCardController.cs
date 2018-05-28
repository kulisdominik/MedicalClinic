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

            return View(patientCard);
        }

        public IActionResult VisitHistory(string id)
        {
            var userVisits = _context.PatientCardModel
                            .Join(
                                _context.AppointmentModel,
                                card => card.Id,
                                visit => visit.PatientCardId,
                                (card, visit) => new { card, visit }
                            )
                            .Where(d => d.card.Id == id)
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
                                (cardVisitDoctor, applicationUser) =>  new VisitHistoryViewModel
                                {
                                    Id = cardVisitDoctor.cardVisit.visit.Id,
                                    DateOfApp = cardVisitDoctor.cardVisit.visit.DateOfApp,
                                    DoctorFirstName = applicationUser.FirstName,
                                    DoctorLastName = applicationUser.LastName,
                                    Specialization = cardVisitDoctor.doctor.Specialization
                                }
                            )
                            .ToList();

            return View(userVisits);
        }

        public IActionResult SeeDetails(string id)
        {
            /*
           var recipeInfo = _context.AppointmentModel
                           .Join(
                               _context.RecipeModel,
                               visit => visit.RecipeId,
                               recipe => recipe.Id,
                               (visit, recipe) => new { visit, recipe }
                           )
                           .Where(d => d.visit.Id == userVisit.Id)
                           .SingleOrDefault();


           var medicineInfo = _context.RecipeModel
                                   .Join(
                                       _context.MedicineModel,
                                       recipe => recipe.Id,
                                       medicine => medicine.RecipeId,
                                       (recipe, medicine) => new { recipe, medicine }
                                   )
                                   .Where(d => d.recipe.Id == recipeInfo.recipe.Id)
                                   .ToList();


           var referralInfo = _context.AppointmentModel
                           .Join(
                               _context.ReferralModel,
                               visit => visit.Id,
                               referral => referral.AppointmentId,
                               (visit, referral) => new { visit, referral }
                           )
                           .Join(
                               _context.ExaminationModel,
                               visRef => visRef.referral.ExaminationId,
                               examination => examination.Id,
                               (visRef, examination) => new { visRef, examination }
                           )
                           .SingleOrDefault();

           var visits = new VisitHistoryViewModel[] { };

           foreach

           visits.Id = 
               Id = userVisit.Id,
               DateOfApp = userVisit.DateOfApp,
               DoctorFirstName = userVisit.DoctorFirstName,
               DoctorLastName = userVisit.DoctorLastName,
               Notes = recipeInfo.recipe.Descrpition,
               Specialization = userVisit.Specialization,
               DateOfIssuance = visRef.referral.DateOfIssuance,
               NameOfExamination = examination.NameOfExamination
           }
       */
            return View();
        }
    }
}
 