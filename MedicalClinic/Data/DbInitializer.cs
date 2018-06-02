using MedicalClinic.Data.Migrations;
using MedicalClinic.Models;
using MedicalClinic.Services;
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

            string[] roleNames = { RoleNames.Admin, RoleNames.Manager, RoleNames.Patient, RoleNames.Doctor, RoleNames.Clerk };

            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            foreach (var roleName in roleNames)
            {
                var role = await roleManager.RoleExistsAsync(roleName);
                if (!role)
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
                    await userManager.AddToRoleAsync(userAdmin, RoleNames.Admin);
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
                    Sex = "Mężczyzna",
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
                    Sex = "Kobieta",
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
                        await userManager.AddToRoleAsync(user, RoleNames.Doctor);
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

            var userPatients = new ApplicationUser[]
            {
                new ApplicationUser
                {
                    UserName = "patient@test.pl",
                    Email = "patient@test.pl",
                    EmailConfirmed = true,
                    FirstName = "Tomasz",
                    LastName = "Adamski",
                    PIN = "79022405942",
                    PhoneNum = "819458234",
                    Sex = "Mężczyzna",
                    ResidenceId = 4
                },

                 new ApplicationUser
                {
                    UserName = "patient2@test.pl",
                    Email = "patient2@test.pl",
                    EmailConfirmed = true,
                    FirstName = "Agata",
                    LastName = "Walczyk",
                    PIN = "69091295041",
                    PhoneNum = "758399405",
                    Sex = "Kobieta",
                    ResidenceId = 5
                },

                new ApplicationUser
                {
                    UserName = "patient3@test.pl",
                    Email = "patient3@test.pl",
                    EmailConfirmed = true,
                    FirstName = "Tobiasz",
                    LastName = "Adamski",
                    PIN = "78031895332",
                    PhoneNum = "199405394",
                    Sex = "Mężczyzna",
                    ResidenceId = 6
                },
            };

            string patientPassword = "H@sl01";

            foreach (ApplicationUser userPatient in userPatients)
            {
                if (!context.ApplicationUser.Any(o => o.UserName == userPatient.UserName))
                {
                    var residence = new ResidenceModel
                    {
                        Country = "Polska",
                        Street = "Krakowska",
                        City = "Kraków",
                        PostalCode = "31-066",
                        BuildingNum = "30",
                        FlatNum = "4"
                    };

                    context.ResidenceModel.Add(residence);
                    context.SaveChanges();

                    var success = await userManager.CreateAsync(userPatient, patientPassword);
                    if (success.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userPatient, RoleNames.Patient);
                    }

                    var patient = new PatientModel
                    {
                        UserId = userPatient.Id
                    };

                    context.PatientModel.Add(patient);
                    context.SaveChanges();

                    var patientCard = new PatientCardModel
                    {
                        Date = "17/05/2018",
                        PatientId = patient.Id
                    };

                    context.PatientCardModel.Add(patientCard);
                    context.SaveChanges();
                }
            }

            var userClerk = new ApplicationUser
            {
                UserName = "clerk@test.pl",
                Email = "clerk@test.pl",
                EmailConfirmed = true,
                FirstName = "Edyta",
                LastName = "Kotarska",
                PIN = "86062702842",
                PhoneNum = "850904195",
                Sex = "Kobieta",
                ResidenceId = 7
            };

            if (!context.ApplicationUser.Any(o => o.UserName == userClerk.UserName))
            {
                context.ResidenceModel.Add(new ResidenceModel { });
                context.SaveChanges();

                var success = await userManager.CreateAsync(userClerk, userPassword);
                if (success.Succeeded)
                {
                    await userManager.AddToRoleAsync(userClerk, RoleNames.Clerk);
                }

                var clerk = new ClerkModel
                {
                    UserId = userClerk.Id
                };

                context.ClerkModel.Add(clerk);
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

            var visits = new AppointmentModel[]
            {
                new AppointmentModel
                {
                    DateOfApp = "8/05/2018",
                    DoctorId = doctors[0].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "15/05/2018",
                    DoctorId = doctors[0].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "22/05/2018",
                    DoctorId = doctors[0].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "05/06/2018",
                    DoctorId = doctors[0].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "24/05/2018",
                    DoctorId = doctors[1].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "07/06/2018",
                    DoctorId = doctors[1].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "14/06/2018",
                    DoctorId = doctors[1].Id
                }
            };

            foreach (AppointmentModel app in visits)
            {
                if (!context.AppointmentModel.Any(o => o.DateOfApp == app.DateOfApp))
                {
                    context.AppointmentModel.Add(app);
                    context.SaveChanges();
                }
            }
        }
    }
}
