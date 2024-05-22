using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("assurance")]
    [ApiController]
    public class AssuranceController : ControllerBase
    {
        private IAssuranceService _service;
        public AssuranceController(IAssuranceService service)
        {
            _service = service;
        }

        [HttpGet("get-available-assurances-with-num-of-day")]
        public async Task<AppActionResult> GetAvailableAssuranceList(int NumOfDay)
        {
            return await _service.GetAvailableAssurance(NumOfDay);
        }
    }
}
