using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response;

public class PrivateTourResponeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Type { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public int NumOfDay { get; set; }
    public int NumOfNight { get; set; }
    public PrivateTourStatus Status { get; set; }
    public Province MainDestination { get; set; }
    public List<Province>? OtherLocation { get; set; }
    public Guid TourId { get; set; }
    public Tour? Tour { get; set; }
    public string? AccountId { get; set; }
    public AccountResponse? Account { get; set; }
}