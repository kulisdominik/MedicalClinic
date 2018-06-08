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

        public async Task<IActionResult> PatientCard()
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

            var visits = new List<VisitHistoryViewModel>();

            DateTime now = DateTime.Today;

            foreach (VisitHistoryViewModel visit in userVisits)
            {
                DateTime myDate = DateTime.ParseExact(visit.DateOfApp, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (DateTime.Compare(myDate, now) < 0)
                {
                    visits.Add(visit);
                }
            }

            return View(visits.AsEnumerable());
        }

        [HttpGet]
        public IActionResult ShowDetails(string id)
        {
            var visit = _context.AppointmentModel
                            .Where(d => d.Id == id)
                            .Join(
                               _context.DoctorModel,
                               app => app.DoctorId,
                               doctor => doctor.Id,
                               (app, doctor) => new { app, doctor }
                            )
                            .Join(
                               _context.ApplicationUser,
                               appDoc => appDoc.doctor.UserId,
                               user => user.Id,
                               (appDoc, user) => new { appDoc, user }
                            )
                            .Single();

           var recipeInfo = _context.AppointmentModel
                           .Join(
                               _context.RecipeModel,
                               app => app.RecipeId,
                               recipe => recipe.Id,
                               (app, recipe) => new { app, recipe }
                           )
                           .Where(d => d.app.Id == id)
                           .SingleOrDefault();

            var cardInfo = _context.AppointmentModel
                            .Where(d => d.Id == id)
                            .Join(
                                _context.PatientCardModel,
                                app => app.PatientCardId,
                                card => card.Id,
                                (app, card) => new { app, card }
                            )
                            .Single();

            List<string> medicineList = new List<string>();
            if(recipeInfo != null)
            {
                var medicineInfo = _context.MedicineModel
                                    .Where(d => d.RecipeId == recipeInfo.recipe.Id)
                                   .ToList();

                foreach(MedicineModel medicine in medicineInfo)
                {
                    medicineList.Add(medicine.Name);
                }
            }
            
           var referralInfo = _context.ReferralModel
                            .Where(d => d.AppointmentId == id)
                           .Join(
                               _context.ExaminationModel,
                               referral => referral.ExaminationId,
                               examination => examination.Id,
                               (referral, examination) => new { referral, examination }
                           )
                           .ToList();

            var diagnosisInfo = _context.DiagnosisModel
                               .Where(d => d.AppointmentId == id)
                               .SingleOrDefault();

            var gradeInfo = _context.GradeModel
                            .Where(d => d.AppointmentId == id)
                            .SingleOrDefault();

            var visitInfo = new VisitDetailsViewModel
            {
                Id = id,
                CardId = cardInfo.card.Id,
                DateOfApp = visit.appDoc.app.DateOfApp,
                DoctorFirstName = visit.user.FirstName,
                DoctorLastName = visit.user.LastName,
                Specialization = visit.appDoc.doctor.Specialization,
                Referral = new List<ReferralViewModel>()
            };

            if(recipeInfo != null)
            {
                visitInfo.ExpDate = recipeInfo.recipe.ExpDate;
                visitInfo.Descrpition = recipeInfo.recipe.Descrpition;
                visitInfo.NameofMedicine = medicineList;
            }
            
            foreach(var referral in referralInfo)
            {
                var refInfo = new ReferralViewModel
                {
                    DateOfIssuance = referral.referral.DateOfIssuance,
                    NameOfExamination = referral.examination.NameOfExamination
                };

                visitInfo.Referral.Add(refInfo);
            }

            if(diagnosisInfo != null)
            {
                visitInfo.DeseaseName = diagnosisInfo.DeseaseName;
                visitInfo.Symptoms = diagnosisInfo.Symptoms;
                visitInfo.Synopsis = diagnosisInfo.Synopsis;
            }

            if(gradeInfo != null)
            {
                visitInfo.Comment = gradeInfo.Comment;
                visitInfo.Grade = gradeInfo.Grade;
            }

            return View(visitInfo);
        }

        [HttpPost]
        public IActionResult ShowDetails(VisitDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newGrade = new GradeModel
            {
                Grade = model.Grade,
                Comment = model.Comment,
                AppointmentId = model.Id
            };

            _context.GradeModel.Add(newGrade);
            _context.SaveChanges();

            return RedirectToAction(nameof(ShowDetails));
        }
    }
}
 