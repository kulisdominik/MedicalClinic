/*Michał Drycz - Base #1 Update
    W public void ConfigureServices(IServiceCollection services) zamieniłem 'ApplicationUser' na 'User'
            services.AddIdentity<ApplicationUser, IdentityRole>()
                na
            services.AddIdentity<User, IdentityRole>()
        }*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalClinic.Data.Migrations;
using MedicalClinic.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalClinic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

<<<<<<< HEAD
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
=======
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
>>>>>>> origin/master
                .AddDefaultTokenProviders();

            services.AddMvc();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ElevatedRights", policy =>
            //        policy.RequireRole("Administrator", "Receptionist", "Doctor"));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
