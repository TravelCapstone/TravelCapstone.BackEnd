using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("sell-price")]
    public class SellPriceHistoryController : Controller
    {
        private readonly ISellPriceHistoryService _service;
        public SellPriceHistoryController(ISellPriceHistoryService service)
        {
            _service = service;
        }


        [HttpGet("get-sell-price-by-facility-Id-and-service-type/{facilityId}/{serviceTypeId}/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetServiceCostByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetSellPriceByFacilityIdAndServiceType(facilityId, serviceTypeId, pageNumber, pageSize);
        }

        [HttpGet("get-attraction-sell-price-range/{districtId}/{privateTourRequestId}/{numOfPlace:int}/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetAttractionSellPriceRange(Guid districtId, Guid privateTourRequestId, int numOfPlace, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetAttractionSellPriceRange(districtId, privateTourRequestId, numOfPlace, pageNumber, pageSize);
        }
    }
}
