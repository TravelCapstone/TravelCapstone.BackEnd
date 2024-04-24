using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels
{
    public class Rating
    {
        [Key]
        public Enum.Rating Id {  get; set; }
        public string Name { get; set; } = null!;
        
    }
}
