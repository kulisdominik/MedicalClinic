using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models.MessageViewModels
{
    public class SendMessageViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public IEnumerable<SelectListItem> Receivers { get; set; }

        public string Sender { get; set; }
    }
}
