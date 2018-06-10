using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.DoctorViewModels
{
    public class RecipeViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Wymagana data ważności")]
        [Display(Name = "Data ważności")]
        public string ExpDate { get; set; }

        [Display(Name = "Opis")]
        public string Descrpition { get; set; }

        public string AppointmentId { get; set; }

        [Required(ErrorMessage = "Wymagane lekarstwa")]
        [Display(Name = "Lekarstwa - wprowadź nazwy oddzielone przecinkiem")]
        public string NameOfMedicine { get; set; }
    }
}
