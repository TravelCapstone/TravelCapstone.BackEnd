using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("facility")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private IFacilityService _facilityService;

        public FacilityController(IFacilityService facilityService)
        {
            _facilityService = facilityService;
        }
        [HttpGet("get-all-facility-by-provinceId")]
        public async Task<AppActionResult> GetAllFacilityByPronvinceId(Guid provinceId,ServiceType serviceType)
        {
            return await _facilityService.GetAllFacilityByPronvinceId(provinceId, serviceType);
                
        }
    }
}
