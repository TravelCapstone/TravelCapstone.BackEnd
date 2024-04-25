using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("sell-price-history-controller")]
    [ApiController]
    public class SellPriceHistoryController : ControllerBase
    {
        private ISellPriceHistoryService _sellPriceHistoryService;
        public SellPriceHistoryController(ISellPriceHistoryService sellPriceHistoryService)
        {
            _sellPriceHistoryService = sellPriceHistoryService; 
        }

        [HttpGet("get-hotel-price-by-rating/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetHotelPriceByRating(Guid ratingId, int pageNumber = 1, int pageSize = 10)
        {
            return await _sellPriceHistoryService.GetHotelPriceByRating(ratingId, pageNumber, pageSize);       
        }

        [HttpGet("get-all-entertainment-price-by-facility-id/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetAllEntertainmentPriceByFacilityId(Guid facilityId, int pageNumber = 1, int pageSize = 10)
        {
            return await _sellPriceHistoryService.GetAllEntertainmentPriceByFacilityId(facilityId, pageNumber, pageSize);
        }
    }
}
