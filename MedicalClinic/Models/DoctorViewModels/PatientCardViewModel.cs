using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.DoctorViewModels
{
    public class PatientCardViewModel
    {
        public string Id { get; set; }

        public string PatientId { get; set; }

        [Display(Name = "Data założenia")]
        public string Date { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "PESEL")]
        public string PIN { get; set; }

        [Display(Name = "Numer telefonu")]
        public string PhoneNum { get; set; }

        [Display(Name = "Płeć")]
        public string Sex { get; set; }

        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Display(Name = "Numer budynku")]
        public string BuildingNum { get; set; }

        [Display(Name = "Numer mieszkania")]
        public string FlatNum { get; set; }
    }
}
