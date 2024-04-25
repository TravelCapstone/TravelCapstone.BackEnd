using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models.BaseModel;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Menu:BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Enum.DietaryPreference DietaryPreferenceId { get; set; }
        [ForeignKey(nameof(DietaryPreferenceId))]
        public Models.EnumModels.DietaryPreference? DietaryPreference { get; set; }
        public Enum.MealType MealTypeId { get; set; }
        [ForeignKey(nameof(MealTypeId))]
        public Models.EnumModels.MealType? MealType { get; set; }
        public Guid FacilityServiceId { get; set; }
        [ForeignKey(nameof(FacilityServiceId))] public FacilityService? FacilityService { get; set; }
    }
}
