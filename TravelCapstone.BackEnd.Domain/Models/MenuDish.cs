using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class MenuDish
    {
        public Guid Id { get; set; }
        public Guid MenuId { get; set; }
        [ForeignKey(nameof(MenuId))] 
        public Menu? Menu { get; set; }

        public Guid DishId { get; set; }
        [ForeignKey(nameof(DishId))]
        public Dish? Dish { get; set; }
    }
}
