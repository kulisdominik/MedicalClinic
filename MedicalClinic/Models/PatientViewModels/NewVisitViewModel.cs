using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.PatientViewModels
{
    public class NewVisitViewModel
    {
        public string Id { get; set; }

        public string CardId { get; set; }

        public string SelectedDate { get; set; }

        public string SelectedHour { get; set; }

        public List<string> Hours { get; set; }

        public List<GradeModel> Grade { get; set; }
    }
}
