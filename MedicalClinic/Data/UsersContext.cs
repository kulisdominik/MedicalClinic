using MedicalClinic.Models;
using Microsoft.EntityFrameworkCore;

// JAKBY KTOS COS NIE ROZUMIAL TO LINK: 
// https://docs.microsoft.com/pl-pl/aspnet/core/data/ef-mvc/intro


namespace MedicalClinic.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}
