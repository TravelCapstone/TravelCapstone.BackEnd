using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("configuration")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _service;
        public ConfigurationController(IConfigurationService provinceService)
        {
            _service = provinceService;
        }

        [HttpGet("get-min-quantity-for-tour-creation/{isForFamily}")]
        public async Task<AppActionResult> GetMinQuantityForTourCreation(bool isForFamily)
        {
            return await _service.GetMinQuantityForTourCreation(isForFamily);
        }

    }
}
