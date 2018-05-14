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

            string[] roleNames = { "Admin", "Manager", "Member" };

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

            var adminResidence = new ResidenceModel
            {
                Id = 1
            };

            var userAdmin = new ApplicationUser
            {
                UserName = "test@admin.pl",
                Email = "test@admin.pl",
                EmailConfirmed = true,
                //FirstName = "Tester",
                //LastName = "Manualny",
                //PIN = "12345678934",
                //PhoneNum = "997998999",
                //Sex = "Male",
                ResidenceId = 1
            };

            //context.Database.
            //context.ResidenceModel.Add(adminResidence);
            //context.SaveChanges();

            string userPassword = "P@ssw0rd";
            
            if(await userManager.FindByEmailAsync("test@admin.pl") == null)
            {
                var success = await userManager.CreateAsync(userAdmin, userPassword);
                if (success.Succeeded)
                {
                    await userManager.AddToRoleAsync(userAdmin, "Admin");
                }
            }
        }
    }
}
