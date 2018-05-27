using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MedicalClinic.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
        [Required] public override string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PIN { get; set; } // pesel
        public string PhoneNum { get; set; }
        [Required] public override string Email { get; set; }
        public string Sex { get; set; }

        public int ResidenceId { get; set; }
        public ResidenceModel ResidenceModel { get; set; }

        public virtual DoctorModel Doctor { get; set; }
        public virtual AdminModel Admin { get; set; }
        public virtual PatientModel Patient { get; set; }
        public virtual ClerkModel Clerk { get; set; }
    }
}
