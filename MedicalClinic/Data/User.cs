using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        public virtual List<Doctor> Doctors { get; set; }
    }
}
