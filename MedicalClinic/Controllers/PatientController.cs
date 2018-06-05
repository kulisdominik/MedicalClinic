﻿using System;
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

        public IActionResult VisitRegistration()
        {
            var doctors = _context.ApplicationUser
                            .Join(
                                _context.DoctorModel,
                                user => user.Id,
                                doctor => doctor.UserId,
                                (user, doctor) => new { user, doctor }
                            )
                            .Join(
                                _context.WorkHours,
                                doc => doc.doctor.Id,
                                hours => hours.DoctorId,
                                (doc, hours) => new VisitRegistrationViewModel
                                {
                                    Id = doc.doctor.Id,
                                    FirstName = doc.user.FirstName,
                                    LastName = doc.user.LastName,
                                    DayofWeek = hours.DayofWeek,
                                    StartHour = hours.StartHour,
                                    EndHour = hours.EndHour
                                }
                            )
                            .ToList();
            
            return View(doctors);
        }

        [HttpGet]
        public IActionResult SelectHour(string id)
        {
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
                                user => user.Id,
                                doctor => doctor.UserId,
                                (user, doctor) => new { user, doctor }
                            )
                            .Where(d => d.doctor.Id == id)
                            .Join(
                                _context.WorkHours,
                                doc => doc.doctor.Id,
                                hours => hours.DoctorId,
                                (doc, hours) => new VisitRegistrationViewModel
                                {
                                    Id = doc.doctor.Id,
                                    FirstName = doc.user.FirstName,
                                    LastName = doc.user.LastName,
                                    DayofWeek = hours.DayofWeek,
                                    StartHour = hours.StartHour,
                                    EndHour = hours.EndHour,
                                    Hours = new List<string>(),
                                    Grade = new List<GradeModel>()
                                }
                            )
                            .Single();

            doctorInfo.Hours.Clear();
            doctorInfo.Grade.Clear();

            foreach(var gr in grades)
            {
                var grade = new GradeModel
                {
                    Grade = gr.gr.Grade,
                    Comment = gr.gr.Comment
                };

                doctorInfo.Grade.Add(grade);
            }

            string startHour = doctorInfo.StartHour;
            string endHour = doctorInfo.EndHour;

            startHour = startHour.Replace(":", string.Empty);
            endHour = endHour.Replace(":", string.Empty);

            int sHour = Int32.Parse(startHour);
            int eHour = Int32.Parse(endHour);

            int hour = sHour;

            while ((hour+30) < eHour)
            {
                doctorInfo.Hours.Add(hour.ToString().Insert(hour.ToString().Length - 2, ":"));

                if((hour + 30) % 100 >= 60)
                    hour = hour + 100 - 30;
                else
                    hour = hour + 30;
            }

            return View(doctorInfo);
        }

        [HttpPost]
        public IActionResult SelectHour(VisitRegistrationViewModel model, string id)
        {
            return View(model);
        }
    }
}
 