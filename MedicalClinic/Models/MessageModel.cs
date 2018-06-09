using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models
{
    public class MessageModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string ReceiverEmail { get; set; }

        public string SenderEmail { get; set; }

        public DateTime Date { get; set; }
    }
}
