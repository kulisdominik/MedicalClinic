using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.PatientViewModels
{
    public class VisitHistoryViewModel
    {
        public string Id { get; set; }

        public string DateOfApp { get; set; }

        public string Hour { get; set; }

        public string DoctorFirstName { get; set; }

        public string DoctorLastName { get; set; }

        public string Specialization { get; set; }

        public int isConfirmed { get; set; }
    }
}
