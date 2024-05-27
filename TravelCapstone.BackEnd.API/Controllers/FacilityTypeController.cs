using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("facility-type")]
    [ApiController]
    public class FacilityTypeController : ControllerBase
    {
        private IFacilityTypeService _facilityTypeService;

        public FacilityTypeController(IFacilityTypeService facilityTypeService)
        {
            _facilityTypeService = facilityTypeService;
        }
        [HttpGet("get-all-facility-type")]
        public async Task<AppActionResult> GetAllFacilityType()
        {
            return await _facilityTypeService.GetAllFacilityType();
        }
        [HttpGet("get-all-facility-rating-by-facilityTypeId/{id}")]
        public async Task<AppActionResult> GetAllFacilityRatingByFacilityTypeId(TravelCapstone.BackEnd.Domain.Enum.FacilityType id)
        {
            return await _facilityTypeService.GetAllFacilityRatingByFacilityTypeId(id);
        }
    }
}
