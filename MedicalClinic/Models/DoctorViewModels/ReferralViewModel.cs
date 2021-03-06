﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.DoctorViewModels
{
    public class ReferralViewModel
    {
        public string DateOfIssuance { get; set; }

        [Display(Name = "Nazwa badania")]
        public string NameOfExamination { get; set; }

        public string AppointmentId { get; set; }
    }
}
