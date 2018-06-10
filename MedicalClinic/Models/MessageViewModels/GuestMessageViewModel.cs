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

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        
        [EmailAddress]
        [Display(Name ="Email")]
        public string SenderEmail { get; set; }

        [Display(Name = "Temat wiadomości")]
        public string Topic { get; set; }

        [Display(Name = "Treść wiadomości")]
        public string Content { get; set; }

        
    }
}
