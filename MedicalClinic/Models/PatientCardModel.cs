using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{   [Table("PatientCard")]
    public class PatientCardModel
    {
        [Required] public string Id { get; set; }
        public string Date { get; set; }

        public string PatientId { get; set; }
        public virtual PatientModel Patient { get; set; }

        public string ClerkId { get; set; }
        public ClerkModel ClerkModel { get; set; }

        public ICollection<AppointmentModel> AppointmentModel { get; set; }
    }
}
