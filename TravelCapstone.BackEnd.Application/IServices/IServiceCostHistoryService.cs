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
    public interface IServiceCostHistoryService
    {
        public Task<AppActionResult> UploadQuotation(IFormFile file);
        public Task<AppActionResult> ValidateExcelFile(IFormFile file);
        public Task<AppActionResult> UploadMenuQuotation(IFormFile file);
        public Task<AppActionResult> ValidateMenuExcelFile(IFormFile file);
        public Task<IActionResult> GetPriceQuotationTemplate();
        public Task<IActionResult> GetMenuPriceQuotationTemplate();
        Task<AppActionResult> GetLastCostHistory(List<Guid> servicesId);
        public Task<AppActionResult> GetServiceCostByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId, int pageNumber, int pageSize);
    }

}
