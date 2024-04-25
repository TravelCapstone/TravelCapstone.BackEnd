using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Application.Services;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    public class FacilityController : Controller
    {
        private readonly FacilityService _service;
        public FacilityController(FacilityService service)
        {
            _service = service;
        }

        [HttpGet("get-all-facility-by-province-Id/{provinceId}")]
        public async Task<AppActionResult> GetFacilityByProvinceId(Guid provinceId)
        {
            return await _service.GetFacilityByProvinceId(provinceId);
        }
    }
    }
