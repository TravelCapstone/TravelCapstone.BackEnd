using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class CreateOptionsPrivateTourDto
{
    public Guid PrivateTourRequestId { get; set; }
    public List<OptionRequestDTO> OptionRequestDtos { get; set; } = new List<OptionRequestDTO>();
}

public class OptionRequestDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public OptionClass OptionClass { get; set; }
    public int Quantity { get; set; }
    public Guid ServiceId { get; set; }
}