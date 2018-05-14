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
            
            /*Doctor |---< WorkHours*/
            builder.Entity<DoctorModel>()
                .HasMany<WorkHoursModel>(g => g.WorkHours)
                .WithOne(s => s.DoctorModel)
                .HasForeignKey(s => s.DoctorId);

            /*Clerk |---< PatientCard*/
            builder.Entity<ClerkModel>()
                .HasMany<PatientCardModel>(g => g.PatientCards)
                .WithOne(s => s.ClerkModel)
                .HasForeignKey(s => s.ClerkId);

            /*Patient | --- | PatientCard*/
            builder.Entity<PatientCardModel>()
                .HasOne(u => u.Patient)
                .WithOne(d => d.PatientCard)
                .HasForeignKey<PatientCardModel>(u => u.PatientId);

            /*Appointment >---| Doctor*/
            builder.Entity<DoctorModel>()
                .HasMany<AppointmentModel>(g => g.AppointmentModel)
                .WithOne(s => s.DoctorModel)
                .HasForeignKey(s => s.DoctorId);

            /*Appoinment >---| PatientCard*/
            builder.Entity<PatientCardModel>()
                .HasMany<AppointmentModel>(g => g.AppointmentModel)
                .WithOne(s => s.PatientCardModel)
                .HasForeignKey(s => s.PatientCardId);

            /* Appointment |---| Diagnosis */
            builder.Entity<AppointmentModel>()
                .HasOne(u => u.DiagnosisModel)
                .WithOne(d => d.AppointmentModel)
                .HasForeignKey<AppointmentModel>(u => u.DiagnosisId);

            /* Appointment |---< Referral */
            builder.Entity<AppointmentModel>()
                .HasMany<ReferralModel>(g => g.ReferralModel)
                .WithOne(s => s.Appointment)
                .HasForeignKey(s => s.AppointmentId);

            /* Referral |---| Examination*/
            builder.Entity<ReferralModel>()
                .HasOne(u => u.ExaminationModel)
                .WithOne(d => d.ReferralModel)
                .HasForeignKey<ReferralModel>(s => s.ExaminationId);

            /* Appointment |---| Recipe*/
            builder.Entity<AppointmentModel>()
                .HasOne(u => u.RecipeModel)
                .WithOne(d => d.AppointmentModel)
                .HasForeignKey<AppointmentModel>(s => s.RecipeId);

            /* Medicine >---| Recipe */
            builder.Entity<RecipeModel>()
                .HasMany<MedicineModel>(g => g.MedicineModel)
                .WithOne(s => s.RecipeModel)
                .HasForeignKey(s => s.RecipeId);

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ResidenceModel> ResidenceModel { get; set; }
        public DbSet<DoctorModel> DoctorModel { get; set; }
        public DbSet<AdminModel> AdminModel { get; set; }
        public DbSet<PatientModel> PatientModel { get; set; }
        public DbSet<ClerkModel> ClerkModel { get; set; }
        public DbSet<WorkHoursModel> WorkHours { get; set; }
        public DbSet<PatientCardModel> PatientCardModel { get; set; }
        public DbSet<AppointmentModel> AppointmentModel { get; set; }
        public DbSet<ReferralModel> ReferralModel { get; set; }
        public DbSet<ExaminationModel> ExaminationModel { get; set; }
        public DbSet<RecipeModel> RecipeModel { get; set; }
        public DbSet<MedicineModel> MedicineModel { get; set; }
    }
}
