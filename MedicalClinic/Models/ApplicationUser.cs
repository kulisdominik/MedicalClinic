using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser() 
            : base()
        {
            IsActive = true;
        }

        [Required]
        public override string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PIN { get; set; } // pesel

        public string PhoneNum { get; set; }

        [Required]
        public override string Email { get; set; }

        public string Sex { get; set; }

        public int ResidenceId { get; set; }

        public ResidenceModel residenceModel { get; set; }
        
        public virtual DoctorModel Doctor { get; set; }

        public virtual AdminModel Admin { get; set; }

        public virtual PatientModel Patient { get; set; }

        public virtual ClerkModel Clerk { get; set; }

        public bool IsActive { get; set; }
    }
}
