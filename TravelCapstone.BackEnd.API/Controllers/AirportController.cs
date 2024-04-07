using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.AirTransport;
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
        public async Task<AppActionResult> SearchAirFlight(FlightSearchRequestDto request)
        {
         //   return await _service.SearchAirFlight(request);
            return new AppActionResult()
            {

            };

        }
        [HttpGet("get-all-airline")]
        public async Task<AppActionResult> GetAllAirline()
        {
            return new AppActionResult
            {

                Result = new List<object>() {
                     new   {Name= "VietJet", Code= "VJ" },
                     new   {Name= "VietNam Airline", Code= "VN" },
                     new   {Name= "Bamboo", Code= "QH" },
                     new   {Name= "VietTravel Airline", Code= "VU" },
                },
                IsSuccess = true,
            };

        }

    }
}
