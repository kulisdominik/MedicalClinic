using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models
{
    public class MessageModel
    {
        #region Constructor
        public MessageModel() : base()
        {
            TimeSending = DateTime.Now;
        }
        #endregion
        #region Fields
        public string Id { get; set; }

        public string Content { get; set; }

        public string SenderEmail { get; set; }

        public string ReceiverEmail { get; set; }

        public DateTime TimeSending { get; }
        #endregion
    }
}
