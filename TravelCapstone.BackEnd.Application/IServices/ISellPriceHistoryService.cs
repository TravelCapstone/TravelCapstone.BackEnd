﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface ISellPriceHistoryService
    {
        public Task<AppActionResult> GetAveragePriceOfMealService(Guid districId, Guid privatetourRequestId, Guid ratingId, MealType mealType, int servingQuantity, int pageNumber, int pageSize);
        public Task<AppActionResult> GetAveragePriceOfService(Guid districId, Guid privatetourRequestId, Guid ratingId, ServiceType serviceType, int servingQuantity , int pageNumber, int pageSize);
        public Task<AppActionResult> GetMinMaxPriceOfHotel(Guid districtId, Guid privatetourRequestId, Guid ratingId, int numOfDay, int pageNumber, int pageSize);
        public Task<AppActionResult> GetPriceOfMeal(Guid districtId, Guid privatetourRequestId, Guid ratingId, int numOfMeal, int pageNumber, int pageSize);
        public Task<AppActionResult> GetPriceOfVehicle(Guid provinceId ,Guid privatetourRequestId, int numOfDay, VehicleType vehicleType, int pageNumber, int pageSize);
        public Task<AppActionResult> GetSellPriceByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId, int pageNumber, int pageSize);
        public Task<AppActionResult> GetAttractionSellPriceRange(Guid districtId, Guid privateTourRequestId, int numOfPlace, int pageNumber, int pageSize);
        public Task<AppActionResult> GetReferenceTransportByProvince(Guid startPoint, Guid endPoint , VehicleType vehicleType, int pageNumber, int pageSize);
        public Task<AppActionResult> GetHotelLatestPriceByDistrict(Guid districtId, Guid ratingId, int servingQuantity, int numOfServiceUse,int pageNumber, int pageSize);
        public Task<AppActionResult> GetVehiclePriceRange(VehiclePriceRangeRequest dto);
        public Task<AppActionResult> UploadQuotation(IFormFile file);
        public Task<AppActionResult> ValidateExcelFile(IFormFile file);
        public Task<IActionResult> GetTemplate();
        public Task<AppActionResult> GetServiceLatestPrice(Guid facilityServiceId);
        public Task<AppActionResult> GetMenuServiceLatestPrice(Guid menuId, int numOfServiceUse);
        public Task<AppActionResult> GetTransportServiceLatestPrice(Guid transportDetailId);
    }
}
