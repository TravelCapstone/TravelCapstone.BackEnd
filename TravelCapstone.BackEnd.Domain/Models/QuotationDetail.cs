using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class QuotationDetail
{
    [Key] public Guid Id { get; set; }

    public int Quantity { get; set; }
    public Guid SellPriceHistoryId { get; set; }

    [ForeignKey(nameof(SellPriceHistoryId))]
    public SellPriceHistory? SellPriceHistory { get; set; }

    public Guid OptionQuotationId { get; set; }

    [ForeignKey(nameof(OptionQuotationId))]
    public OptionQuotation? OptionQuotation { get; set; }
}