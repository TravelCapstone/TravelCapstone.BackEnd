using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
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

        [HttpGet("get-sell-price-by-facility-service-id/{facilityServiceId}")]
        public async Task<AppActionResult> GetServiceLatestPrice(Guid facilityServiceId)
        {
            return await _service.GetServiceLatestPrice(facilityServiceId);
        }

        [HttpGet("get-sell-price-by-menu-id/{menuId}/{numOfServiceUse}")]
        public async Task<AppActionResult> GetMenuServiceLatestPrice(Guid menuId, int numOfServiceUse)
        {
            return await _service.GetMenuServiceLatestPrice(menuId, numOfServiceUse);
        }

        [HttpGet("get-sell-price-by-transport-service-id/{transportDetailId}")]
        public async Task<AppActionResult> GetTransportServiceLatestPrice(Guid transportDetailId)
        {
            return await _service.GetTransportServiceLatestPrice(transportDetailId);
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

        [HttpGet("get-average-price-of-service/{districId}/{privatetourRequestId}/{ratingId}")]
        public async Task<AppActionResult> GetAveragePriceOfService(Guid districId, Guid privatetourRequestId, Guid ratingId, ServiceType serviceType, int servingQuantity, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetAveragePriceOfService(districId, privatetourRequestId, ratingId, serviceType, servingQuantity, pageNumber, pageSize);
        }

        [HttpGet("get-average-price-of-meal-service/{districId}/{privatetourRequestId}/{ratingId}")]
        public async Task<AppActionResult> GetAveragePriceOfMealService(Guid districId, Guid privatetourRequestId, Guid ratingId, MealType mealType, int servingQuantity, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetAveragePriceOfMealService(districId, privatetourRequestId, ratingId, mealType, servingQuantity, pageNumber, pageSize);
        }

        [HttpPost("get-vehicle-price-range")]
        public async Task<AppActionResult> GetVehiclePriceRange([FromBody]VehiclePriceRangeRequest dto)
        {
            return await _service.GetVehiclePriceRange(dto);
        }

        [HttpGet("get-latest-hotel-price/{districtId}/{ratingId}/{servingQuantity}/{numOfServiceUse}/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetHotelLatestPriceByDistrict(Guid districtId, Guid ratingId, int servingQuantity, int numOfServiceUse, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetHotelLatestPriceByDistrict(districtId, ratingId, servingQuantity, numOfServiceUse,pageNumber, pageSize);
        }

        [HttpGet("get-latest-hotel-price/{districtId}/{privateTourRequestId}")]
        public async Task<AppActionResult> GetEntertainmentLatestPrice(Guid districtId, Guid privateTourRequestId)
        {
            return await _service.GetEntertainmentLatestPrice(districtId, privateTourRequestId);
        }
    }
}
