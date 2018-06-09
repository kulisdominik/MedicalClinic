using System;
using System.Collections.Generic;
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

        public string Content { get; set; }

        public string ReceiverEmail { get; set; }

        public bool ReceiverVisibility { get; set; }

        public string SenderEmail { get; set; }

        public bool SenderVisibility { get; set; }

        public DateTime Date { get; set; }
    }
}
