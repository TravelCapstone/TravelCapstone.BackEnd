using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class ServiceCostHistory
{
    [Key] public Guid Id { get; set; }

    public double Price { get; set; }
    public double RedundancyCost { get; set; }
    public int MOQ { get; set; }
    public DateTime Date { get; set; }
    public Guid? FacilityServiceId { get; set; }
    [ForeignKey(nameof(FacilityServiceId))] public FacilityService? FacilityService { get; set; }

    public Guid? MenuId { get; set; }
    [ForeignKey(nameof(MenuId))] public Menu? Menu { get; set; }
    public Guid? TransportServiceDetailId { get; set; }
    [ForeignKey(nameof(TransportServiceDetailId))]
    public TransportServiceDetail? Transport { get; set;}
    
}