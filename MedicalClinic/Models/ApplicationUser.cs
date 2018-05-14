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
        [Display(Name = "Nazwa użytkownika")]
        public override string UserName { get; set; }

        [Display(Name = "Email")]
        public override string Email { get; set; }

        [Display(Name = "Potw.")]
        public override bool EmailConfirmed { get; set; }

    }
}
