using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.PatientViewModels
{
    public class VisitDetailsViewModel
    {
        public string Id { get; set; }

        public string CardId { get; set; }

        public string DateOfApp { get; set; }

        public string Hour { get; set; }

        public string DoctorFirstName { get; set; }

        public string DoctorLastName { get; set; }

        public string Specialization { get; set; }

        public string Synopsis { get; set; }

        public string Symptoms { get; set; }

        public string DeseaseName { get; set; }

        public List<string> NameofMedicine { get; set; }

        public string ExpDate { get; set; }

        public string Descrpition { get; set; }

        public List<ReferralViewModel> Referral { get; set; }

        public string Comment { get; set; }

        public int Grade { get; set; }

    }
}
