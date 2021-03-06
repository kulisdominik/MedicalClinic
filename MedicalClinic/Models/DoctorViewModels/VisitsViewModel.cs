﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.DoctorViewModels
{
    public class VisitsViewModel
    {
        public string Id { get; set; }

        public string CardId { get; set; }

        public string DateOfApp { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        public string Synopsis { get; set; }

        public string Symptoms { get; set; }

        public string DeseaseName { get; set; }

        public List<string> NameofMedicine { get; set; }

        public string ExpDate { get; set; }

        public string Descrpition { get; set; }

        public List<ReferralViewModel> Referral { get; set; }

        public bool Edit { get; set; }

    }
}
