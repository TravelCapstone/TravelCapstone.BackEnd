using Microsoft.AspNetCore.Http;
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
    public interface IVehicleService
    {
        //public Task<AppActionResult> UploadQuotation(IFormFile file);
        //public Task<AppActionResult> ValidateExcelFile(IFormFile file);
        public Task<IActionResult> GetPriceQuotationTemplate();
        public Task<AppActionResult> GetAvailableVehicle(VehicleType type, DateTime startTime, DateTime endTime, int pageNumber, int pageSize);
        public Task<AppActionResult> GetPriceForVehicle(FilterTransportServiceRequest filter);
        public Task<AppActionResult> GetAvailableVehicleType(Guid provinceStartPointId, Guid provinceEndPointId);
    }
}
