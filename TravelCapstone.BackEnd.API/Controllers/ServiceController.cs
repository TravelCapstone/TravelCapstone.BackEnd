using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.API.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceService _service;
        public ServiceController(IServiceService serviceService)
        {
            _service = serviceService;
        }

        [HttpGet("get-service-by-province-id/{id}/{type}")]
        public async Task<AppActionResult> GetServiceByProvinceId(Guid id, ServiceType type)
        {
            return await _service.GetServiceByProvinceIdAndServiceType(id,type);
        }

        [HttpGet("get-service-by-province-id-and-tour-request-id/{provinceId}/{requestId}")]
        public async Task<AppActionResult> GetServiceByProvinceIdAndPrivateTourRequestId(Guid provinceId, Guid requestId)
        {
            return await _service.GetServiceByProvinceId(provinceId, requestId);
        }
    }
}
