using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("Residence")]
    public class ResidenceModel
    {
        [Required] public int Id { get; set; }
        [Required] public string Country { get; set; }
        public string Street { get; set; }
        [Required] public string City { get; set; }
        [Required] public string PostalCode { get; set; }
        [Required] public string BuildingNum { get; set; }
        public string FlatNum { get; set; }
       
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }
}
