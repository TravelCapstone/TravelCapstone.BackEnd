using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    public class DistrictController : Controller
    {
        private readonly IDistrictService _service;
        public DistrictController(IDistrictService provinceService)
        {
            _service = provinceService;
        }

        [HttpGet("get-all-province-by-private-tour-request-id/{id}")]
        public async Task<AppActionResult> GetAllProvinceByPrivateTourRequestId(Guid id)
        {
            return await _service.GetAllDistrictByPrivateTourRequestId(id);
        }

    }
}
