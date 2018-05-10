using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("User")]
    public class ApplicationUser : IdentityUser<string>
    {
        [Required] public override string Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string PIN { get; set; } // pesel
        [Required] public string PhoneNum { get; set; }
        [Required] public override string Email { get; set; }
        [Required] public string Sex { get; set; }

        public int ResidenceId { get; set; }
        public ResidenceModel residenceModel { get; set; }
        
        public DoctorModel Doctor { get; set; }
        public AdminModel Admin { get; set; }
        public PatientModel Patient { get; set; }
        public ClerkModel Clerk { get; set; }
    }
}
