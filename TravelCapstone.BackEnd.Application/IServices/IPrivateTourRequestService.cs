﻿using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface IPrivateTourRequestService
{
    Task<AppActionResult> CreatePrivateTourRequest(PrivateTourRequestDTO privateTourequestDto);

    Task<AppActionResult> GetAllTourPrivate(int pageNumber, int pageSize);

    Task<AppActionResult> GetPrivateTourRequestById(Guid id);

    Task<AppActionResult> CreateOptionsPrivateTour(CreateOptionsPrivateTourDto dto);

    Task<AppActionResult> ConfirmOptionPrivateTour(Guid optionId, string accountId);
    Task<AppActionResult> GetServiceRatingListByServiceType(Guid communeId, Domain.Enum.ServiceType serviceTypeId);

    Task<AppActionResult> GetServicePriceRangeOfCommune(Guid communeId, Guid serviceRatingId, int adultQuantity);
}