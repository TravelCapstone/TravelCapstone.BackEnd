using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels
{
    public class DishType
    {
        [Key]
        public TravelCapstone.BackEnd.Domain.Enum.DishType Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
