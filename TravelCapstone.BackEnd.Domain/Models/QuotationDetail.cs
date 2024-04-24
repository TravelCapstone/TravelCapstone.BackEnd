using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Models.EnumModels;

namespace TravelCapstone.BackEnd.Domain.Models;

public class QuotationDetail
{
    [Key] public Guid Id { get; set; }
    public int QuantityOfAdult { get; set; }
    public int QuantityOfChild { get; set; }
    public Guid FacilityRatingId { get; set; }
    [ForeignKey(nameof(FacilityRatingId))]
    public FacilityRating? FacilityRating { get; set; }

    public Enum.ServiceType ServiceTypeId { get; set; }
    [ForeignKey(nameof(ServiceTypeId))]
    public ServiceType? ServiceType { get; set; }

    public DateTime? StartDate {  get; set; }    
    public DateTime? EndDate {  get; set; }
    public double MinPrice { get; set; }   
    public double MaxPrice { get; set; }
    public Guid OptionQuotationId { get; set; }
    [ForeignKey(nameof(OptionQuotationId))]
    public OptionQuotation? OptionQuotation { get; set; }

}