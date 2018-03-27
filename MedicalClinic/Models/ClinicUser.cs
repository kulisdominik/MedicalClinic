using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using MedicalClinic.Data;
using Microsoft.AspNetCore.Identity;

namespace MedicalClinic.Models
{
    public class ClinicUser : IdentityUser
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
    }
}
