using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("Referral")]
    public class ReferralModel
    {
        [Required] public string Id { get; set; }
        public string DateOfIssuance { get; set; }

        public string AppointmentId { get; set; }
        public AppointmentModel Appointment { get; set; }

        public string ExaminationId { get; set; }
        public virtual ExaminationModel ExaminationModel { get; set; }
    }
}
