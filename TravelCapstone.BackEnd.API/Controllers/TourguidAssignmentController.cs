using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("tour-guide-assignment")]
    [ApiController]
    public class TourguidAssignmentController : ControllerBase
    {
        private ITourguideAssignmentsService _tourguideAssignmentsService;
        public TourguidAssignmentController(ITourguideAssignmentsService tourguideAssignmentsService)
        {
            _tourguideAssignmentsService = tourguideAssignmentsService; 
        }

        [HttpGet("get-unassign-tour-guide-by-province")]
        public async Task<AppActionResult> GetUnassignTourGuideByProvince(Guid provinceId)
        {
            return await _tourguideAssignmentsService.GetUnassignTourGuideByProvince(provinceId);
        }

        [HttpGet("get-available-tour-guide/{provinceId}")]
        public async Task<AppActionResult> GetAvailableTourGuide(Guid provinceId, DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 10)
        {
            return await _tourguideAssignmentsService.GetAvailableTourGuide(provinceId, startDate, endDate, pageNumber, pageSize);
        }

        [HttpGet("get-max-tour-guide-number")]
        public async Task<AppActionResult> GetMaxTourGuideNumber(int numOfVehicle, Guid preValueId)
        {
            return await _tourguideAssignmentsService.GetMaxTourGuideNumber(numOfVehicle, preValueId);
        }
    }
}
