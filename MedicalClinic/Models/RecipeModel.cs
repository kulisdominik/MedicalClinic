using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinic.Models
{
    [Table("Recipe")]
    public class RecipeModel
    {
        [Required] public string Id { get; set; }
        public string ExpDate { get; set; }
        public string Descrpition { get; set; }

        public virtual AppointmentModel AppointmentModel { get; set; }

        public ICollection<MedicineModel> MedicineModel { get; set; }
    }
}
