using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface ITourService
{
    Task<AppActionResult> GetById(Guid id);

    Task<AppActionResult> GetAll(string? keyWord, int pageNumber, int pageSize);
    Task<AppActionResult> RegisterTour(TourRegistrationDto dto);
    Task<AppActionResult> CreateTour(CreatePlanDetailDto dto);
    Task<AppActionResult> GetPlanByTour(Guid tourId);

}