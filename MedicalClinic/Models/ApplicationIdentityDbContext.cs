using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ClinicUser>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {

        }
    }
}
