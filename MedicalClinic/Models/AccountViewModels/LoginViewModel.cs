using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Wymagany adres e-mail.")]
        [EmailAddress(ErrorMessage = "Podano nieprawidłowy adres e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wymagane hasło.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}
