using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.ClerkViewModels
{
    public class VisitsToConfirmViewModel
    {
        public string Id { get; set; }

        public string DateOfApp { get; set; }

        public string Hour { get; set; }

        public string DoctorFirstName { get; set; }

        public string DoctorLastName { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        public string Specialization { get; set; }
    }
}
