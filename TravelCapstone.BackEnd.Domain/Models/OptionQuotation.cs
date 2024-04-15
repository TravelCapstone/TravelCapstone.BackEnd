using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class OptionQuotation
{
    [Key] public Guid Id { get; set; }
    public OptionClass OptionClassId { get; set; }
    [ForeignKey(nameof(OptionClassId))]
    public Models.EnumModels.OptionClass? OptionClass { get; set; }
    public double MinTotal { get; set; }
    public double MaxTotal { get; set; }
    public OptionQuotationStatus OptionQuotationStatusId { get; set; }
    [ForeignKey(nameof(OptionQuotationStatusId))]
    public Models.EnumModels.OptionQuotationStatus? OptionQuotationStatus { get; set; }
    public Guid PrivateTourRequestId { get; set; }

    [ForeignKey(nameof(PrivateTourRequestId))]
    public PrivateTourRequest? PrivateTourRequest { get; set; }
}