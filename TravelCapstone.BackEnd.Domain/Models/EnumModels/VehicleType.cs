using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class VehicleType
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.VehicleType Id { get; set; }
    public string Name { get; set; } = null!;
}