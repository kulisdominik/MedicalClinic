using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("Doctor")]
    public class DoctorModel
    {
        [Required] public string Id { get; set; }
        [Required] public string Specialization { get; set; }

        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<WorkHoursModel> WorkHours { get; set; }

    }
}
