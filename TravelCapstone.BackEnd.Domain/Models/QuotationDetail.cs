using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class QuotationDetail
{
    [Key] public Guid Id { get; set; }
    public int QuantityOfAdult { get; set; }
    public int QuantityOfChild { get; set; }
    public Guid ServiceRatingId { get; set; }
    [ForeignKey(nameof(ServiceRatingId))]
    public ServiceRating? ServiceRating { get; set; }
    public int MinPrice { get; set; }   
    public int MaxPrice { get; set; }
    public Guid OptionQuotationId { get; set; }
    [ForeignKey(nameof(OptionQuotationId))]
    public OptionQuotation? OptionQuotation { get; set; }

    public TransportInformation? FlightInformation { get; set; }
}