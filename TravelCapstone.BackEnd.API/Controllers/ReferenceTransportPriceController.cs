using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.API.Middlewares;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("reference-transport-price")]
    [ApiController]
    public class ReferenceTransportPriceController : ControllerBase
    {
        private IReferenceTransportService _referenceTransportService;
        public ReferenceTransportPriceController(IReferenceTransportService referenceTransportService)
        {
            _referenceTransportService = referenceTransportService; 
        }
        [HttpGet("get-all-reference-transport/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetAllReferenceTransport(int pageNumber = 1, int pageSize = 10) 
        { 
             return await _referenceTransportService.GetAllReferenceTransport(pageNumber , pageSize);        
        }
        [HttpPost("get-all-reference-transport-by-filter/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetAllReferenceTransportByProvinceId(FilterReferenceTransportPrice filter, int pageNumber = 1, int pageSize = 10)
        {
            return await _referenceTransportService.GetAllReferenceTransportByProvinceId(filter,pageNumber, pageSize);
        }
    }
}
