using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class PrivateTourRequestDTO
{
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = null!;
    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public int NumOfDay { get; set; }
    public int NumOfNight { get; set; }
    public Guid TourId { get; set; }
    public VehicleType MainVehicle { get; set; }
    public bool isEnterprise { get; set; }
    public Guid MainDestinationId {  get; set; }
    public List<Guid>? OtherLocationIds { get; set; }
    public string? AccountId { get; set; }
}