using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;   
        }
        [HttpGet("get-available-vehicle")]
        public async Task<AppActionResult> GetAvailableVehicle(DateTime startTime, DateTime endTime, int pageNumber = 1, int pageSize = 10)
        {
            return await _vehicleService.GetAvailableVehicle(startTime, endTime, pageNumber, pageSize);       
        }
    }
}
