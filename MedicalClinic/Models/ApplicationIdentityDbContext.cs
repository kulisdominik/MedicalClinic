/* Michał Dyrcz Base #1
    1. Podmienienie ApplicationUser na User
    2. Dodanie Database.EnsureCreated();
    3. Konfiguracja polaczen oraz przy pomocy buildera tabel zignorowanie autogenerowanych tabel przez aspnet i pol w tabeli User
    4. Stworzenie DbSet
*/
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models
{
    public class ApplicationIdentityDbContext : IdentityDbContext<User>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        // Configure the relationship stuff
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUser<string>>();

            modelBuilder.Entity<User>()
                // Ignore auto-generated stuff
                .Ignore(c => c.Id)
                .Ignore(c => c.AccessFailedCount)
                .Ignore(c => c.ConcurrencyStamp)
                .Ignore(c => c.EmailConfirmed)
                .Ignore(c => c.LockoutEnd)
                .Ignore(c => c.LockoutEnabled)
                .Ignore(c => c.NormalizedEmail)
                .Ignore(c => c.NormalizedUserName)
                .Ignore(c => c.PhoneNumberConfirmed)
                .Ignore(c => c.SecurityStamp)
                .Ignore(c => c.TwoFactorEnabled)
                .Ignore(c => c.UserName)

                
                // Relation User and Residence
                .HasOne<Residence>(s => s.Residence)
                .WithMany(g => g.Users)
                .HasForeignKey(s => s.ResidenceId);
        }

        // Make those entities
        public DbSet<Residence> Residences { get; set; }
        public new DbSet<User> Users { get; set; }

    }
}
