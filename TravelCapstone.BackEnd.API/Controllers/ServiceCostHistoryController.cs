using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Validator;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("service-cost-history")]
    public class ServiceCostHistoryController : Controller
    {
        private readonly IServiceCostHistoryService _service;
        public ServiceCostHistoryController(IServiceCostHistoryService serviceCostHistoryService)
        {
            _service = serviceCostHistoryService;
        }

        [HttpPost("upload-service-cost-quotation")]
        public async Task<AppActionResult> UploadQuotation(IFormFile file)
        {
            return await _service.UploadQuotation(file);
        }

        [HttpPost("validate-service-cost-quotation")]
        public async Task<AppActionResult> ValidateExcelFile(IFormFile file)
        {
            return await _service.ValidateExcelFile(file);
        }

        [HttpPost("upload-menu-service-cost-quotation")]
        public async Task<AppActionResult> UploadMenuQuotation(IFormFile file)
        {
            return await _service.UploadMenuQuotation(file);
        }

        [HttpPost("validate-menu-service-cost-quotation")]
        public async Task<AppActionResult> ValidateMenuExcelFile(IFormFile file)
        {
            return await _service.ValidateMenuExcelFile(file);
        }

        [HttpGet("get-template")]
        public async Task<IActionResult> GetTemplate()
        {
            return await _service.GetPriceQuotationTemplate();
        }

        [HttpGet("get-menu-price-template")]
        public async Task<IActionResult> GetMenuPriceQuotationTemplate()
        {
            return await _service.GetMenuPriceQuotationTemplate();
        }

        [HttpPost("get-last-cost-history")]
        public async Task<AppActionResult> GetLastCostHistory(List<Guid> guids)
        {
            return await _service.GetLastCostHistory(guids);
        }

        [HttpGet("get-service-cost-by-facility-Id-and-service-type/{facilityId}/{serviceTypeId}/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetServiceCostByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetServiceCostByFacilityIdAndServiceType(facilityId, serviceTypeId, pageNumber, pageSize);
        }

    }
}
