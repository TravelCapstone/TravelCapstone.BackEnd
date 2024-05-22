using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class OptionQuotation
{
    [Key] public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public OptionClass OptionClassId { get; set; }
    [ForeignKey(nameof(OptionClassId))]
    public Models.EnumModels.OptionClass? OptionClass { get; set; }
    public double OrganizationCost { get; set; }
    public double ContingencyFee { get; set; }
    public double EscortFee { get; set; }
    public double OperatingFee { get; set; }
    public double DriverCost { get; set; }
    public double MinTotal { get; set; }
    public double MaxTotal { get; set; }
    public OptionQuotationStatus OptionQuotationStatusId { get; set; }
    [ForeignKey(nameof(OptionQuotationStatusId))]
    public Models.EnumModels.OptionQuotationStatus? OptionQuotationStatus { get; set; }
    public Guid PrivateTourRequestId { get; set; }

    [ForeignKey(nameof(PrivateTourRequestId))]
    public PrivateTourRequest? PrivateTourRequest { get; set; }

    public Guid AssurancePriceHistoryId { get; set; }
    [ForeignKey(nameof(AssurancePriceHistoryId))]
    public AssurancePriceHistory? AssurancePriceHistory { get; set;}
}