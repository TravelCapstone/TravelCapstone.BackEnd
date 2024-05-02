using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response;

public class TourDetail
{
    public Tour Tour { get; set; }
    public List<DayPlanDto> DayPlanDtos { get; set; }
    public List<Material> Materials { get; set; }
}

public class DayPlanDto
{
    public DayPlan DayPlan { get; set; }
    public List<Route> Routes { get; set; }
}