using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class TourType
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.TourType Id { get; set; }
    public string Name { get; set; } = null!;
}