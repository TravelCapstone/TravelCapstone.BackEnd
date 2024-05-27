using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IEventService _service;
        public EventController(IEventService service)
        {
            _service = service;
        }

        [HttpGet("get-event-list-with-quantity")]
        public async Task<AppActionResult> GetEventListWithQuantity(int quantity)
        {
            return await _service.GetEventListWithQuantity(quantity);
        }

        [HttpPost("create-custom-event-string")]
        public async Task<AppActionResult> CreateCustomEventString(CreateCustomEventStringRequest dto)
        {
            return await _service.CreateCustomEventString(dto);
        }
    }
}
