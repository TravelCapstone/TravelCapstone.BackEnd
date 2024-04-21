using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class PrivateTourRequestDTO
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = null!;
    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public int NumOfDay { get; set; }
    public int NumOfNight { get; set; }
    public string? StartLocation { get; set; }
    public Guid? StartCommuneId { get; set; }
    public Guid TourId { get; set; }
    public string? RecommnendedTourUrl { get; set; }
    public string? Note { get; set; }
    public bool IsEnterprise { get; set; }
    public Guid MainDestinationId { get; set; }
    public List<RequestedLocationDTO>? OtherLocation { get; set; }
    public string? AccountId { get; set; }
}

public class RequestedLocationDTO
{
    public string Address { get; set; }
    public Guid ProvinceId { get; set; }
}