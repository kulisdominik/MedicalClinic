﻿using MedicalClinic.Data.Migrations;
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

            Random random = new Random();
            string[] Towns = { "Kraków", "Warszawa", "Gdańsk", "Wrocław", "Lublin" };
            string[] Streets = { "Długa", "Krótka", "Warszawska", "Armii Krajowej", "Clepardia", "Aleja", "Lea",
                                    "Wąska", "Ogrodowa", "Wilenska", "Wiejska"};

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

                var residence = new ResidenceModel
                {
                    Country = "Polska",
                    Street = Streets[random.Next(0, Streets.Length)].ToString(),
                    City = Towns[random.Next(0, Towns.Length)].ToString(),
                    PostalCode = random.Next(10, 34).ToString() + "-" + random.Next(100, 456).ToString(),
                    BuildingNum = random.Next(1, 400).ToString(),
                    FlatNum = random.Next(1, 120).ToString()
                };

                context.ResidenceModel.Add(residence);
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
                },

                 new ApplicationUser
                 {
                    UserName = "doctor@przychodnia.pl",
                    Email = "doctor@przychodnia.pl",
                    EmailConfirmed = true,
                    FirstName = "Zygmunt",
                    LastName = "Rojekt",
                    PIN = "56022323123",
                    PhoneNum = "796256840",
                    Sex = "Mężczyzna",
                    ResidenceId = 4
                }
            };

            foreach (ApplicationUser user in userDoctors)
            {
                if (!context.ApplicationUser.Any(o => o.UserName == user.UserName))
                {

                    var residence = new ResidenceModel
                    {
                        Country = "Polska",
                        Street = Streets[random.Next(0, Streets.Length)].ToString(),
                        City = Towns[random.Next(0, Towns.Length)].ToString(),
                        PostalCode = random.Next(10, 34).ToString() + "-" + random.Next(100, 456).ToString(),
                        BuildingNum = random.Next(1, 400).ToString(),
                        FlatNum = random.Next(1, 120).ToString()
                    };

                    context.ResidenceModel.Add(residence);
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
                },

                new DoctorModel{
                    Specialization = "---",
                    UserId = userDoctors[2].Id
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
                ResidenceId = 5
            };

            if (!context.ApplicationUser.Any(o => o.UserName == userClerk.UserName))
            {

                var residence = new ResidenceModel
                {
                    Country = "Polska",
                    Street = Streets[random.Next(0, Streets.Length)].ToString(),
                    City = Towns[random.Next(0, Towns.Length)].ToString(),
                    PostalCode = random.Next(10, 34).ToString() + "-" + random.Next(100, 456).ToString(),
                    BuildingNum = random.Next(1, 400).ToString(),
                    FlatNum = random.Next(1, 120).ToString()
                };

                context.ResidenceModel.Add(residence);
                context.SaveChanges();

                var success = await userManager.CreateAsync(userClerk, userPassword);
                if (success.Succeeded)
                {
                    await userManager.AddToRoleAsync(userClerk, "Clerk");
                }
            }

            var clerk = new ClerkModel
            {
                UserId = userClerk.Id
            };

            if (!context.ClerkModel.Any())
            {
                context.ClerkModel.Add(clerk);
                context.SaveChanges();
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
                    ResidenceId = 6
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
                    ResidenceId = 7
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
                    ResidenceId = 8
                },

                new ApplicationUser
                {
                    UserName = "patient4@test.pl",
                    Email = "patient4@test.pl",
                    EmailConfirmed = true,
                    FirstName = "Kornelia",
                    LastName = "Ostrowska",
                    PIN = "75071253969",
                    PhoneNum = "516810946",
                    Sex = "Kobieta",
                    ResidenceId = 9
                },
                               
                new ApplicationUser
                {
                    UserName = "bglaister0@ifeng.com",
                    Email = "bglaister0@ifeng.com",
                    EmailConfirmed = true,
                    FirstName = "Walenty",
                    LastName = "Słoń",
                    PIN = "68782448145",
                    PhoneNum = "633453498",
                    Sex = "Kobieta",
                    ResidenceId = 10
                },

                new ApplicationUser
                {
                    UserName = "cbrattan1@oaic.gov.au",
                    Email = "cbrattan1@oaic.gov.au",
                    EmailConfirmed = true,
                    FirstName = "Karolina",
                    LastName = "Kulak",
                    PIN = "185943649",
                    PhoneNum = "060450344",
                    Sex = "Kobieta",
                    ResidenceId = 11
                },

                new ApplicationUser
                {
                    UserName = "rconnichie2@yellowpages.com",
                    Email = "rconnichie2@yellowpages.com",
                    EmailConfirmed = true,
                    FirstName = "Rozalia",
                    LastName = "Kiepska",
                    PIN = "926412148",
                    PhoneNum = "354250585",
                    Sex = "Kobieta",
                    ResidenceId = 12
                },

                new ApplicationUser
                {
                    UserName = "bbowstead3@paginegialle.it",
                    Email = "bbowstead3@paginegialle.it",
                    EmailConfirmed = true,
                    FirstName = "Michał",
                    LastName = "Piwowar",
                    PIN = "947661095",
                    PhoneNum = "491760050",
                    Sex = "Mężczyzna",
                    ResidenceId = 13
                },

                new ApplicationUser
                {
                    UserName = "enornasell4@mysql.com",
                    Email = "enornasell4@mysql.com",
                    EmailConfirmed = true,
                    FirstName = "Igor",
                    LastName = "Hugo",
                    PIN = "007033489",
                    PhoneNum = "501810177",
                    Sex = "Mężczyzna",
                    ResidenceId = 14
                },

                new ApplicationUser
                {
                    UserName = "zbuessen5@harvard.edu",
                    Email = "zbuessen5@harvard.edu",
                    EmailConfirmed = true,
                    FirstName = "Żaneta",
                    LastName = "Bansen",
                    PIN = "611879490",
                    PhoneNum = "639866398",
                    Sex = "Kobieta",
                    ResidenceId = 15
                },

                new ApplicationUser
                {
                    UserName = "rleconte6@ycombinator.com",
                    Email = "rleconte6@ycombinator.com",
                    EmailConfirmed = true,
                    FirstName = "Kamil",
                    LastName = "Nowakowski",
                    PIN = "477777552",
                    PhoneNum = "358855814",
                    Sex = "Mężczyzna",
                    ResidenceId = 16
                },

                new ApplicationUser
                {
                    UserName = "srodolico7@opensource.org",
                    Email = "srodolico7@opensource.org",
                    EmailConfirmed = true,
                    FirstName = "Stefan",
                    LastName = "Radykał",
                    PIN = "929720491",
                    PhoneNum = "358209817",
                    Sex = "Mężczyzna",
                    ResidenceId = 17
                },

                new ApplicationUser
                {
                    UserName = "kbarthorpe8@examiner.com",
                    Email = "kbarthorpe8@examiner.com",
                    EmailConfirmed = true,
                    FirstName = "Krystyna",
                    LastName = "Zajac",
                    PIN = "083013242",
                    PhoneNum = "354233141",
                    Sex = "Kobieta",
                    ResidenceId = 18
                },

                new ApplicationUser
                {
                    UserName = "fsculley9@technorati.com",
                    Email = "fsculley9@technorati.com",
                    EmailConfirmed = true,
                    FirstName = "Franciszek",
                    LastName = "Droga",
                    PIN = "543349067",
                    PhoneNum = "561099340",
                    Sex = "Kobieta",
                    ResidenceId = 19
                },

                new ApplicationUser
                {
                    UserName = "achallacea@vinaora.com",
                    Email = "achallacea@vinaora.com",
                    EmailConfirmed = true,
                    FirstName = "Aleks",
                    LastName = "Kawka",
                    PIN = "663290857",
                    PhoneNum = "417500104",
                    Sex = "Kobieta",
                    ResidenceId = 20
                }
            };

            string patientPassword = "H@sl01";

            foreach (ApplicationUser userPatient in userPatients)
            {
                if (!context.ApplicationUser.Any(o => o.UserName == userPatient.UserName))
                {
                    var residence = new ResidenceModel
                    {
                        Country = "Polska",
                        Street = Streets[random.Next(0, Streets.Length)].ToString(),
                        City = Towns[random.Next(0, Towns.Length)].ToString(),
                        PostalCode = random.Next(10, 34).ToString() + "-" + random.Next(100, 456).ToString(),
                        BuildingNum = random.Next(1, 400).ToString(),
                        FlatNum = random.Next(1, 120).ToString()
                    };

                    context.ResidenceModel.Add(residence);
                    context.SaveChanges();

                    var success = await userManager.CreateAsync(userPatient, patientPassword);
                    if (success.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userPatient, "Patient");
                    }
                }
            }

            var newPatients = new PatientModel[]
            {
                new PatientModel
                {
                    UserId = userPatients[0].Id
                },

                new PatientModel
                {
                    UserId = userPatients[1].Id
                },

                new PatientModel
                {
                    UserId = userPatients[2].Id
                },

                new PatientModel
                {
                    UserId = userPatients[3].Id
                },

                new PatientModel
                {
                    UserId = userPatients[4].Id
                },

                new PatientModel
                {
                    UserId = userPatients[5].Id
                },

                new PatientModel
                {
                    UserId = userPatients[6].Id
                },

                new PatientModel
                {
                    UserId = userPatients[7].Id
                },

                new PatientModel
                {
                    UserId = userPatients[8].Id
                },

                new PatientModel
                {
                    UserId = userPatients[9].Id
                },

                new PatientModel
                {
                    UserId = userPatients[10].Id
                },

                new PatientModel
                {
                    UserId = userPatients[11].Id
                },

                new PatientModel
                {
                    UserId = userPatients[12].Id
                },

                new PatientModel
                {
                    UserId = userPatients[13].Id
                },

                new PatientModel
                {
                    UserId = userPatients[14].Id
                }
            };

            if (!context.PatientModel.Any())
            {
                foreach (PatientModel patient in newPatients)
                {
                    context.PatientModel.Add(patient);
                    context.SaveChanges();
                }
            }

            var patientCards = new PatientCardModel[]
            {
                new PatientCardModel
                {
                    Date = "20/07/2017",
                    PatientId = newPatients[0].Id,
                    ClerkId = clerk.Id
                },
                
                new PatientCardModel
                {
                    Date = "14/02/2018",
                    PatientId = newPatients[1].Id,
                    ClerkId = clerk.Id
                },

                new PatientCardModel
                {
                    Date = "16/03/2018",
                    PatientId = newPatients[2].Id,
                    ClerkId = clerk.Id
                },

                new PatientCardModel
                {
                    Date = "23/02/2018",
                    PatientId = newPatients[3].Id,
                    ClerkId = clerk.Id
                },

                new PatientCardModel
                {
                    Date = "23/09/2017",
                    PatientId = newPatients[4].Id,
                    ClerkId = clerk.Id
                },

                new PatientCardModel
                {
                    Date = "01/01/2018",
                    PatientId = newPatients[5].Id,
                    ClerkId = clerk.Id
                },

                new PatientCardModel
                {
                    Date = "11/02/2018",
                    PatientId = newPatients[6].Id,
                    ClerkId = clerk.Id
                },

                new PatientCardModel
                {
                    Date = "10/04/2018",
                    PatientId = newPatients[7].Id,
                    ClerkId = clerk.Id
                }
            };         

            if (!context.PatientCardModel.Any())
            {
                foreach (PatientCardModel patientCard in patientCards)
                {
                    context.PatientCardModel.Add(patientCard);
                    context.SaveChanges();
                }
            }

            var workHours = new WorkHoursModel[]
            {
                new WorkHoursModel
                {
                    DayofWeek = "wtorek",
                    StartHour = "12:55",
                    EndHour = "16:40",
                    DoctorId = doctors[0].Id
                },

                new WorkHoursModel
                {
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

            /* Recipe */

            var recpies = new RecipeModel[]
            {
                new RecipeModel
                {
                    ExpDate = "20/06/2019",
                    Descrpition = "Bierz dwie tabletki dziennie przed posiłekiem."
                },

                new RecipeModel
                {
                    ExpDate = "25/05/2019",
                    Descrpition = "Bierz jedną przed snem przy wyskoiej goraczce."
                },

                new RecipeModel
                {
                    ExpDate = "13/06/2019",
                    Descrpition = "Bierz dwie tabletki dziennie przed posiłekiem."
                },

                new RecipeModel
                {
                    ExpDate = "03/07/2019",
                    Descrpition = "Bierz jedną przed snem przy wyskoiej goraczce."
                },

                new RecipeModel
                {
                    ExpDate = "05/12/2019",
                    Descrpition = "Bierz dwie tabletki dziennie przed posiłekiem."
                },

                new RecipeModel
                {
                    ExpDate = "12/05/2019",
                    Descrpition = "Bierz jedną przed snem przy wyskoiej goraczce."
                },

                new RecipeModel
                {
                    ExpDate = "12/02/2019",
                    Descrpition = "Bierz dwie."
                }
            };

            if (!context.RecipeModel.Any())
            {
                foreach (RecipeModel recipe in recpies)
                {
                    context.RecipeModel.Add(recipe);
                    context.SaveChanges();
                }
            }

            /* Medicine */

            var medicines = new MedicineModel[]
            {
                new MedicineModel
                {
                    RecipeId = recpies[0].Id,
                    Name = "Adderall"
                },

                new MedicineModel
                {
                    RecipeId = recpies[1].Id,
                    Name = "Nurofen Extra Forte w Zielonej powłoce :o"
                },

                new MedicineModel
                {
                    RecipeId = recpies[2].Id,
                    Name = "NameName"
                },

                 new MedicineModel
                {
                    RecipeId = recpies[3].Id,
                    Name = "Abilify"
                },

                 new MedicineModel
                 {
                     RecipeId = recpies[4].Id,
                     Name = "Acticin"
                 },

                 new MedicineModel
                 {
                     RecipeId = recpies[5].Id,
                     Name = "Axert"
                 },

                 new MedicineModel
                 {
                     RecipeId = recpies[6].Id,
                     Name = "Kionex"
                 }
            };

            if (!context.MedicineModel.Any())
            {
                foreach (MedicineModel medicine in medicines)
                {
                    context.MedicineModel.Add(medicine);
                    context.SaveChanges();
                }
            }

            /* Appointment */

            var visits = new AppointmentModel[]
            {
                new AppointmentModel
                {
                    DateOfApp = "10/05/2018",
                    Hour = "09:30",
                    IsConfirmed = 1,
                    Notes = "-----",
                    DoctorId = doctors[1].Id,
                    RecipeId = recpies[0].Id,
                    PatientCardId = patientCards[0].Id 
                },

                new AppointmentModel
                {
                    DateOfApp = "17/05/2018",
                    Hour = "10:00",
                    IsConfirmed = 1,
                    Notes = "-----",
                    DoctorId = doctors[1].Id,
                    RecipeId = recpies[1].Id,
                    PatientCardId = patientCards[1].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "24/05/2018",
                    Hour = "10:30",
                    IsConfirmed = 1,
                    Notes = "------",
                    DoctorId = doctors[1].Id,
                    RecipeId = recpies[2].Id,
                    PatientCardId = patientCards[2].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "07/06/2018",
                    Hour = "11:00",
                    IsConfirmed = 1,
                    Notes = " ----- ",
                    DoctorId = doctors[1].Id,
                    RecipeId = recpies[3].Id,
                    PatientCardId = patientCards[3].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "22/05/2018",
                    Hour = "13:00",
                    IsConfirmed = 1,
                    Notes = "-----",
                    DoctorId = doctors[0].Id,
                    RecipeId = recpies[4].Id,
                    PatientCardId = patientCards[4].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "05/06/2018",
                    Hour = "13:30",
                    IsConfirmed = 1,
                    Notes ="-----",
                    DoctorId = doctors[0].Id,
                    RecipeId = recpies[5].Id,
                    PatientCardId = patientCards[5].Id
                },

                new AppointmentModel
                {
                    DateOfApp = "29/05/2018",
                    Hour = "14:25",
                    IsConfirmed = 1,
                    Notes = "-------",
                    DoctorId = doctors[0].Id,
                    //RecipeId = recpies[6].Id,
                    PatientCardId = patientCards[6].Id
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

            /* Diagnosis */

            var diagnosises = new DiagnosisModel[]
            {
                new DiagnosisModel
                {
                    Synopsis = "Pacjent przyszedl z kaszlem i katarem.",
                    Symptoms = "Kaszel, ból w płucach",
                    DeseaseName = "Zapalenie płuc",
                    AppointmentId = visits[0].Id
                },

                new DiagnosisModel
                {
                    Synopsis = "Pacjent ma gorączke i opuchnięte migdałki, nie dobrze z nim.",
                    Symptoms = "Ból gardła, gorączka, problemy z przełykaniem, powiekszone węzły chłonne.",
                    DeseaseName = "Angina",
                    AppointmentId = visits[1].Id
                },

                new DiagnosisModel
                {
                    Synopsis = "Pacjent przyszedł z bólem ucha i lekką gorączką.",
                    Symptoms = "Ból ucha, gorączka, problemy ze słuchem",
                    DeseaseName = "Zapalenie ucha środkowego",
                    AppointmentId = visits[2].Id
                },

                new DiagnosisModel
                {
                    Synopsis = "Pacjent ma lekkie bóle głowy",
                    Symptoms = "Gorączka, zawroty głowy, ból głowy z prawej strony",
                    DeseaseName = "Ropień mózgu",
                    AppointmentId = visits[3].Id
                },

                new DiagnosisModel
                {
                    Synopsis = "Pacjent przyszedl z kaszlem i katarem.",
                    Symptoms = "Kaszel, ból w płucach",
                    DeseaseName = "Zapalenie płuc",
                    AppointmentId = visits[4].Id
                },

                new DiagnosisModel
                {
                    Synopsis = "Pacjent przyszedł z krwawiącymi oczami i gorączką",
                    Symptoms = "Wysoka gorączka, podwyższone ciśnienie, krew z oczu",
                    DeseaseName = "Ebola",
                    AppointmentId = visits[5].Id
                },

               /* new DiagnosisModel
                {
                    //Synopsis = "Pacjent przyszedl z kaszlem i katarem.",
                   // Symptoms = "Kaszel, ból w płucach",
                    //DeseaseName = "Zapalenie płuc",
                    AppointmentId = visits[6].Id
                }*/
            };

            if (!context.DiagnosisModel.Any())
            {
                foreach (DiagnosisModel diagnosis in diagnosises)
                {
                    context.DiagnosisModel.Add(diagnosis);
                    context.SaveChanges();
                }
            }

            /* Examination */
            var exams = new ExaminationModel[]
            {
                new ExaminationModel
                {
                    NameOfExamination = "Prześwietlenie"
                },

                new ExaminationModel
                {
                    NameOfExamination = "----"
                },

                new ExaminationModel
                {
                    NameOfExamination = "Badanie ucha wewnętrznego"
                },

                new ExaminationModel
                {
                    NameOfExamination = "Rezonans magnetyczny"
                },

                new ExaminationModel
                {
                    NameOfExamination = "Prześwietlnenie"
                },

                new ExaminationModel
                {
                    NameOfExamination = "----"
                }

               /* new ExaminationModel
                {
                    NameOfExamination = "Prześwietlenie"
                }*/
            };

            if (!context.ExaminationModel.Any())
            {
                foreach (ExaminationModel examination in exams)
                {
                    context.ExaminationModel.Add(examination);
                    context.SaveChanges();
                }
            }

            /* Refferal */
            var refferals = new ReferralModel[]
            {
                new ReferralModel
                {
                    DateOfIssuance = "08/05/2018",
                    ExaminationId = exams[0].Id,
                    AppointmentId = visits[0].Id,
                },

                new ReferralModel
                {
                    DateOfIssuance = "----",
                    ExaminationId = exams[1].Id,
                    AppointmentId = visits[1].Id,
                },

                new ReferralModel
                {
                    DateOfIssuance = "22/05/2018",
                    ExaminationId = exams[2].Id,
                    AppointmentId = visits[2].Id,
                },

                new ReferralModel
                {
                    DateOfIssuance = "05/06/2018",
                    ExaminationId = exams[3].Id,
                    AppointmentId = visits[3].Id,
                },

                new ReferralModel
                {
                    DateOfIssuance = "24/05/2018",
                    ExaminationId = exams[4].Id,
                    AppointmentId = visits[4].Id,
                },

                new ReferralModel
                {
                    DateOfIssuance = "----",
                    ExaminationId = exams[5].Id,
                    AppointmentId = visits[5].Id,
                }

               /* new ReferralModel
                {
                   DateOfIssuance = "14/06/2018",
                  ExaminationId = exams[6].Id,
                   AppointmentId = visits[6].Id,
                }*/
            };

            if (!context.ReferralModel.Any())
            {
                foreach (ReferralModel refferal in refferals)
                {
                    context.ReferralModel.Add(refferal);
                    context.SaveChanges();
                }
            }

            /* Grade */

            var grades = new GradeModel[]
            {
                new GradeModel
                {
                    Grade = 1,
                    Comment = "Beznadzieja ... ",
                    AppointmentId = visits[0].Id
                },

                new GradeModel
                {
                    Grade = 5,
                    Comment = "Bardzo dobra i szybka obsługa",
                    AppointmentId = visits[1].Id
                },

                new GradeModel
                {
                    Grade = 3,
                    Comment = "Dobre rozpoznanie ale nie chciał wypisać L4",
                    AppointmentId = visits[2].Id
                },

                new GradeModel
                {
                    Grade = 5,
                    Comment = "Szybko umówiona wizyta uratowała mi życie",
                    AppointmentId = visits[3].Id
                },

                new GradeModel
                {
                    Grade = 1,
                    Comment = "Nietrafna diagnoza, nie polecam",
                    AppointmentId = visits[4].Id
                },

                new GradeModel
                {
                    Grade = 4,
                    Comment = "Szybko i dobrze",
                    AppointmentId = visits[5].Id
                }

               /* new GradeModel
                {
                   Grade = 2,
                   Comment = "Ostatnia deska ratunku.... tylko dlatego go był moim wyborem",
                    AppointmentId = visits[6].Id
                }*/
            };

            if (!context.GradeModel.Any())
            {
                foreach (GradeModel grade in grades)
                {
                    context.GradeModel.Add(grade);
                    context.SaveChanges();
                }
            }
        }
    }
}