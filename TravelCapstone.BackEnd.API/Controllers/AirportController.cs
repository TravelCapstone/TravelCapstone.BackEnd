using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Enum;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("airport")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private IAirportService _service;

        public AirportController(IAirportService service)
        {
            _service = service;
        }

        [HttpGet("search-airport")]
        public async Task<AppActionResult> SearchAiport(string keyword)
        {
            // return await _service.SearchAirport(keyword);
            return new AppActionResult()
            {

            };
        }
        [HttpPost("search-airport")]
        public async Task<AppActionResult> SearchAirFlight(List<AirlineType> airlineTypes, string startCode, string endCode,DateTime date)
        {
            //return await _service.SearchAirFlight(airlineTypes,startCode,endCode,date);
            return new AppActionResult()
            {

            };
        }

    }
}
