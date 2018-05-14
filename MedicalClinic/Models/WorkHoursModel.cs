using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("WorkHours")]
    public class WorkHoursModel
    {
        [Required] public int Id { get; set; }
        public string DayofWeek { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }

        public int DoctorId { get; set; }
        public DoctorModel DoctorModel { get; set; }

    }
}
