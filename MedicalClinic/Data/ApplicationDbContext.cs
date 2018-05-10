using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalClinic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.Data.Migrations
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            /* Residence |- - -< User */
            builder.Entity<ResidenceModel>()
                .HasMany<ApplicationUser>(g => g.ApplicationUsers)
                .WithOne(s => s.residenceModel)
                .HasForeignKey(s => s.ResidenceId);

            /* User |---| Doctor*/
            builder.Entity<DoctorModel>()
                .HasOne(u => u.ApplicationUser)
                .WithOne(d => d.Doctor)
                .HasForeignKey<DoctorModel>(u => u.UserId);

            /* User |---| Admin*/
            builder.Entity<AdminModel>()
                .HasOne(u => u.ApplicationUser)
                .WithOne(d => d.Admin)
                .HasForeignKey<AdminModel>(u => u.UserId);

            /* User |---| Patient*/
            builder.Entity<PatientModel>()
                .HasOne(u => u.ApplicationUser)
                .WithOne(d => d.Patient)
                .HasForeignKey<PatientModel>(u => u.UserId);

            /* User |---| Clerk*/
            builder.Entity<ClerkModel>()
                .HasOne(u => u.ApplicationUser)
                .WithOne(d => d.Clerk)
                .HasForeignKey<ClerkModel>(u => u.UserId);

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ResidenceModel> ResidenceModel { get; set; }
        public DbSet<DoctorModel> DoctorModel { get; set; }
        public DbSet<AdminModel> AdminModel { get; set; }
        public DbSet<PatientModel> PatientModel { get; set; }
        public DbSet<ClerkModel> ClerkModel { get; set; }
    }
}
