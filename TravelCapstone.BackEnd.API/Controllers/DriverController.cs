using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    public class DriverController : Controller
    {
        private readonly IDriverService _driverService;
        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet("get-available-driver")]
        public async Task<AppActionResult> GetAvailableDriver(DateTime startTime, DateTime endTime)
        {
            return await _driverService.GetAvailableDriver(startTime, endTime);
        }
    }
}
