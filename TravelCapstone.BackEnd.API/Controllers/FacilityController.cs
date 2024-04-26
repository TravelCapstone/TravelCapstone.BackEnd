using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Application.Services;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("facility")]
    public class FacilityController : Controller
    {
        private readonly IFacilityService _service;
        public FacilityController(IFacilityService service)
        {
            _service = service;
        }

        [HttpGet("get-all-facility-by-province-Id/{provinceId}")]
        public async Task<AppActionResult> GetFacilityByProvinceId(Guid provinceId, int pageNumber=1, int pageSize = 10)
        {
            return await _service.GetFacilityByProvinceId(provinceId,pageNumber,pageSize);
        }
    }
    }
