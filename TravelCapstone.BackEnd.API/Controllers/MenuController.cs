using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    [Route("menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;     
        }

        [HttpGet("get-menu-by-facility/{pageNumber:int}/{pageSize:int}")]
        public async Task<AppActionResult> GetMenuByFacility(Guid facilityId, int pageNumber = 1, int pageSize = 10)
        {
            return await _menuService.GetMenuByFacilityId(facilityId, pageNumber, pageSize);        
        }
    }
}
