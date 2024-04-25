using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Validator;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.API.Controllers
{
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

        [HttpGet("get-template")]
        public async Task<IActionResult> GetTemplate()
        {
            return await _service.GetPriceQuotationTemplate();
        }


        [HttpPost("get-last-cost-history")]
        public async Task<AppActionResult> GetLastCostHistory(List<Guid> guids)
        {
            return await _service.GetLastCostHistory(guids);
        }

        [HttpGet("get-service-cost-by-facility-Id-and-service-type")]
        public async Task<AppActionResult> GetServiceCostByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId)
        {
            return await _service.GetServiceCostByFacilityIdAndServiceType(facilityId, serviceTypeId);
        }

    }
}
