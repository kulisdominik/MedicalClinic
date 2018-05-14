using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("Patient")]
    public class PatientModel
    {
        [Required] public string Id { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual PatientCardModel PatientCard { get; set; }
    }
}
