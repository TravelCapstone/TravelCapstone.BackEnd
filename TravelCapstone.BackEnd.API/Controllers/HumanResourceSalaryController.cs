using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("human-resource-salary")]
    [ApiController]
    public class HumanResourceSalaryController : ControllerBase
    {
        private IHumanResourceFeeService _service;
        public HumanResourceSalaryController(IHumanResourceFeeService service)
        {
            _service = service;
        }

        [HttpPost("get-salary")]
        public async Task<AppActionResult> GetSalary([FromBody]List<HumanResourceCost> dtos, bool isForTourguide)
        {
            return await _service.GetSalary(dtos, isForTourguide);
        }
    }
}
