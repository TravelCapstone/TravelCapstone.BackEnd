using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Material
{
    [Key] public Guid Id { get; set; }

    public MaterialType MaterialTypeId { get; set; }
    [ForeignKey(nameof(MaterialTypeId))]
    public Models.EnumModels.MaterialType? MaterialType { get; set; }
    public int Quantity { get; set; }
    public Guid DayPlanId { get; set; }

    [ForeignKey(nameof(DayPlanId))] public DayPlan? DayPlan { get; set; }
}