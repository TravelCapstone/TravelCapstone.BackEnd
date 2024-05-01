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
        public async Task<AppActionResult> GetSellPriceByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetSellPriceByFacilityIdAndServiceType(facilityId, serviceTypeId, pageNumber, pageSize);
        }

        [HttpGet("get-min-max-price-of-hotel/{districtId}/{privatetourRequestId}/{ratingId}/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetMinMaxPriceOfHotel(Guid districtId, Guid privatetourRequestId, Guid ratingId, int numOfDay = 1, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetMinMaxPriceOfHotel(districtId, privatetourRequestId, ratingId, numOfDay, pageNumber, pageSize);
        }

        [HttpGet("get-price-of-meal/{districtId}/{privatetourRequestId}/{ratingId}")]
        public async Task<AppActionResult> GetPriceOfMeal(Guid districtId, Guid privatetourRequestId, Guid ratingId, int numOfMeal = 1, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetPriceOfMeal(districtId, privatetourRequestId, ratingId, numOfMeal, pageNumber, pageSize);
        }

        [HttpGet("get-price-of-vehicle/{districtId}/{privatetourRequestId}")]
        public async Task<AppActionResult> GetPriceOfVehicle(Guid districtId, Guid privatetourRequestId, VehicleType vehicleType ,int numOfDay = 1, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetPriceOfVehicle(districtId, privatetourRequestId, numOfDay, vehicleType, pageNumber, pageSize);
        }

        [HttpGet("get-reference-transport-by-province/{startPoint}/{endPoint}")]
        public async Task<AppActionResult> GetReferenceTransportByProvince(Guid startPoint, Guid endPoint, VehicleType vehicleType, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetReferenceTransportByProvince(startPoint, endPoint, vehicleType, pageNumber, pageSize);
        }

        [HttpGet("get-attraction-sell-price-range/{districtId}/{privateTourRequestId}/{numOfPlace:int}/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetAttractionSellPriceRange(Guid districtId, Guid privateTourRequestId, int numOfPlace, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetAttractionSellPriceRange(districtId, privateTourRequestId, numOfPlace, pageNumber, pageSize);
        }
    }
}
