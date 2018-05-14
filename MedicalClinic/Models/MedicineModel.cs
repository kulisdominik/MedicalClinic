using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("Medicine")]
    public class MedicineModel
    {
        [Required] public string Id { get; set; }
        public string Name { get; set; }

        public string RecipeId { get; set; }
        public RecipeModel RecipeModel { get; set; }
    }
}
