using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO;

public class ServiceProviderDto
{
    public Guid? Id { get; set; }

    public string Name { get; set; } = null!;
    public List<ServiceDto> Services { get; set; } = new();
}

public class ServiceDto
{
    public Guid? Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public FacilityType Type { get; set; }
    public string Address { get; set; } = null!;
    public Guid CommunceId { get; set; }
    public Guid? ServiceProviderId { get; set; }
}