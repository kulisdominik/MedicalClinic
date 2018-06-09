using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        [StringLength(30, ErrorMessage = "{0} może mieć maksymalnie {1} znaków")]
        [Display(Name ="Imię")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "{0} może mieć maksymalnie {1} znaków")]
        [Display(Name ="Nazwisko")]
        public string LastName { get; set; }

        [StringLength(11, ErrorMessage ="{0} musi mieć dokładnie {1} znaków")]
        [Display(Name ="PESEL")]
        public string PIN { get; set; }

        [Phone]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required(ErrorMessage = "E-mail jest wymagany.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name ="Płeć")]
        public string Sex { get; set; }
        
        [StringLength(30, ErrorMessage ="{0} może mieć maksymalnie {1} znaków")]
        [Display(Name ="Kraj")]
        public string Country { get; set; }

        [StringLength(50, ErrorMessage = "{0} może mieć maksymalnie {1} znaków")]
        [Display(Name ="Miasto")]
        public string City { get; set; }

        [StringLength(50, ErrorMessage = "{0} może mieć maksymalnie {1} znaków")]
        [Display(Name ="Ulica")]
        public string Street { get; set; }

        [Display(Name ="Numer budynku")]
        public string BuildingNum { get; set; }

        [Display(Name ="Numer mieszkania")]
        public string FlatNum { get; set; }

        [StringLength(6, ErrorMessage ="{0} musi mieć co najmniej {2} znaków i może mieć maksymalnie {1} znaków", MinimumLength = 5)]
        [Display(Name ="Kod pocztowy")]
        public string PostalCode { get; set; }

        public string StatusMessage { get; set; }
    }
}
