using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models
{
    public class Residence
    {
        // Private key
        [Required] public int ResidenceId { get; set; }

        [Required] public string Country { get; set; }
        public string Street { get; set; }
        [Required] public string City { get; set; }
        [Required] public string PostalCode { get; set; }
        [Required] public int NumBuilding { get; set; }
        public int NumApartment { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
