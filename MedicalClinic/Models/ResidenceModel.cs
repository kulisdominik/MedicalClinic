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
        /*[Required] dodać do skonczonej bazy*/

        [Required]
        public int Id { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string BuildingNum { get; set; }
        public string FlatNum { get; set; }
       
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }
}
