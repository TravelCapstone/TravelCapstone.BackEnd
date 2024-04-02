using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services;

public class TourService : GenericBackendService, ITourService
{
    private readonly IRepository<Tour> _repository;

    public TourService(IServiceProvider serviceProvider, IRepository<Tour> repository) : base(serviceProvider)
    {
        _repository = repository;
    }

    public async Task<AppActionResult> GetById(Guid id)
    {
        var result = new AppActionResult();
        try
        {
            var dayPlanRepository = Resolve<IRepository<DayPlan>>();
            var routeRepository = Resolve<IRepository<Route>>();
            var materialRepository = Resolve<IRepository<Material>>();
            var detail = new TourDetail();
            detail.DayPlanDtos = new List<DayPlanDto>();
            var tour = await _repository.GetById(id);
            detail.Tour = tour;

            var listPlan = await dayPlanRepository!.GetAllDataByExpression(a => a.TourId == id, 0, 0, null);
            foreach (var item in listPlan.Items!)
            {
                var route = await routeRepository!.GetAllDataByExpression(a => a.DayPlanId == item.Id, 0, 0, a=> a.StartPoint!,a=> a.EndPoint!);
                var materials =
                    await materialRepository!.GetAllDataByExpression(a => a.DayPlanId == item.Id, 0, 0, null);
              detail.DayPlanDtos.Add(new DayPlanDto
                {
                    DayPlan = item,
                    Routes = route.Items!,
                    Materials = materials.Items!
                });
            }

            result.Result = detail;
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Lỗi đã xảy ra: {e.Message}");
        }

        return result;
    }
}