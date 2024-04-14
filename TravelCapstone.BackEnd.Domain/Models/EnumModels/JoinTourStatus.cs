using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class JoinTourStatus
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.JoinTourStatus Id { get; set; }
    public string Name { get; set; } = null!;
}