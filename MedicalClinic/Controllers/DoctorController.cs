using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedicalClinic.Models;
using MedicalClinic.Models.DoctorViewModels;
using MedicalClinic.Data.Migrations;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using MedicalClinic.Models.ClerkViewModels;

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
                        .Where(c => c.app.IsConfirmed == 1)
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
                        .Where(c => c.app.IsConfirmed == 1)
                        .ToList();

            foreach (var date in dates)
            {
                visits.DateOfApp.Add(date.app.DateOfApp);
            }

            visits.Visits.Clear();

            var userVisits = _context.AppointmentModel
                            .Where(d => (d.DateOfApp == model.SelectedDate) && (d.DoctorId == model.Id) && (d.IsConfirmed == 1))
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

        public IActionResult EditVisit(string id)
        {
            var visit = _context.AppointmentModel
                           .Where(d => d.Id == id)
                           .Join(
                              _context.PatientCardModel,
                              app => app.PatientCardId,
                              card => card.Id,
                              (app, card) => new { app, card }
                           )
                           .Join(
                              _context.PatientModel,
                              appCard => appCard.card.PatientId,
                              patient => patient.Id,
                              (appCard, patient) => new { appCard, patient }
                           )
                           .Join(
                              _context.ApplicationUser,
                              appCardPat => appCardPat.patient.UserId,
                              user => user.Id,
                              (appCardPat, user) => new { appCardPat, user }
                           )
                           .Single();

            var recipeInfo = _context.AppointmentModel
                            .Where(d => d.Id == id)
                            .Join(
                                _context.RecipeModel,
                                app => app.RecipeId,
                                recipe => recipe.Id,
                                (app, recipe) => new { app, recipe }
                            )
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
            if (recipeInfo != null)
            {
                var medicineInfo = _context.MedicineModel
                                   .Where(d => d.RecipeId == recipeInfo.recipe.Id)
                                   .ToList();

                foreach (MedicineModel medicine in medicineInfo)
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

            var visitInfo = new VisitsViewModel
            {
                Id = id,
                CardId = cardInfo.card.Id,
                DateOfApp = visit.appCardPat.appCard.app.DateOfApp,
                PatientFirstName = visit.user.FirstName,
                PatientLastName = visit.user.LastName,
                Referral = new List<ReferralViewModel>(),
                Edit = false
            };

            if (recipeInfo != null)
            {
                visitInfo.ExpDate = recipeInfo.recipe.ExpDate;
                visitInfo.Descrpition = recipeInfo.recipe.Descrpition;
                visitInfo.NameofMedicine = medicineList;
            }

            foreach (var referral in referralInfo)
            {
                var refInfo = new ReferralViewModel
                {
                    DateOfIssuance = referral.referral.DateOfIssuance,
                    NameOfExamination = referral.examination.NameOfExamination
                };

                visitInfo.Referral.Add(refInfo);
            }

            if (diagnosisInfo != null)
            {
                visitInfo.DeseaseName = diagnosisInfo.DeseaseName;
                visitInfo.Symptoms = diagnosisInfo.Symptoms;
                visitInfo.Synopsis = diagnosisInfo.Synopsis;
            }

            DateTime myDate = DateTime.ParseExact(visitInfo.DateOfApp, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (DateTime.Compare(myDate, DateTime.Today) < 0)
            {
                visitInfo.Edit = true;
            }

            return View(visitInfo);
        }

        [HttpGet]
        public IActionResult AddDiagnosis(string id)
        {
            var diagnosis = new DiagnosisViewModel
            {
                AppointmentId = id
            };

            return View(diagnosis);
        }

        [HttpPost]
        public IActionResult AddDiagnosis(DiagnosisViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newDiagnosis = new DiagnosisModel
            {
                DeseaseName = model.DeseaseName,
                Synopsis = model.Synopsis,
                Symptoms = model.Symptoms,
                AppointmentId = model.AppointmentId
            };

            _context.DiagnosisModel.Add(newDiagnosis);
            _context.SaveChanges();

            return RedirectToAction("EditVisit", "Doctor", new { id = model.AppointmentId });
        }

        [HttpGet]
        public IActionResult AddRecipe(string id)
        {
            var recipe = new RecipeViewModel
            {
                AppointmentId = id
            };

            return View(recipe);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipe(RecipeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newRecipe = new RecipeModel
            {
                ExpDate = model.ExpDate,
                Descrpition = model.Descrpition
            };

            _context.RecipeModel.Add(newRecipe);

            if (await _context.SaveChangesAsync() > 0)
            {
                    List<string> medicine = model.NameOfMedicine.Split(',').ToList();

                    foreach (string med in medicine)
                    {
                        var newMedicine = new MedicineModel
                        {
                            Name = med,
                            RecipeId = newRecipe.Id
                        };

                        _context.MedicineModel.Add(newMedicine);
                        _context.SaveChanges();
                    }

                var updateVisit = _context.AppointmentModel
                              .Where(d => d.Id == model.AppointmentId)
                              .First();

                updateVisit.RecipeId = newRecipe.Id;
                _context.SaveChanges();

                return RedirectToAction("EditVisit", "Doctor", new { id = model.AppointmentId });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddReferral(string id)
        {
            var referral = new ReferralViewModel
            {
                AppointmentId = id
            };

            return View(referral);
        }

        [HttpPost]
        public async Task<IActionResult> AddReferral(ReferralViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newExamination = new ExaminationModel
            {
                NameOfExamination = model.NameOfExamination
            };

            _context.ExaminationModel.Add(newExamination);

            if( await _context.SaveChangesAsync() > 0 )
            {
                var newReferral = new ReferralModel
                {
                    DateOfIssuance = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    AppointmentId = model.AppointmentId,
                    ExaminationId = newExamination.Id
                };

                _context.ReferralModel.Add(newReferral);
                _context.SaveChanges();
            }

            return RedirectToAction("EditVisit", "Doctor", new { id = model.AppointmentId });
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

            return View(patientCard);
        }
    }
}