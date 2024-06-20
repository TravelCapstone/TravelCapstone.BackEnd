using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response;

public class TourDetail
{
    public Tour Tour { get; set; }
    public List<DayPlanDto> DayPlanDtos { get; set; } = new List<DayPlanDto>();
    public List<MaterialAssignment> Materials { get; set; } = new List<MaterialAssignment>();
    public List<string> Imgs { get; set; } = new List<string>();
    public List<TourLocationDto> Locations { get; set; } = new List<TourLocationDto>();
}

public class DayPlanDto
{
    public DayPlan DayPlan { get; set; }
    public List<Route> Routes { get; set; }
}

public class TourLocationDto
{
    public Guid ProvinceId { get; set; }
    public Guid DistrictId { get; set; }
    public string Address { get; set; }
}