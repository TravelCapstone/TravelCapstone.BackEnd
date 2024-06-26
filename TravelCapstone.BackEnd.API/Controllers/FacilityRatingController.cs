using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
	[Route("facility-rating")]
	[ApiController]
	public class FacilityRatingController : ControllerBase
	{
		private IFacilityRatingService _service;
		public FacilityRatingController(IFacilityRatingService service)
		{
			_service = service;
		}

		[HttpPost("get-facility-rating-by-location")]
		public async Task<AppActionResult> GetFacilityRatingByLocation([FromBody]LocationRequestDto dto)
		{
			return await _service.GetFacilityRatingByLocation(dto);
		}
	}
}
