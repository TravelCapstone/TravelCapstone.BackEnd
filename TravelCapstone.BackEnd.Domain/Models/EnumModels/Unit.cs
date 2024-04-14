using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class Unit
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.Unit Id { get; set; }
    public string Name { get; set; } = null!;
}