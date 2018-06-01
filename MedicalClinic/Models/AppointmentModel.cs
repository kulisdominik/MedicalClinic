using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("Appointment")]
    public class AppointmentModel
    {
        [Required] public string Id { get; set; }
        public string DateOfApp { get; set; }
        public string Notes { get; set; }

        public string DoctorId { get; set; }
        public DoctorModel DoctorModel { get; set; }

        public string PatientCardId { get; set; }
        public PatientCardModel PatientCardModel { get; set; }

        public virtual DiagnosisModel DiagnosisModel { get; set; }

        public ICollection<ReferralModel> ReferralModel { get; set; }

        public string RecipeId { get; set; }
        public virtual RecipeModel RecipeModel { get; set; }

        public virtual GradeModel GradeModel { get; set; }
    }
}
