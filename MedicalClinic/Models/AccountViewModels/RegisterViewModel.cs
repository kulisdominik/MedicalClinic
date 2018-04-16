using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Podano nieprawidłowy adres e-mail.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} musi mieć co najmniej {2} znaków i być krótsze niż {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("Password", ErrorMessage = "Podane hasła się nie zgadzają.")]
        public string ConfirmPassword { get; set; }

    }
}
