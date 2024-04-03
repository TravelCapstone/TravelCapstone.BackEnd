using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface IPrivateTourRequestService
{
    Task<AppActionResult> CreatePrivateTourRequest(PrivateTourRequestDTO privateTourequestDto);
    Task<AppActionResult> GetAllTourPrivate(int pageNumber, int pageSize);
    Task<AppActionResult> CreateOptionsPrivateTour(CreateOptionsPrivateTourDto dto);

}