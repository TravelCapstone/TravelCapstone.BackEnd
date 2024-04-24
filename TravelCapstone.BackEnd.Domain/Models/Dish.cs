using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Dish
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Enum.DishType DishTypeId { get; set; }
        [ForeignKey(nameof(DishTypeId))]
        public Models.EnumModels.DishType? DishType { get; set; }
    }
}
