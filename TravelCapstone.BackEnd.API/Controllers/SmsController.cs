using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private ISmsService _service;

        public SmsController(ISmsService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<AppActionResult> Post([FromBody] string body)
        {
            return await _service.SendMessage(body);
        }
    }
}