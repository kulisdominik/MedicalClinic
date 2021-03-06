﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.MessageViewModels
{
    public class MessageViewModel
    {
        [Display(Name ="Treść wiadomości")]
        public string Content { get; set; }

        [Display(Name ="Email odbiorcy")]
        [NotMapped]
        public List<SelectListItem> ReceiversEmail { get; set; }

        [Required]
        public string ReceiverEmail { get; set; }
    }
}
