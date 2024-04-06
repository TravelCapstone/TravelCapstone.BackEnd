using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.API.Middlewares;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

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
    [Cache(60 *24)]
    [HttpGet("get-all/{pageNumber:int}/{pageSize:int}")]
    public async Task<AppActionResult> GetAll(string? keyword, int pageNumber = 1, int pageSize = 10)
    {
        return await _service.GetAll(keyword, pageNumber, pageSize);
    }
}