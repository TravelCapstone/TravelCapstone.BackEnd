using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.API.Middlewares;
using TravelCapstone.BackEnd.Application.IServices;
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
        [CacheAttribute(60 * 60 *60 * 24)]
        [HttpGet("get-all-reference-transport/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetAllReferenceTransport(int pageNumber = 1, int pageSize = 10) 
        { 
             return await _referenceTransportService.GetAllReferenceTransport(pageNumber , pageSize);        
        }
        [CacheAttribute(60 * 60 * 60 * 24)]
        [HttpGet("get-all-reference-transport-by-province-id/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetAllReferenceTransportByProvinceId(Guid firstProvince, Guid secondProvince, int pageNumber = 1, int pageSize = 10)
        {
            return await _referenceTransportService.GetAllReferenceTransportByProvinceId(pageNumber, pageSize, firstProvince, secondProvince);
        }
    }
}
