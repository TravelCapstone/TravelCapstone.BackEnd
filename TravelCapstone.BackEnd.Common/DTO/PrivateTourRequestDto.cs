using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO;

public class PrivateTourRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int NumOfAdult { get; set; }
    public int NumOfChildren { get; set; }
    public PrivateTourStatus Status { get; set; }

    public Guid TourId { get; set; }
    public string? AccountId { get; set; }
    public AccountResponse? Account { get; set; }
}

