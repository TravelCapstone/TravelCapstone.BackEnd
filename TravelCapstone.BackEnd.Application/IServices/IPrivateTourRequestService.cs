using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface IPrivateTourRequestService
{
    Task<AppActionResult> CreatePrivateTourRequest(PrivateTourRequestDTO privateTourequestDto);

    Task<AppActionResult> GetAllPrivateTourRequest(int pageNumber, int pageSize);

    Task<AppActionResult> GetPrivateTourRequestById(Guid id); 
    Task<AppActionResult> GetPrivateTourRequestByIdForCustomer(Guid id);

    Task<AppActionResult> CreateOptionsPrivateTour(CreateOptionsPrivateTourDto dto);

    Task<AppActionResult> ConfirmOptionPrivateTour(Guid optionId, string accountId);
    Task<AppActionResult> SendToCustomer(Guid privateTourRequestId);
   // Task<AppActionResult> GetServiceRatingListByServiceType(Guid provinceId, Domain.Enum.FacilityType serviceTypeId);
   Task<IActionResult> GetExcelQuotation(Guid privateTourRequestId);

}