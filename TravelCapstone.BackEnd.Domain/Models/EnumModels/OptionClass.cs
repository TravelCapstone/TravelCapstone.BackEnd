using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class OptionClass
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.OptionClass Id { get; set; }
    public string Name { get; set; } = null!;
}