using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Service
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid ServiceRatingId { get; set; }

    [ForeignKey(nameof(ServiceRatingId))]
    public ServiceRating? ServiceRating { get; set; }

    public ServiceAvailability ServiceAvailabilityId { get; set; }
    [ForeignKey(nameof(ServiceAvailabilityId))]
    public Models.EnumModels.ServiceAvailability? ServiceAvailability {get;set;}
    public bool IsActive { get; set; }
    public string Address { get; set; }=null!;
    public int ServingQuantity { get; set; }
    public double SurchargePercent { get; set; } = 0;
    public Guid CommunceId { get; set; }

    [ForeignKey(nameof(CommunceId))]
    public Commune? Communce { get; set; }

    public Guid ServiceProviderId { get; set; }

    [ForeignKey(nameof(ServiceProviderId))]
    public ServiceProvider? ServiceProvider { get; set; }
}