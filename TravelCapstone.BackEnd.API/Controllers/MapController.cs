using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

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

    [HttpGet("import-position-to-province")]
    public async Task<AppActionResult> ImportPosition()
    {
        return await _service.ImportPositionToProvince();
    }

    [HttpPost("get-optimal-path")]
    public async Task<AppActionResult> ImportPosition(Guid StartDestinationId, List<Guid> DestinationIds, bool IsPilgrimageTrip)
    {
        return await _service.FindOptimalPath(StartDestinationId, DestinationIds, IsPilgrimageTrip);
    }
    [HttpPost("get-estimate-trip-date")]
    public async Task<AppActionResult> DistanceMatrix(Guid StartDestinationId, List<Guid> DestinationIds, VehicleType vehicleType, DateTime startDate, DateTime endDate )
    {
        return await _service.GetEstimateTripDate(StartDestinationId, DestinationIds, vehicleType,  startDate,  endDate);           
    }
}