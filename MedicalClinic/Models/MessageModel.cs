using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models
{
    public class MessageModel
    {
        public MessageModel() : base ()
        {
            this.ReceiverVisibility = true;
            this.SenderVisibility = true;
        }

        public string Id { get; set; }
        
        [Display(Name ="Treść")]
        public string Content { get; set; }

        [Display(Name ="Email odbiorcy")]
        public string ReceiverEmail { get; set; }
        
        public bool ReceiverVisibility { get; set; }
        
        [Display(Name ="Email nadawcy")]
        public string SenderEmail { get; set; }

        public bool SenderVisibility { get; set; }

        [Display(Name ="Data i godzina")]
        public DateTime Date { get; set; }
    }
}
