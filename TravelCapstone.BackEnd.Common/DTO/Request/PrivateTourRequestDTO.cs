using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class PrivateTourRequestDTO
{
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreateAt { get; set; }
    public string Description { get; set; } = null!;
    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public Guid TourId { get; set; }
    public VehicleType MainVehicle { get; set; }
    public bool isEnterprise { get; set; }
    public List<Province>? RequestedLocations { get; set; }
    public string? AccountId { get; set; }
}