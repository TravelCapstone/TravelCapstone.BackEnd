using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class TourStatus
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.TourStatus Id { get; set; }
    public string Name { get; set; } = null!;
}