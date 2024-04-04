using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface ITourService
{
    Task<AppActionResult> GetById(Guid id);

    Task<AppActionResult> GetAll(string? keyWord, int pageNumber, int pageSize);
}