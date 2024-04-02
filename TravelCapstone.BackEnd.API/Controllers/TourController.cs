using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO;

namespace TravelCapstone.BackEnd.API.Controllers;

[Route("tour")]
[ApiController]
public class TourController : ControllerBase
{
    private readonly ITourService _service;

    public TourController(ITourService service)
    {
        _service = service;
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<AppActionResult> GetById(Guid id)
    {
        return await _service.GetById(id);
    }
}