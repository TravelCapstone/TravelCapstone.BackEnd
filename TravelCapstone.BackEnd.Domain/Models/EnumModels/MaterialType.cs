using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class MaterialType
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.MaterialType Id { get; set; }
    public string Name { get; set; } = null!;
}