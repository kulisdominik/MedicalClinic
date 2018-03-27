using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Data
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Spec { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
