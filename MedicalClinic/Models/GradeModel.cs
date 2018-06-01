using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("Grade")]
    public class GradeModel
    {
        [Required] public string Id { get; set; }
        public string Comment { get; set; }
        public int Grade { get; set; }
       
        public string AppointmentId { get; set; }
        public virtual AppointmentModel AppointmentModel { get; set; }

    }
}
