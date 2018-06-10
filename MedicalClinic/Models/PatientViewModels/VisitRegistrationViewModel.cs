using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.PatientViewModels
{
    public class VisitRegistrationViewModel
    {
        public string Id { get; set; }

        public string CardId { get; set; }

        public string SelectedDate { get; set; }

        public string Specialization { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DayofWeek { get; set; }

        public string StartHour { get; set; }

        public string EndHour { get; set; }

        public List<GradeModel> Grade { get; set; }

        public double Average { get; set; }
    }
}
