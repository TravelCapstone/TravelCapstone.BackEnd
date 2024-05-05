using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelCapstone.BackEnd.Domain.Models;

public class RequestedLocation
{
    [Key]
    public Guid Id { get; set; }

    public Guid PrivateTourRequestId { get; set; }

    [ForeignKey(nameof(PrivateTourRequestId))]
    public PrivateTourRequest? PrivateTourRequest { get; set; }
    public string Address { get; set; } = string.Empty;
    //public Guid DistrictId { get; set; }

    //[ForeignKey(nameof(DistrictId))]
    //public District? District { get; set; }
    public Guid ProvinceId { get; set; }

    [ForeignKey(nameof(ProvinceId))]
    public Province? Province { get; set; }
}