using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("Examination")]
    public class ExaminationModel
    {
        [Required] public string Id { get; set; }
        public string NameOfExamination { get; set; }

        public virtual ReferralModel ReferralModel { get; set; }
    }
}
