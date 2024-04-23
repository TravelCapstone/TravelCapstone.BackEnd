using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Menu
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Enum.DietaryPreference DietaryPreferenceId { get; set; }
        [ForeignKey(nameof(DietaryPreferenceId))]
        public Models.EnumModels.DietaryPreference? DietaryPreference { get; set; }
        public Guid FoodServiceId { get; set; }
        [ForeignKey(nameof(FoodServiceId))] public Service? Service { get; set; }
    }
}
