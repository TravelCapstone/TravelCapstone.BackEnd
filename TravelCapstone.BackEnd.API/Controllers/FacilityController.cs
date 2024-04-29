using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Application.Services;
using TravelCapstone.BackEnd.Common.DTO.Request;
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

        [HttpPost("get-all-facility-by-filter")]
        public async Task<AppActionResult> GetFacilityByProvinceId([FromBody]FilterLocation filter, int pageNumber=1, int pageSize = 10)
        {
            return await _service.GetFacilityByProvinceId(filter,pageNumber,pageSize);
        }
        [HttpGet("get-all-facility")]
        public async Task<AppActionResult> GetAllFacility( int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetAllFacility( pageNumber, pageSize);
        }
        [HttpPost("get-all-facility-by-location-and-ratingId/{ratingId}")]
        public async Task<AppActionResult> GetAllFacilityByRatingId([FromBody] FilterLocation filter, TravelCapstone.BackEnd.Domain.Enum.Rating ratingId,int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetAllFacilityByRatingId(filter,ratingId,pageNumber, pageSize);
        }
    }
    }
