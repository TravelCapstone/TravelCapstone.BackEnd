using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    public class ProvinceController : Controller
    {
        private readonly IProvinceService _service;
        public ProvinceController(IProvinceService provinceService)
        {
            _service = provinceService;
        }

        [HttpGet("get-all-province-by-private-tour-request-id/{id}")]
        public async Task<AppActionResult> getAllProvinceByPrivateTourRequestId(Guid id)
        {
            return await _service.getAllProvinceByPrivateTourRequestId(id);
        }

    }
}
