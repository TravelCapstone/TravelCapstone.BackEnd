﻿using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("facility-service")]
    public class FacilityServiceController : Controller
    {
        private readonly IFacilityServiceService _service;
        public FacilityServiceController(IFacilityServiceService serviceService)
        {
            _service = serviceService;
        }

        [HttpGet("get-service-by-province-id/{id}/{type}")]
        public async Task<AppActionResult> GetServiceByProvinceId(Guid id, ServiceType type, int pageNumber=1, int pageSize = 10)
        {
            return await _service.GetServiceByProvinceIdAndServiceType(id, type,pageNumber,pageSize);
        }

        [HttpGet("get-service-price-range-by-province-id-and-tour-request-id/{provinceId}/{requestId}")]
        public async Task<AppActionResult> GetServicePriceRangeByDistrictIdAndRequestId(Guid provinceId, Guid requestId, int pageNumber=1, int pageSize=10)
        {
            return await _service.GetServicePriceRangeByDistrictIdAndRequestId(provinceId, requestId, pageNumber, pageSize);
        }
        [HttpGet("get-service-by-facilityId/{facilityId}")]
        public async Task<AppActionResult> GetServiceByFacilityId(Guid facilityId, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetServiceByFacilityId(facilityId, pageNumber,pageSize);
        }
        [HttpGet("get-list-service-for-plan/{privateTourRequestId}/{quotationDetailId}")]
        public async Task<AppActionResult> GetListServiceForPlan(Guid privateTourRequestId, Guid quotationDetailId, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetListServiceForPlan(privateTourRequestId, quotationDetailId, pageNumber, pageSize);
        }

        [HttpGet("get-all-facility-rating")]
        public async Task<AppActionResult> GetAllFacilityRating()
        {
            return await _service.GetAllFacilityRating();
        }
    }
}
