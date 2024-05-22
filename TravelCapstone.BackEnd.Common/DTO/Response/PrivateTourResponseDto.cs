using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response;

public class PrivateTourResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public int NumOfDay { get; set; }
    public int NumOfNight { get; set; }
    public PrivateTourStatus Status { get; set; }
    public bool IsEnterprise { get; set; }
    public string? RecommnendedTourUrl { get; set; }
    public string? Note { get; set; }
    public string? StartLocation { get; set; }
    public Guid StartLocationCommuneId { get; set; }
    public Commune StartLocationCommune { get; set; }
    public Province? MainDestination { get; set; } 
    public double WishPrice {  get; set; }
    public DietaryPreference DietaryPreference { get; set; }
    public List<RequestedLocation>? OtherLocation { get; set; }
    public List<RoomQuantityDetail>? RoomDetails { get; set; }
    public Guid TourId { get; set; }
    public Tour? Tour { get; set; }
    public string? AccountId { get; set; }
    public AccountResponse? Account { get; set; }
}