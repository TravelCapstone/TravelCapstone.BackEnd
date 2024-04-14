using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class PrivateTourStatus
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.PrivateTourStatus Id { get; set; }
    public string Name { get; set; } = null!;
}