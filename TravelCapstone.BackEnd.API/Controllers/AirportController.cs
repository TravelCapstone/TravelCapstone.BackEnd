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
        [HttpGet("get-all-airport")]
        public async Task<AppActionResult> GetAll()
        {
            return new AppActionResult
            {

                Result = new List<object>() {
                    new  { Name = "Hà Nội", Code = "HAN" },
                    new  { Name = "Hồ Chí Minh", Code = "SGN" },
                    new  { Name = "Đà Nẵng", Code = "DAD" },
                    new  { Name = "Điện Biên Phủ", Code = "DIN" },
                    new  { Name = "Hải Phòng", Code = "HPH" },
                    new  { Name = "Thanh Hóa", Code = "THD" },
                    new  { Name = "Vinh", Code = "VII" },
                    new  { Name = "Quảng Bình", Code = "VDH" },
                    new  { Name = "Quảng Nam", Code = "VCL" },
                    new  { Name = "Huế", Code = "HUI" },
                    new  { Name = "PleiKu", Code = "PXU" },
                    new  { Name = "Phú Yên", Code = "TBB" },
                    new  { Name = "Ban Mê Thuột", Code = "BMV" },
                    new  { Name = "Nha Trang", Code = "CXR" },
                    new  { Name = "Qui Nhơn", Code = "UIH" },
                    new  { Name = "Đà Lạt", Code = "DLI" },
                    new  { Name = "Cần Thơ", Code = "VCA" },
                    new  { Name = "Kiên Giang", Code = "VKG" },
                    new  { Name = "Cà Mau", Code = "CAH" },
                    new  { Name = "Phú Quốc", Code = "PQC" },
                    new  { Name = "Côn Đảo", Code = "VCS" },
                    new  { Name = "Vân Đồn", Code = "VDO" }
                },
                IsSuccess = true,
            };

        }


    }
}
