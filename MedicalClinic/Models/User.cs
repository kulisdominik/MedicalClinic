using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MedicalClinic.Models
{
    public class User : IdentityUser
    {
        //Private Key
        [Required] public int UserId { get; set; }

        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public override string Email { get; set; }


        public int ResidenceId { get; set; }
        public Residence Residence { get; set; }
    }
}
