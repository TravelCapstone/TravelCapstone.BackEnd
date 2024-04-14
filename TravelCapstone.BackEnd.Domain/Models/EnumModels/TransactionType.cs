using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class TransactionType
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.TransactionType Id { get; set; }
    public string Name { get; set; } = null!;
}