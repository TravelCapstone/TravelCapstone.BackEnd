using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models;

public class Facility
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    public bool IsActive { get; set; }
    public string Address { get; set; }=null!;
    public string? PhoneNumber { get; set; }
    public Guid CommunceId { get; set; }

    [ForeignKey(nameof(CommunceId))]
    public Commune? Communce { get; set; }

    public Guid ServiceProviderId { get; set; }

    [ForeignKey(nameof(ServiceProviderId))]
    public ServiceProvider? ServiceProvider { get; set; }

    public Guid FacilityRatingId { get; set; }
    [ForeignKey(nameof(FacilityRatingId))]
    public FacilityRating? FacilityRating { get; set;}
}