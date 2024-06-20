using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response;

public class TourDetail
{
    public Tour Tour { get; set; }
    public List<DayPlanDto> DayPlanDtos { get; set; } = new List<DayPlanDto>();
    public List<MaterialAssignment> Materials { get; set; } = new List<MaterialAssignment>();
    public List<string> Imgs { get; set; } = new List<string>();
}

public class DayPlanDto
{
    public DayPlan DayPlan { get; set; }
    public List<Route> Routes { get; set; }
}