using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class OptionQuotation
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public OptionClass OptionClass { get; set; }
    public double Total { get; set; }
    public OptionQuotationStatus Status { get; set; }
    public Guid PrivateTourRequestId { get; set; }  

    [ForeignKey(nameof(PrivateTourRequestId))]
    public PrivateTourRequest? PrivateTourRequest { get; set; }
}