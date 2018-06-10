using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.MessageViewModels
{
    public class GuestMessageViewModel
    {
        [Key]
        public string Id { get; set; }

        public string Content { get; set; }

        [EmailAddress]
        public string SenderEmail { get; set; }
    }
}
