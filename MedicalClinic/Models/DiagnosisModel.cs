using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("Diagnosis")]
    public class DiagnosisModel
    {
        [Required] public string Id { get; set; }
        public string Synopsis { get; set; }
        public string Symptoms { get; set; }
        public string DeseaseName { get; set; }

        public string AppointmentId { get; set; }
        public virtual AppointmentModel AppointmentModel { get; set; }
    }
}
