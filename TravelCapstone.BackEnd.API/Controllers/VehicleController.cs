using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;   
        }
        [HttpGet("get-available-vehicle")]
        public async Task<AppActionResult> GetAvailableVehicle(VehicleType type, DateTime startTime, DateTime endTime, int pageNumber = 1, int pageSize = 10)
        {
            return await _vehicleService.GetAvailableVehicle(type, startTime, endTime, pageNumber, pageSize);       
        }
        [HttpPost("get-price-for-vehicle")]
        public async Task<AppActionResult> GetPriceForVehicle(FilterTransportServiceRequest filter)
        {
            return await _vehicleService.GetPriceForVehicle(filter);
        }

        [HttpGet("get-available-vehicle-type/{provinceStartPointId}/{provinceEndPointId}")]
        public async Task<AppActionResult> GetAvailableVehicleType(Guid provinceStartPointId, Guid provinceEndPointId)
        {
            return await _vehicleService.GetAvailableVehicleType(provinceStartPointId, provinceEndPointId);
        }
    }
}
