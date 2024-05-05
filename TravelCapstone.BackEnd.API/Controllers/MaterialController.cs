using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers
{
    public class MaterialController : Controller
    {
        private IMaterialService _materialService;
        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpPost("add-material-to-tour")]
        public async Task<AppActionResult> AddMaterialtoTour(AddMaterialRequest request)
        {
            return await _materialService.AddMaterialtoTour(request);
        }
    }
}
