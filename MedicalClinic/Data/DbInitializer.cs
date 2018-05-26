using MedicalClinic.Data.Migrations;
using MedicalClinic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        public DbInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async void Initialize(IConfiguration configuration)
        {
            var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

            string[] roleNames = { "Admin", "Manager", "Patient", "Doctor", "Clerk" };

            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.EnsureCreated();

            foreach (var roleName in roleNames)
            {
                if (!(await roleManager.RoleExistsAsync(roleName)))
                {
                    await roleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }

            // Create admin account
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            var userAdmin = new ApplicationUser
            {
                UserName = "test@admin.pl",
                Email = "test@admin.pl",
                EmailConfirmed = true,
                ResidenceId = 1
            };

            string userPassword = "P@ssw0rd";

            if (await userManager.FindByEmailAsync("test@admin.pl") == null)
            {
                context.ResidenceModel.Add(new ResidenceModel { });
                context.SaveChanges();

                var success = await userManager.CreateAsync(userAdmin, userPassword);
                if (success.Succeeded)
                {
                    await userManager.AddToRoleAsync(userAdmin, "Admin");
                }
                context.SaveChanges();
            }

            var userDoctors = new ApplicationUser[]
            {
                new ApplicationUser
                {
                    UserName = "doc@tor.pl",
                    Email = "doc@tor.pl",
                    EmailConfirmed = true,
                    FirstName = "Henryk",
                    LastName = "Kowalski",
                    PIN = "66071496752",
                    PhoneNum = "345663874",
                    Sex = "Male",
                    ResidenceId = 2
                },

                new ApplicationUser
                {
                    UserName = "doctor@test.pl",
                    Email = "doctor@test.pl",
                    EmailConfirmed = true,
                    FirstName = "Maria",
                    LastName = "Nowak",
                    PIN = "72062607345",
                    PhoneNum = "796256840",
                    Sex = "Female",
                    ResidenceId = 3
                }
            };

            foreach (ApplicationUser user in userDoctors)
            {
                if (!context.ApplicationUser.Any(o => o.UserName == user.UserName))
                {
                    context.ResidenceModel.Add(new ResidenceModel { });
                    context.SaveChanges();

                    var success = await userManager.CreateAsync(user, userPassword);
                    if (success.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Doctor");
                    }

                    context.SaveChanges();
                }
            }

            var doctors = new DoctorModel[]
            {
                new DoctorModel{
                    Specialization = "Endokrynologia",
                    UserId = userDoctors[0].Id
                },
                new DoctorModel{
                    Specialization = "Alergologia",
                    UserId = userDoctors[1].Id
                }
            };

            foreach (DoctorModel doctor in doctors)
            {
                if (!context.DoctorModel.Any(o => o.Specialization == doctor.Specialization))
                {
                    context.DoctorModel.Add(doctor);
                    context.SaveChanges();
                }
            }

            var userPatient = new ApplicationUser
            {
                UserName = "patient@test.pl",
                Email = "patient@test.pl",
                EmailConfirmed = true,
                FirstName = "Tomasz",
                LastName = "Adamski",
                PIN = "79022405942",
                PhoneNum = "819458234",
                Sex = "Male",
                ResidenceId = 4
            };

            string patientPassword = "H@sl01";

            if (!context.ApplicationUser.Any(o => o.UserName == userPatient.UserName))
            {
                context.ResidenceModel.Add(new ResidenceModel { });
                context.SaveChanges();

                var success = await userManager.CreateAsync(userPatient, patientPassword);
                if (success.Succeeded)
                {
                    await userManager.AddToRoleAsync(userPatient, "Patient");
                }

                var patient = new PatientModel
                {
                    UserId = userPatient.Id
                };

                context.PatientModel.Add(patient);
                context.SaveChanges();
            }

            var workHours = new WorkHoursModel[]
            {
                new WorkHoursModel{
                    DayofWeek = "wtorek",
                    StartHour = "12:55",
                    EndHour = "16:40",
                    DoctorId = doctors[0].Id
                },

                new WorkHoursModel{
                    DayofWeek = "czwartek",
                    StartHour = "9:30",
                    EndHour = "13:00",
                    DoctorId = doctors[1].Id
                }
            };

            foreach (WorkHoursModel workHour in workHours)
            {
                if (!context.WorkHours.Any(o => o.DayofWeek == workHour.DayofWeek && o.StartHour == workHour.StartHour))
                {
                    context.WorkHours.Add(workHour);
                    context.SaveChanges();
                }
            }
        }
    }
}
