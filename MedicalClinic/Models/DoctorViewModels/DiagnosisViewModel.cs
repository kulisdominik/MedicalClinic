using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.DoctorViewModels
{
    public class DiagnosisViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Opis")]
        public string Synopsis { get; set; }

        [Display(Name = "Objawy")]
        public string Symptoms { get; set; }

        [Display(Name = "Nazwa choroby")]
        public string DeseaseName { get; set; }

        public string AppointmentId { get; set; }
    }
}
