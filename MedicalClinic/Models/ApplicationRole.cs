using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalClinic.Models
{
    public class ApplicationRole : IdentityRole<string>
    {
        public ApplicationRole()
            : base()
        {
        }

        public ApplicationRole(string roleName) 
            : base(roleName)
        {
        }
    }
}
