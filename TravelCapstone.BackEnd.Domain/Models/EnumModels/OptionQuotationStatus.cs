using System.ComponentModel.DataAnnotations;

namespace TravelCapstone.BackEnd.Domain.Models.EnumModels;

public class OptionQuotationStatus
{
    [Key]
    public TravelCapstone.BackEnd.Domain.Enum.OptionQuotationStatus Id { get; set; }
    public string Name { get; set; } = null!;
}