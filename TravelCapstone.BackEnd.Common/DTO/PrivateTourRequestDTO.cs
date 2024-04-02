using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO;

public class PrivateTourRequestDTO
{
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = null!;
    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public Guid TourId { get; set; }
    public VehicleType MainVehicle { get; set; }
    public bool isEnterprise { get; set; }
    public string? AccountId { get; set; }
}