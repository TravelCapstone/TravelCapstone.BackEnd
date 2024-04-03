using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers;

[Route("api/map")]
[ApiController]
public class MapController : ControllerBase
{
    private readonly IMapService _service;

    public MapController(IMapService service)
    {
        _service = service;
    }

    [HttpGet("geocode")]
    public async Task<AppActionResult> Geocode(string address)
    {
        return await _service.Geocode(address);
    }

    [HttpGet("auto-complete")]
    public async Task<AppActionResult> AutoComplete(string address)
    {
        return await _service.AutoComplete(address);
    }
}