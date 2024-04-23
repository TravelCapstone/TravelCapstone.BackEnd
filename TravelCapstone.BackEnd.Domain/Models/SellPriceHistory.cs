using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class SellPriceHistory
{
    [Key] public Guid Id { get; set; }
    public int MOQ { get; set; }
    public double Price { get; set; }
    public DateTime Date { get; set; }
    public Guid FacilityServiceId { get; set; }

    [ForeignKey(nameof(FacilityServiceId))] public FacilityService? FacilityService { get; set; }
}