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


        [HttpGet("get-sell-price-by-facility-Id-and-service-type")]
        public async Task<AppActionResult> GetServiceCostByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId)
        {
            return await _service.GetSellPriceByFacilityIdAndServiceType(facilityId, serviceTypeId);
        }
    }
}
