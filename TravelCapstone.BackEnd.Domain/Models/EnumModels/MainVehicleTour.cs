using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class MainVehicleTour
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.MainVehicleTour Id { get; set; }
    public string Name { get; set; } = null!;
}