using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    public class DishController : Controller
    {
        private IDishService _dishService;
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet("get-dish-list-by-menu-id/{menuId}")]
        public async Task<AppActionResult> GetDishListByMenuId(Guid menuId)
        {
            return await _dishService.GetDishListByMenuId(menuId);
        }
    }
}
