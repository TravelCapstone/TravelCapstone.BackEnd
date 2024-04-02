using TravelCapstone.BackEnd.Common.DTO;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface ITourPrivateService
{
    Task<AppActionResult> GetAllTourPrivate(int pageNumber,int pageSize);
}