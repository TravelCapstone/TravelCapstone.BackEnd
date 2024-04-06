using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Request;

public class CreateOptionsPrivateTourDto
{
    public Guid PrivateTourRequestId { get; set; }
    public  Option? Option1 { get; set; }
    public Option? Option2 { get; set; }
    public Option? Option3 { get; set; } 
}


public class Option
{
    public string Name { get; set; } = null!;
    public OptionClass OptionClass { get; set; }
    public List<ServiceOptionDto> Services { get; set; } = new List<ServiceOptionDto>();

}

public class ServiceOptionDto
{
    public int Quantity { get; set; }
    public Guid ServiceId { get; set; }
}