using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Vehicle
{
    [Key] public Guid Id { get; set; }

    public Enum.VehicleType VehicleTypeId { get; set; }
    [ForeignKey(nameof(VehicleTypeId))]
    public EnumModels.VehicleType? VehicleType { get; set; }
    public string Plate { get; set; } = null!;
    public int Capacity { get; set; }
    public string EngineNumber { get; set; } = null!;
    public string ChassisNumber { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string Owner { get; set; } = null!;
    public string Color { get; set; } = null!;
}