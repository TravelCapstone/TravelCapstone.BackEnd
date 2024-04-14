using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class OrderStatus
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.OrderStatus Id { get; set; }
    public string Name { get; set; } = null!;
}