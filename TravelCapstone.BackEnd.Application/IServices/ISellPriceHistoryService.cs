using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface ISellPriceHistoryService
    {
        public Task<AppActionResult> GetMinMaxPriceOfHotel(Guid districtId, Guid privatetourRequestId, Guid ratingId, int numOfDay, int pageNumber, int pageSize);
        public Task<AppActionResult> GetPriceOfMeal(Guid districtId, Guid privatetourRequestId, Guid ratingId, int numOfMeal, int pageNumber, int pageSize);
        public Task<AppActionResult> GetPriceOfVehicle(Guid districtId, Guid privatetourRequestId, int numOfDay, VehicleType vehicleType, int pageNumber, int pageSize);
        public Task<AppActionResult> GetSellPriceByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId, int pageNumber, int pageSize);
        public Task<AppActionResult> UploadQuotation(IFormFile file);
        public Task<AppActionResult> ValidateExcelFile(IFormFile file);
        public Task<IActionResult> GetTemplate();
    }
}
