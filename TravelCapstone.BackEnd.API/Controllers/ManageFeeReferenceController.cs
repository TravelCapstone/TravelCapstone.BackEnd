using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("manage-fee-reference")]
    [ApiController]
    public class ManageFeeReferenceController : ControllerBase
    {
        private IManageFeeReferenceService _service;
        public ManageFeeReferenceController(IManageFeeReferenceService service)
        {
            _service = service;
        }

        [HttpGet("get-operation-fees/{quantity:int}")]
        public async Task<AppActionResult> GetOperationFees(int quantity)
        {
            return await _service.GetOperationFees(quantity);
        }
    }
}