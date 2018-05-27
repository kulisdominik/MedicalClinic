using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.DoctorViewModels
{
    public class CalendarViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Specialization { get; set; }

        public string DateOfApp { get; set; }

        public string Notes { get; set; }
    }
}
