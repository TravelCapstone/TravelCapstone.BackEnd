using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceService _service;
        public ServiceController(IServiceService serviceService)
        {
            _service = serviceService;
        }

        [HttpGet("get-service-by-province-id/{id}")]
        public async Task<AppActionResult> GetServiceByProvinceId(Guid id)
        {
            return await _service.GetServiceByProvinceId(id);
        }
    }
}
